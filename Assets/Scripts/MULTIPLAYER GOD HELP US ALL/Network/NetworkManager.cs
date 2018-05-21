using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager de todo los callbacks para el Networking de Photon
/// </summary>
public class NetworkManager : Photon.PunBehaviour, IPunObservable {

    public static NetworkManager Instance = null;

    public GameController controller;
    public GameObject playerInstancePrefab;
    public PlayerInstancesManager playerInstanceManager;
    public string gameVersion = "0.1";
    public byte maxPlayers = 10;

    [HideInInspector] public bool isConnecting;
    [HideInInspector] public bool connected;


    public delegate void PlayerInstancesChanges(PlayerInstance playerInstance, GameController controller);
    public static event PlayerInstancesChanges OnExamine;

    private void Awake()
    {
        isConnecting = false;
        connected = false;
        Instance = this;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void ConnectToServer()
    {
        isConnecting = true;

        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayers }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado a habitación. ");
        isConnecting = false;
        connected = true;

        PhotonNetwork.player.NickName = controller.playerManager.playerName;

        string[] newPlayer = StoreMyPlayerData();

        photonView.RPC("NewPlayerJoined", PhotonTargets.Others, newPlayer, PhotonNetwork.player.ID);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        PlayerDisconnected(otherPlayer.NickName);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public string[] StoreMyPlayerData()
    {
        return new string[]
        {
            controller.playerManager.playerName,
            PhotonNetwork.player.ID.ToString(),

            controller.playerManager.gender,
            controller.playerManager.playerLevel.ToString(),

            controller.playerManager.characteristics.playerJob.jobName,
            controller.playerManager.characteristics.playerRace.raceName,
            controller.playerManager.currentState.stateName,

            controller.playerManager.characteristics.currentStrength.ToString(),
            controller.playerManager.characteristics.currentIntelligence.ToString(),
            controller.playerManager.characteristics.currentResistance.ToString(),
            controller.playerManager.characteristics.currentDexterity.ToString(),

            controller.playerManager.currentHealth.ToString(),

            controller.playerManager.currentVisibility.ToString(),

            controller.playerRoomNavigation.currentPosition.ToString()

        };
    }

    public PlayerInstance CreatePlayerInstance(string[] playerData)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerData[0]))
        {
            Debug.Log("Player already exists");
            return null;
        }

        PlayerInstance newPlayer = Instantiate(playerInstancePrefab).GetComponent<PlayerInstance>();

        UpdatePlayerInstancesStats(newPlayer, playerData);

        playerInstanceManager.playerInstancesOnScene.Add(newPlayer.playerName, newPlayer);

        return newPlayer;
    }

    public void UpdatePlayerInstancesStats(PlayerInstance newPlayer, string[] playerData)
    {
        newPlayer.playerName = playerData[0];
        Int32.TryParse(playerData[1], out newPlayer.playerUserID);

        newPlayer.playerGender = playerData[2];
        Int32.TryParse(playerData[3], out newPlayer.playerLevel);

        newPlayer.playerJob = StringsIntoObjectsInator.Instance.JobFromString(playerData[4]);
        newPlayer.playerRace = StringsIntoObjectsInator.Instance.RaceFromString(playerData[5]);
        newPlayer.playerState = StringsIntoObjectsInator.Instance.StateFromString(playerData[6]);

        float.TryParse(playerData[7], out newPlayer.strength);
        float.TryParse(playerData[8], out newPlayer.intelligence);
        float.TryParse(playerData[9], out newPlayer.resistance);
        float.TryParse(playerData[10], out newPlayer.dexterity);

        float.TryParse(playerData[11], out newPlayer.currentHealth);

        Int32.TryParse(playerData[12], out newPlayer.currentVisibility);

        newPlayer.currentRoom = RoomsChecker.RoomObjectFromVector(
            RoomsChecker.RoomPositionFromText(playerData[13])
            );
    }

    [PunRPC]
    public void InstantiateAlreadyExistingPlayers(string[] playerData)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerData[0]))
        {
            Debug.Log("Player already exists here");
            return;
        }

        Debug.Log("Player entered");
        PlayerInstance oldPlayer = CreatePlayerInstance(playerData);

        if (oldPlayer.currentRoom != null)
        {
            oldPlayer.currentRoom.AddPlayerInRoom(oldPlayer);

            if (oldPlayer.currentRoom == controller.playerRoomNavigation.currentRoom)
            {
                Debug.Log("Jugador en la habitación");
                controller.playerRoomNavigation.ShowPlayersInRoom();
            }
        }
    }

    [PunRPC]
    public void NewPlayerJoined(string[] playerData, int playerID)
    {
        Debug.Log("Player entered");
        PlayerInstance newPlayer = CreatePlayerInstance(playerData);

        if (newPlayer.currentRoom != null)
        {
            newPlayer.currentRoom.PlayerEnteredRoom(newPlayer, controller);
        }

        photonView.RPC("InstantiateAlreadyExistingPlayers", PhotonPlayer.Find(playerID), StoreMyPlayerData());

    }

    public void PlayerDisconnected(string playerName)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance oldPlayer = playerInstanceManager.playerInstancesOnScene[playerName];

            if (oldPlayer.currentRoom == controller.playerRoomNavigation.currentRoom)
            {
                controller.LogStringWithoutReturn(playerName + " se ha desvanecido frente a tus ojos.");
            }

            oldPlayer.currentRoom.RemovePlayerInRoom(oldPlayer);
            Destroy(playerInstanceManager.playerInstancesOnScene[playerName].gameObject);

            playerInstanceManager.playerInstancesOnScene.Remove(playerName);
        }
    }

    #region Players Exploring Methods

    public void MyPlayerChangedRooms(string playerID, Vector3 newPosition)
    {
        photonView.RPC("PlayerChangedRoom", PhotonTargets.Others, playerID, newPosition.ToString());
    }

    [PunRPC]
    public void PlayerChangedRoom(string playerName, string newRoomPosition)
    {
        if (RoomsChecker.roomsDictionary.ContainsKey(
            RoomsChecker.RoomPositionFromText(newRoomPosition)
            ))
        {
            if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
            {
                PlayerInstance otherPlayer = playerInstanceManager.playerInstancesOnScene[playerName];

                //Se borra al jugador de la habitación, revisando que nosotros no estuvieramos ahi
                otherPlayer.currentRoom.PlayerLeftRoom(
                    playerInstanceManager.playerInstancesOnScene[playerName], controller);

                //Se cambia la habitación actual del jugador
                otherPlayer.currentRoom =
                    RoomsChecker.RoomObjectFromVector(
                    RoomsChecker.RoomPositionFromText(newRoomPosition)
                    );

                //Se agrega al jugador a la nueva habitación
                otherPlayer.currentRoom.PlayerEnteredRoom(
                    playerInstanceManager.playerInstancesOnScene[playerName],
                    controller
                    );
            }
        }
    }

    #endregion


    #region Players say something

    public void SayThingInRoom(string thingToSay, string playerName)
    {
        photonView.RPC("ThingBeingSaidToeveryone", PhotonTargets.Others, thingToSay, playerName);
    }

    public void SayThingInRoomToPlayer(string thingToSay, string myPlayerName, int otherPlayerID)
    {
        photonView.RPC("ThingBeingSaidToSomeone", PhotonPlayer.Find(otherPlayerID), thingToSay, myPlayerName, otherPlayerID);
    }

    [PunRPC]
    public void ThingBeingSaidToeveryone(string thingSaid, string playerName)
    {
        if (!playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            return;
        }

        PlayerInstance speakingPlayer = playerInstanceManager.playerInstancesOnScene[playerName];

        if (controller.playerRoomNavigation.currentRoom.playersInRoom.Contains(speakingPlayer))
        {
            string thingSomeoneSaid = string.Format("{0}: \"{1}\" ", playerName, thingSaid);
            controller.LogStringWithoutReturn(thingSomeoneSaid);
        }
    }

    [PunRPC]
    public void ThingBeingSaidToSomeone(string thingSaid, string playerName, int otherPlayerID)
    {
        if (!playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            return;
        }

        PlayerInstance speakingPlayer = playerInstanceManager.playerInstancesOnScene[playerName];

        if (controller.playerRoomNavigation.currentRoom.playersInRoom.Contains(speakingPlayer))
        {
            string thingSomeoneSaid = string.Format("{0} te dice: \"{1}\" ", playerName, thingSaid);
            controller.LogStringWithoutReturn(thingSomeoneSaid);
        }
    }

    #endregion

    #region Player Examination

    public void AskForCurrentStats(string playerName)
    {
        if (!playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            return;
        }

        PlayerInstance player = playerInstanceManager.playerInstancesOnScene[playerName];


        photonView.RPC("UpdateInstance", PhotonPlayer.Find(player.playerUserID), PhotonNetwork.player.ID);
    }

    [PunRPC]
    public void UpdateInstance(int playerID)
    {
        string[] myUpdate = StoreMyPlayerData();

        photonView.RPC("ExamineTarget", PhotonPlayer.Find(playerID), controller.playerManager.playerName, myUpdate);
    }

    [PunRPC]
    public void ExamineTarget(string playerName, string[] playerUpdated)
    {
        PlayerInstance player = playerInstanceManager.playerInstancesOnScene[playerName];

        UpdatePlayerInstancesStats(player, playerUpdated);

        if (OnExamine != null)
        {
            OnExamine(player, controller);
        }
    }

    #endregion

    #region Player Fight



    #endregion
}

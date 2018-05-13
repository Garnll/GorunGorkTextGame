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
            controller.playerManager.gender,
            controller.playerManager.playerLevel.ToString(),
            controller.playerManager.currentHealth.ToString(),
            controller.playerManager.currentVisibility.ToString(),
            controller.playerRoomNavigation.currentPosition.ToString(),
            PhotonNetwork.player.ID.ToString()
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

        newPlayer.playerName = playerData[0];
        newPlayer.playerGender = playerData[1];
        Int32.TryParse(playerData[2], out newPlayer.playerLevel);
        Int32.TryParse(playerData[3], out newPlayer.currentHealth);
        Int32.TryParse(playerData[4], out newPlayer.currentVisibility);
        newPlayer.currentRoom = RoomsChecker.RoomObjectFromVector(
            RoomsChecker.RoomPositionFromText(playerData[5])
            );
        Int32.TryParse(playerData[6], out newPlayer.playerUserID);



        playerInstanceManager.playerInstancesOnScene.Add(newPlayer.playerName, newPlayer);

        return newPlayer;
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

    #endregion
}

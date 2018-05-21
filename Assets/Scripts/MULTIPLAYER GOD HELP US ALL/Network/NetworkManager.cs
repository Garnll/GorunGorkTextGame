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

        PlayerInstance oldPlayer = CreatePlayerInstance(playerData);

        if (oldPlayer.currentRoom != null)
        {
            oldPlayer.currentRoom.AddPlayerInRoom(oldPlayer);

            if (oldPlayer.currentRoom == controller.playerRoomNavigation.currentRoom)
            {
                if (GameState.Instance.CurrentState != GameState.GameStates.combat)
                {
                    controller.playerRoomNavigation.ShowPlayersInRoom();
                }
            }
        }
    }

    [PunRPC]
    public void NewPlayerJoined(string[] playerData, int playerID)
    {
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

            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                if (controller.combatController.enemyPlayer == oldPlayer)
                {
                    controller.combatController.UpdateEnemyPlayerLog(oldPlayer.playerName + " se desvaneció...");
                    controller.combatController.EndCombatByEscaping(oldPlayer);
                }
            }
            else
            {
                if (oldPlayer.currentRoom == controller.playerRoomNavigation.currentRoom)
                {
                    controller.LogStringWithoutReturn(playerName + " se ha desvanecido frente a tus ojos.");
                }
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

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
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

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            return;
        }

        PlayerInstance speakingPlayer = playerInstanceManager.playerInstancesOnScene[playerName];

        if (controller.playerRoomNavigation.currentRoom.playersInRoom.Contains(speakingPlayer))
        {
            string thingSomeoneSaid = string.Format("De {0} a ti: \"{1}\" ", playerName, thingSaid);
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

    public void TryToAttack(string playerName)
    {   
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerName];

            photonView.RPC("PlayerAttackedMe", PhotonPlayer.Find(enemy.playerUserID), controller.playerManager.playerName);
        }
    }

    [PunRPC]
    public void PlayerAttackedMe(string challenger)
    {
        Debug.Log("Have been attacked");

        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(challenger))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[challenger];

            controller.LogStringWithoutReturn(challenger + " te está atacando.");
            controller.combatController.PrepareFight(enemy, controller.playerManager);
            controller.LogStringWithReturn(" ");
        }
    }

    public void StartFight(string playerName)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerName];

            photonView.RPC("FightStarted", PhotonPlayer.Find(enemy.playerUserID), controller.playerManager.playerName);
        }
    }

    [PunRPC]
    public void FightStarted(string challenger)
    {
        Debug.Log("Fight Start");
        controller.combatController.StartFightNow();
    }


    private float[] StoreMyCombatData()
    {
        return new float[]
        {
            controller.playerManager.MaxHealth,

            controller.playerManager.currentTurn,
            controller.playerManager.MaxTurn
        };
    }

    private void UpdatePlayerCombatStats(PlayerEnemyInstance playerStats, float[] stats)
    {
        playerStats.maxHealth = (int)stats[0];
        playerStats.currentTurn = stats[1];
        playerStats.maxTurn = (int)stats[2];
    }


    public void UpdatePlayerData(PlayerInstance enemy, PlayerManager me)
    {
        string[] commonPlayerData = StoreMyPlayerData();
        float[] combatPlayerData = StoreMyCombatData();

        photonView.RPC("PlayerHasUpdatedData", PhotonPlayer.Find(enemy.playerUserID), commonPlayerData, combatPlayerData);
    }

    [PunRPC]
    public void PlayerHasUpdatedData(string[] playerCommon, float[] playerCombat)
    {
        Debug.Log("Updated others data");

        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerCommon[0]))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerCommon[0]];

            UpdatePlayerInstancesStats(enemy, playerCommon);
            UpdatePlayerCombatStats(enemy.enemyStats, playerCombat);


            controller.combatController.InitializeEnemyPlayer();
        }
    }


    public void UpdatePlayerLifeAndTurn(PlayerInstance enemy, PlayerManager me)
    {
        float currentLife = me.currentHealth;
        float currentTurn = me.currentTurn;

        photonView.RPC("PlayerUpdatedLifeAndTurn", PhotonPlayer.Find(enemy.playerUserID), me.playerName,
            currentLife, currentTurn);
    }

    [PunRPC]
    public void PlayerUpdatedLifeAndTurn(string playerName, float life, float turn)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerName];

            enemy.currentHealth = life;
            enemy.enemyStats.currentTurn = turn;

            controller.combatController.UpdateEnemyLife();
            controller.combatController.UpdateEnemyTurn();
        }
    }

    public void OtherPlayerReceivedDamage(PlayerInstance player, float damage)
    {
        photonView.RPC("PlayerReceivedDamage", PhotonPlayer.Find(player.playerUserID), damage);
    }

    [PunRPC]
    public void PlayerReceivedDamage(float damageDone)
    {
        controller.playerManager.ReceiveDamage(damageDone);
    }


    public void UpdateOtherPlayersEnemyLog(PlayerInstance player, string message)
    {
        photonView.RPC("UpdateEnemyLog", PhotonPlayer.Find(player.playerUserID), message);
    }

    [PunRPC]
    public void UpdateEnemyLog(string newMessage)
    {
        controller.combatController.UpdateEnemyPlayerLog(newMessage);
    }


    public void PlayerEscapedBattle(PlayerInstance player)
    {
        photonView.RPC("OtherPlayerEscaped", PhotonPlayer.Find(player.playerUserID), controller.playerManager.playerName);
    }

    [PunRPC]
    public void OtherPlayerEscaped(string playerName)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerName];

            controller.combatController.StartCoroutine(controller.combatController.EndCombatByEscaping(enemy));
        }
    }

    public void PlayerDies(PlayerInstance player)
    {
        photonView.RPC("OtherPlayerDied", PhotonPlayer.Find(player.playerUserID), controller.playerManager.playerName);
    }

    [PunRPC]
    public void OtherPlayerDied(string playerName)
    {
        if (playerInstanceManager.playerInstancesOnScene.ContainsKey(playerName))
        {
            PlayerInstance enemy = playerInstanceManager.playerInstancesOnScene[playerName];

            controller.combatController.StartCoroutine(controller.combatController.EndCombat(enemy));
        }
    }

    #endregion
}

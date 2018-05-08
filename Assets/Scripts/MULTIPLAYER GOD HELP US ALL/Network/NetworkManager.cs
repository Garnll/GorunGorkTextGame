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
    public string gameVersion = "0.1";
    public byte maxPlayers = 10;

    [HideInInspector]public bool isConnecting;
    [HideInInspector]public bool connected;

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
        PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers =  this.maxPlayers}, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Conectado a habitación. ");
        isConnecting = false;
        connected = true;

        //Temp
        string[] newPlayer = new string[]
        {
            controller.playerManager.playerName,
            controller.playerManager.gender,
            controller.playerManager.playerLevel.ToString(),
            controller.playerManager.currentHealth.ToString(),
            controller.playerManager.currentVisibility.ToString(),
            controller.playerRoomNavigation.currentPosition.ToString()
        };

        photonView.RPC("NewPlayerJoined", PhotonTargets.Others, newPlayer);
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("Desconectado de Photon. ");
        connected = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
    }

    [PunRPC]
    public void NewPlayerJoined(string[] playerData)
    {
        PlayerInstance newPlayer = Instantiate(playerInstancePrefab).GetComponent<PlayerInstance>();

        newPlayer.playerName = playerData[0];
        newPlayer.playerGender = playerData[1];
        Int32.TryParse(playerData[2], out newPlayer.playerLevel);
        Int32.TryParse(playerData[3], out newPlayer.currentHealth);
        Int32.TryParse(playerData[4], out newPlayer.currentVisibility);
        newPlayer.currentRoom = RoomsChecker.RoomObjectFromVector(
            RoomsChecker.RoomPositionFromText(playerData[5])
            );

        if (newPlayer.currentRoom != null)
        {
            newPlayer.currentRoom.PlayerEnteredRoom(newPlayer, controller);
        }
    }
}

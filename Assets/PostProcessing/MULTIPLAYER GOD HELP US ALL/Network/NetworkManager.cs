using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

    private static NetworkManager instance = null;

    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkManager>();    
            }
            return instance;
        }
    }

    string gameVersion = "0.1";

    private void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void ConnectToServer()
    {
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
        
    }
}

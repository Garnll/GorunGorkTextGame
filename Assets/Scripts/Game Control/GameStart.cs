using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStart : MonoBehaviour {

    public GameController controller;
    public Room originRoom;

    void Awake () {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        controller.playerRoomNavigation.currentRoom = originRoom;
        controller.playerManager.Initialize();
	}


	
}

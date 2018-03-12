using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStart : MonoBehaviour {

    public TextUserInput input;
    public GameController controller;
    public Room originRoom;

    void Awake () {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        //input.EnableInput(false);
        controller.playerRoomNavigation.currentRoom = originRoom;
	}


	
}

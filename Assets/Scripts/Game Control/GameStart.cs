﻿using UnityEngine;

/// <summary>
/// Solo se usa al iniciar el juego. Actualmente, cambia la habitación en la que el jugador aparece
/// y el estado del juego lo inicia en "exploración".
/// </summary>
public class GameStart : MonoBehaviour {

    public GameController controller;
    public Room originRoom;

    void Awake () {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        controller.playerRoomNavigation.currentRoom = originRoom;
        controller.playerManager.Initialize();
	}


	
}

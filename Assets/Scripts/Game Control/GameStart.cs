using UnityEngine;

/// <summary>
/// Solo se usa al iniciar el juego. Actualmente, cambia la habitación en la que el jugador aparece
/// y el estado del juego lo inicia en "exploración".
/// </summary>
public class GameStart : MonoBehaviour {

    public GameController controller;
    public RoomObject originRoom;

    void Awake () {
        Application.runInBackground = true;

        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        controller.playerRoomNavigation.MovePlayerToRoom(originRoom);
        controller.playerManager.Initialize();
	}


	
}

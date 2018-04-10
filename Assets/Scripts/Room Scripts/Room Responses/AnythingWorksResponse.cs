using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Room Responses/Anything Works")]
public class AnythingWorksResponse : RoomResponse {

    public override void TriggerResponse(GameController controller)
    {
        base.TriggerResponse(controller);
    }

    public void AcceptInput(GameController gameController)
    {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        gameController.playerRoomNavigation.AttemptToChangeRooms(
            gameController.playerRoomNavigation.currentRoom.exits[0].myKeyword);
    }
}

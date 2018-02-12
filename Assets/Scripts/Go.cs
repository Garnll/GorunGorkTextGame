using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Go")]
public class Go : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.playerRoomNavigation.AttemptToChangeRooms(separatedInputWords[1]);
    }
}

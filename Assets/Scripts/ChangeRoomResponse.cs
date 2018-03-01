using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/ActionResponses/ChangeRoom")]
public class ChangeRoomResponse : ActionResponse {

    public Room roomToChangeTo;

    public override bool DoActionResponse(GameController controller)
    {
        if (controller.playerRoomNavigation.currentRoom.roomName == requiredString)
        {
            controller.playerRoomNavigation.currentRoom = roomToChangeTo;
            controller.DisplayRoomText();
            return true;
        }

        return false;
    }

}

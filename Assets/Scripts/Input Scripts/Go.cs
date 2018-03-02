using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Go")]
public class Go : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (separatedInputWords[0] == keyWord)
        {
            DirectionKeyword direction = converter.ConvertFromString(separatedInputWords[1]);

            if (direction != DirectionKeyword.unrecognized)
            {
                controller.playerRoomNavigation.AttemptToChangeRooms(direction);
            }
            else
            {
                controller.playerRoomNavigation.AttemptToChangeRooms(separatedInputWords[1]);
            }
        }
        else
        {
            controller.playerRoomNavigation.AttemptToChangeRooms(
                converter.ConvertFromString(separatedInputWords[0]));
        }
    }
}

using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para ir a otras habitaciones.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Go")]
public class GoInput : InputActions {

    /// <summary>
    /// Redirige el código a PlayerRoomNagation para que este se mueva, dependiendo de si reconoce o
    /// no la dirección dada.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (separatedInputWords[0] == keyWord)
        {
            if (separatedInputWords.Length > 1)
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
                controller.LogStringWithReturn("¿Ir donde?");
            }
        }
        else
        {
            controller.playerRoomNavigation.AttemptToChangeRooms(
                converter.ConvertFromString(separatedInputWords[0]));
        }
    }
}

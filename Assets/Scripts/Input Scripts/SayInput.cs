using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para decir cosas a todos en la habitación.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Say")]
public class SayInput : InputActions {

    /// <summary>
    /// Responde exactamente lo que dijo el jugador menos el input.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        NetworkManager.Instance.SayThingInRoom(SayJustTheString(separatedInputWords), controller.playerManager.playerName);
        controller.LogStringWithReturn(SayExactString(separatedInputWords));
    }

    /// <summary>
    /// Detecta qué dijo el jugador y elimina el input para devolver solo el resto de cosas.
    /// </summary>
    /// <param name="stringToSay"></param>
    /// <returns></returns>
    private string SayExactString(string[] stringToSay)
    {
        stringToSay[0] = "Dices a todos:";
        stringToSay[1] = "\"" + stringToSay[1];
        stringToSay[stringToSay.Length - 1] = stringToSay[stringToSay.Length - 1] + "\"";

        string combinedStrings = string.Join(" ", stringToSay);

        return combinedStrings;
    }

    /// <summary>
    /// Quita el primer input y deja el resto como una string
    /// </summary>
    /// <param name="stringToSay"></param>
    /// <returns></returns>
    private string SayJustTheString(string[] stringToSay)
    {
        string[] newString = new string[stringToSay.Length - 1];

        for (int i = 1; i < stringToSay.Length; i++)
        {
            newString[i - 1] = stringToSay[i];
        }

        string combinedStrings = string.Join(" ", newString);

        return combinedStrings;
    }

}

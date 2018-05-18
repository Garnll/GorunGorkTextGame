using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para decir cosas a todos en la habitación.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Say")]
public class SayInput : InputActions {

    PlayerInstance playerToSpeakTo;

    /// <summary>
    /// Responde exactamente lo que dijo el jugador menos el input.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        playerToSpeakTo = null;

        if (separatedInputWords.Length <= 1)
        {
            controller.LogStringWithReturn("Ibas a decir algo pero te quedaste callado.");
            return;
        }

        separatedInputWords =  CheckDifferentPossibilities(separatedInputWords);

        if (!playerToSpeakTo)
        {
            NetworkManager.Instance.SayThingInRoom(SayJustTheString(separatedInputWords), controller.playerManager.playerName);
            controller.LogStringWithReturn(SayExactString(separatedInputWords));
        }
        else
        {
            if (controller.playerRoomNavigation.currentRoom.playersInRoom.Contains(playerToSpeakTo))
            {
                NetworkManager.Instance.SayThingInRoomToPlayer(SayJustTheString(separatedInputWords),
                    controller.playerManager.playerName,
                    playerToSpeakTo.playerUserID);

                controller.LogStringWithReturn(SayExactString(separatedInputWords, playerToSpeakTo.playerName));
            }
            else
            {
                controller.LogStringWithReturn(playerToSpeakTo.playerName + " no está en este lugar...");
            }
        }
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

    private string SayExactString(string[] stringToSay, string otherPlayer)
    {
        stringToSay[0] = "Le dices a " +  otherPlayer + ":";
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


    private string[] CheckDifferentPossibilities(string[] lastString)
    {
        int playerPossiblePosition = 1;

        if (lastString.Length > 2)
        {
            if (lastString[1] == "a")
            {
                playerPossiblePosition = 2;
            }
        }
        if (lastString.Length == playerPossiblePosition + 1)
        {
            return lastString;
        }

        if (NetworkManager.Instance.playerInstanceManager.playerInstancesOnScene.ContainsKey(lastString[playerPossiblePosition]))
        {
            playerToSpeakTo = NetworkManager.Instance.playerInstanceManager.playerInstancesOnScene[lastString[playerPossiblePosition]];

            string[] newString = new string[lastString.Length - playerPossiblePosition];

            newString[0] = "";

            for (int i = 0; i <= playerPossiblePosition; i++)
            {
                newString[0] += lastString[i];
            }

            for (int i = 1; i < newString.Length; i++)
            {
                newString[i] = lastString[i + playerPossiblePosition];
            }

            return newString;
        }
        else
        {
            return lastString;
        }
    }
}

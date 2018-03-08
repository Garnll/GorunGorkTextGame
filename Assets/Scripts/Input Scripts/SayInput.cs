using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Say")]
public class SayInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.LogStringWithReturn(SayExactString(separatedInputWords));
    }

    public string SayExactString(string[] stringToSay)
    {
        stringToSay[0] = "Dices a todos:";
        stringToSay[1] = "\"" + stringToSay[1];
        stringToSay[stringToSay.Length - 1] = stringToSay[stringToSay.Length - 1] + "\"";

        string combinedStrings = string.Join(" ", stringToSay);

        return combinedStrings;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Take")]
public class Take : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        Dictionary<string, string> takeDictionary = controller.interactableItems.Take(separatedInputWords);

        if (takeDictionary != null)
        {
            controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(takeDictionary,
                separatedInputWords[0],
                separatedInputWords[1]));


        }
    }
}

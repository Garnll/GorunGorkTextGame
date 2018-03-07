using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Throw")]
public class Throw : InputActions
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //bool threwObject = controller.interactableItems.Throw(separatedInputWords);

        //if (threwObject)
        //{
        //    controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(
        //        controller.interactableItems.throwDictionary,
        //        separatedInputWords[0],
        //        separatedInputWords[1]));
        //}
    }
}

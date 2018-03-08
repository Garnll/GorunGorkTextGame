using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Take")]
public class TakeInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //Dictionary<string, string> takeDictionary = controller.interactableItems.Take(separatedInputWords);

        //if (takeDictionary != null)
        //{
        //    controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(takeDictionary,
        //        separatedInputWords[0],
        //        separatedInputWords[1]));
        //}

        InteractableObject objectToTake = controller.itemHandler.SearchObjectInRoom(separatedInputWords);
        if (objectToTake != null)
        {
            controller.itemHandler.TakeObject(objectToTake);
        }
    }
}

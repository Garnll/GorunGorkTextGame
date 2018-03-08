using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Examine")]
public class ExamineInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //controller.LogStringWithReturn(controller.TestVerbDictionaryWithNoun(
        //    controller.interactableItems.examineDictionary, 
        //    separatedInputWords[0], 
        //    separatedInputWords[1]));
        if (separatedInputWords.Length > 1)
        {
            string noun = separatedInputWords[1];

            if (noun == "habitacion" || noun == "" || noun == "lugar")
            {
                controller.LogStringWithReturn(controller.RefreshedCurrentRoomDescription());
                return;
            }
        }
        else
        {
            controller.LogStringWithReturn(controller.RefreshedCurrentRoomDescription());
            return;
        }

        InteractableObject objectToExamine = controller.itemHandler.SearchObjectInRoomAndInventory(separatedInputWords);

        if (objectToExamine != null)
        {
            controller.itemHandler.ExamineObject(objectToExamine);
        }
    }
}

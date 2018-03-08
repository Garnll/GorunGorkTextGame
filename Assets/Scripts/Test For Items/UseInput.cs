using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Use")]
public class UseInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //controller.interactableItems.UseItem(separatedInputWords);

        InteractableObject objectToExamine = controller.itemHandler.SearchObjectInInventory(separatedInputWords);

        if (objectToExamine != null)
        {
            controller.itemHandler.UseObject(objectToExamine);
        }
    }
}

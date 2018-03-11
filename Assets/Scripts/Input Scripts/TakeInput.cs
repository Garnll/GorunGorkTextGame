using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Take")]
public class TakeInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {

        InteractableObject objectToTake = controller.itemHandler.SearchObjectInRoom(separatedInputWords);

        if (objectToTake != null)
        {
            controller.itemHandler.TakeObject(objectToTake);
        }
    }
}

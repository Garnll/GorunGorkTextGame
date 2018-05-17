using UnityEngine;

/// <summary>
/// Input que el usuario para coger cosas de la habitación.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Take")]
public class TakeInput : InputActions {

    /// <summary>
    /// Redirige el código para revisar que el objeto si exista y cogerlo.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
	/// 

    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        InteractableObject objectToTake = 
            controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, true, false);

        if (objectToTake != null)
        {


            controller.itemHandler.TakeObject(objectToTake);
        }
    }
}

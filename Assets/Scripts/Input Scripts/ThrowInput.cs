using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para soltar/tirar objetos en la habitación.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Throw")]
public class ThrowInput : InputActions
{
    /// <summary>
    /// Redirige el código para ver si el objeto si está en el inventario, y lo tira.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        InteractableObject objectToThrow = 
            controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, false, true);
        if (objectToThrow != null)
        {
            controller.itemHandler.ThrowObject(objectToThrow);
        }
    }
}

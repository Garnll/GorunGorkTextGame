using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para usar objetos.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Use")]
public class UseInput : InputActions {

    /// <summary>
    /// Redirige el código para ver si el objeto existe en el inventario, y de ser asi, si se puede usar.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        InteractableObject objectToExamine = 
            controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, false, true);

        if (objectToExamine != null)
        {
            controller.itemHandler.UseObject(objectToExamine);
        }
    }
}

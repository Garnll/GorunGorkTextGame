using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para ver el inventario dentro de la pantalla principal.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Inventory")]
public class InventoryInput : InputActions {

    /// <summary>
    /// Redirige el código para mostrar el inventario.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.itemHandler.DisplayInventoryByCommand();
    }

}

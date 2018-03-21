using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para examinar objetos y/o personajes.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Examine")]
public class ExamineInput : InputActions {

    /// <summary>
    /// Revisa el input del usuario, y determina si se puede examinar o no lo que se está diciendo.
    /// En ambos casos envía una respuesta.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            string noun = separatedInputWords[1];

            if (noun == "habitacion" || noun == "" || noun == "lugar")
            {
                controller.LogStringWithReturn(controller.RefreshCurrentRoomDescription());
                return;
            }
        }
        else
        {
            controller.LogStringWithReturn(controller.RefreshCurrentRoomDescription());
            return;
        }

        InteractableObject objectToExamine = 
            controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, true, true);

        if (objectToExamine != null)
        {
            controller.itemHandler.ExamineObject(objectToExamine);
        }
    }
}

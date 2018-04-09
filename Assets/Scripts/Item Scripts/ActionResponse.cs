using UnityEngine;

/// <summary>
/// Clase base para todas las posibles respuestas a las acciones del jugador (como tirar, coger, usar).
/// </summary>
public abstract class ActionResponse : ScriptableObject {

    [TextArea] public string responseDescription;
    [TextArea] public string negationDescription;

    public abstract bool DoActionResponse(GameController controller);

}

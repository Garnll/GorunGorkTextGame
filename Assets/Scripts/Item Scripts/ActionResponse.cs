using UnityEngine;

/// <summary>
/// Clase base para todas las posibles respuestas a las acciones del jugador (como tirar, coger, usar).
/// </summary>
public abstract class ActionResponse : ScriptableObject {

    public string requiredString;

    public abstract bool DoActionResponse(GameController controller);

}

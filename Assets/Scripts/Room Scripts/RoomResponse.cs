using UnityEngine;

/// <summary>
/// Clase base de las posibles respuestas que pueden dar las habitaciones al llegar a ellas.
/// </summary>
[System.Serializable]
public abstract class RoomResponse : ScriptableObject {

    public GameState.GameStates stateToChangeTo = GameState.GameStates.exploration;
    public string[] responses;
    public bool willRespond = true;

    /// <summary>
    /// En principio, todas las respuestas cambiarán el estado del juego, y enviará textos extra a la
    /// habitación.
    /// </summary>
    /// <param name="controller"></param>
    public virtual void TriggerResponse(GameController controller)
    {
        if (!willRespond)
            return;

        GameState.Instance.ChangeCurrentState(stateToChangeTo);
        for (int i = 0; i < responses.Length; i++)
        {
            controller.LogStringWithReturn(responses[i]);
        }
    }
	
}

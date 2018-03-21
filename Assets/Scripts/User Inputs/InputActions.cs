using UnityEngine;

/// <summary>
/// Clase base para todas los inputs del jugador que puedan generar acciones dentro del juego.
/// </summary>
public abstract class InputActions : ScriptableObject {

    public string keyWord;
    protected KeywordToStringConverter converter;

    public abstract void RespondToInput(GameController controller, string[] separatedInputWords);
}

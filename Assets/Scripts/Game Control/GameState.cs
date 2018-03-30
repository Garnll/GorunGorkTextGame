/// <summary>
/// Singleton. Contiene el estado actual del juego, y puede cambiarse desde acá.
/// </summary>
public class GameState {

    private static GameState instance = null;

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameState();
            }
            return instance;
        }
    }

    public enum GameStates
    {
        creation,
        exploration,
        combat,
        dead,
        none
    }

    private GameStates currentState = GameStates.creation;

    /// <summary>
    /// Devuelve el estado actual.
    /// </summary>
    public GameStates CurrentState
    {
        get
        {
            return currentState;
        }
    }

    /// <summary>
    /// Permite cambiar el estado actua al estado que se le envíe..
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeCurrentState(GameStates newState)
    {
        currentState = newState;
    }


}

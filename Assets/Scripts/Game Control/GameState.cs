using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        dead
    }

    private GameStates currentState = GameStates.creation;

    public GameStates CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public void ChangeCurrentState(GameStates newState)
    {
        currentState = newState;
    }


}

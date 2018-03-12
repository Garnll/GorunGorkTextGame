using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomResponse : ScriptableObject {

    public GameState.GameStates stateToChangeTo = GameState.GameStates.exploration;
    public string[] responses;
    public bool willRespond = true;

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

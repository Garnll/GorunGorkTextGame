    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : ScriptableObject {

    public string stateName = "estado";
    public int magnitude = 0;
    [TextArea] public string stateDescription;
    public int durationTime = 0;

    public abstract void ApplyStateEffect<T>(T character);

    public abstract void DissableStateEffect<T>(T character);


}

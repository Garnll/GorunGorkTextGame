    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : ScriptableObject {

    public string stateName;
    [TextArea] public string stateDescription;

    public abstract void ApplyStateEffect(PlayerManager player);


}

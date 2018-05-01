using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Normal")]
public class NormalState : CharacterEffectiveState {

    public override void ApplyStateEffect<T>(T character)
    {
        //Nothing goes here
    }

    public override void DissableStateEffect<T>(T character)
    {
        //Nothing goes here
    }
}

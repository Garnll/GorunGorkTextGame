using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour {

    public float criticalHitProbability;
    public float cooldownReduction;
    public float healthRegenPerSecond;
    public float turnRegenPerSecond;
    public float evasion;

    private float escapeProbability;

    public float EscapeProbability()
    {


        return escapeProbability;
    }

}

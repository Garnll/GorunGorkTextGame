using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour {

    [SerializeField] private float defaultCriticalHitProbability = 0;
    [HideInInspector] public float currentCriticalHitProbability;

    [SerializeField] private float defaultCooldownReduction = 0;
    [HideInInspector] public float currentCooldownReduction;

    [SerializeField] private float defaultHealthRegenPerSecond = 2;
    [HideInInspector] public float currentHealthRegenPerSecond;

    [SerializeField] private float defaultTurnRegenPerSecond = 5;
    [HideInInspector] public float currentTurnRegenPerSecond;

    [SerializeField] private float defaultEvasion = 0;
    [HideInInspector] public float currentEvasion;

    private float escapeProbability = 0;


    public float DefaultCriticalHitProbability
    {
        get
        {
            return defaultCriticalHitProbability;
        }
    }

    public float DefaultCooldownReduction
    {
        get
        {
            return defaultCooldownReduction;
        }
    }

    public float DefaultHealthRegenPerSecond
    {
        get
        {
            return defaultHealthRegenPerSecond;
        }
    }

    public float DefaultEvasion
    {
        get
        {
            return defaultEvasion;
        }
    }



    public void InitializeOthers()
    {
        currentCooldownReduction = defaultCooldownReduction;
        currentCriticalHitProbability = defaultCriticalHitProbability;
        currentEvasion = defaultEvasion;
        currentHealthRegenPerSecond = defaultHealthRegenPerSecond;
        currentTurnRegenPerSecond = defaultTurnRegenPerSecond;
    }

    public float EscapeProbability()
    {


        return escapeProbability;
    }

}

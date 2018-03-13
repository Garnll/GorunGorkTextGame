using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour {

    [SerializeField] private float defaultCriticalHitProbability = 0;
    public float currentCriticalHitProbability;

    [SerializeField] private float defaultCooldownReduction = 0;
    public float currentCooldownReduction;

    [SerializeField] private float defaultHealthRegenPerSecond = 2;
    public float currentHealthRegenPerSecond;

    [SerializeField] private float defaultTurnRegenPerSecond = 5;
    public float currentTurnRegenPerSecond;

    [SerializeField] private float defaultEvasion = 0;
    public float currentEvasion;

    private float escapeProbability = 0;

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

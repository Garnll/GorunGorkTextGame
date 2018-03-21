using UnityEngine;

/// <summary>
/// "Otros" parametros del jugador. Cosas que se usan usualmente en combate.
/// </summary>
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


    /// <summary>
    /// Inicia las variables extras del jugador según sus defaults.
    /// </summary>
    public void InitializeOthers()
    {
        currentCooldownReduction = defaultCooldownReduction;
        currentCriticalHitProbability = defaultCriticalHitProbability;
        currentEvasion = defaultEvasion;
        currentHealthRegenPerSecond = defaultHealthRegenPerSecond;
        currentTurnRegenPerSecond = defaultTurnRegenPerSecond;
    }

    /// <summary>
    /// Aun no implementada.
    /// </summary>
    /// <returns></returns>
    public float EscapeProbability()
    {


        return escapeProbability;
    }

}

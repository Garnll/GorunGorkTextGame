using UnityEngine;

/// <summary>
/// Base para crear los NPCs puramente enemigos. No tienen texto para hablarles, y funcionan
/// solo dentro del combate.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/NPC/Enemy")]
public class EnemyNPC : NPCTemplate {

    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float maxTurn = 100;
    [HideInInspector] public float currentTurn;
    [SerializeField] private float maxWill = 10;
    [HideInInspector] public float currentWill;

    public Hability[] habilities;

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


    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float MaxTurn
    {
        get
        {
            return maxTurn;
        }
    }

    public float MaxWill
    {
        get
        {
            return maxWill;
        }
    }


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
    /// Aun no implementada.
    /// </summary>
    /// <returns></returns>
    public float EscapeProbability()
    {


        return escapeProbability;
    }



}

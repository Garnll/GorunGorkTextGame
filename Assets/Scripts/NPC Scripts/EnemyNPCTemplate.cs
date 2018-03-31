using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base para crear los NPCs puramente enemigos. No tienen texto para hablarles, y funcionan
/// solo dentro del combate.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/NPC/Enemy")]
public class EnemyNPCTemplate : NPCTemplate {

    public GameObject enemyGameObject;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxTurn = 100;
    [SerializeField] private float maxWill = 10;


    public Hability[] habilities;
    public CharacterState defaultState;

    public int defaultStrength = 1;
    public int defaultIntelligence = 1;
    public int defaultResistance = 1;
    public int defaultDexterity = 1;

    [Space(5)]
    [SerializeField] private float defaultCriticalHitProbability = 0;

    [SerializeField] private float defaultCooldownReduction = 0;

    [SerializeField] private float defaultHealthRegenPerSecond = 2;

    [SerializeField] private float defaultTurnRegenPerSecond = 5;

    [SerializeField] private float defaultEvasion = 0;

    private float escapeProbability = 0;

    CombatController combatController;

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

    public float DefaultTurnRegenPerSecond
    {
        get
        {
            return defaultTurnRegenPerSecond;
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

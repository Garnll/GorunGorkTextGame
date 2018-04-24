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

    public float defaultStrength = 1;
    public float defaultIntelligence = 1;
    public float defaultResistance = 1;
    public float defaultDexterity = 1;

    [Space(5)]
    [SerializeField] private float defaultCriticalHitProbability = 0;

    [SerializeField] private float defaultCooldownReduction = 0;

    [SerializeField] private float defaultHealthRegenPerSecond = 0.2f;

    [SerializeField] private float defaultTurnRegenPerSecond = 5;

    [SerializeField] private float defaultEvasion = 0;

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
    /// Calcula la probabilidad de escape del enemigo según los parametros dados.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public float EscapeProbability(EnemyNPC enemy, PlayerManager player)
    {
        escapeProbability =
            (((player.MaxHealth - player.currentHealth) / player.MaxHealth) * 100) +
            (enemy.currentDexterity / 10) +
            (enemy.currentState.magnitude) -
            (player.currentState.magnitude);

        return escapeProbability;
    }
}

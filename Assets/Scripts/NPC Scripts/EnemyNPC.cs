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
    public CharacterState defaultState;
    [HideInInspector] public CharacterState currentState;

    public int defaultStrength = 1;
    [HideInInspector] public int currentStrength;
    public int defaultIntelligence = 1;
    [HideInInspector] public int currentIntelligence;
    public int defaultResistance = 1;
    [HideInInspector] public int currentResistance;
    public int defaultDexterity = 1;
    [HideInInspector] public int currentDexterity;

    private int timePassed = 0;
    public int pacifier = 1;

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

    public void ReturnToNormalState()
    {
        currentState = defaultState;
        timePassed = 0;
    }

    public void StartCombat(CombatController controller)
    {
        combatController = controller;

        currentDexterity = defaultDexterity;
        currentIntelligence = defaultIntelligence;
        currentResistance = defaultResistance;
        currentStrength = defaultStrength;

        currentTurnRegenPerSecond = defaultTurnRegenPerSecond;
        currentHealthRegenPerSecond = defaultHealthRegenPerSecond;
        currentCriticalHitProbability = defaultCriticalHitProbability;
        currentEvasion = defaultEvasion;
        currentCooldownReduction = defaultCooldownReduction;
        currentHealth = maxHealth;

        currentTurn = 0;
        currentState = defaultState;
    }

    public void AttackInCombat(PlayerManager player)
    {
        if (currentTurn < maxTurn)
        {
            combatController.UpdateEnemyLog("Cómo es que está atacando?");
            return;
        }
        currentTurn -= maxTurn;
        combatController.UpdateEnemyTurn();

        int damage = currentStrength + Random.Range(1, 5) + Random.Range(0, 3);
        damage *= pacifier;

        combatController.UpdateEnemyLog("El " + npcName + " ha atacado.");
        player.ReceiveDamage(damage);
    }

    public void ChargeBySecond()
    {
        currentTurn += currentTurnRegenPerSecond;
        currentHealth += currentHealthRegenPerSecond;

        if (currentTurn >= maxTurn)
        {
            currentTurn = maxTurn;

            if (currentState.GetType() == typeof(InertiaState))
            {
                currentState.DissableStateEffect(this);
            }
        }

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        combatController.UpdateEnemyTurn();
        combatController.UpdateEnemyLife();

        CheckForStateDuration();
    }

    private void CheckForStateDuration()
    {
        if (currentState.durationTime > 0)
        {
            timePassed++;
            if (timePassed > currentState.durationTime)
            {
                currentState.DissableStateEffect(this);
            }
        }
    }

    public void ChangeState(CharacterState newState)
    {
        currentState = newState;
        currentState.ApplyStateEffect(this);
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        combatController.UpdateEnemyLife();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }
        combatController.UpdateEnemyLog("El " + npcName + " ha recibido " + damage + " puntos de daño.");
    }

    public void Die()
    {
        isAlive = false;
        combatController.UpdateEnemyLog("El " + npcName + " ha muerto.");

        combatController.StartCoroutine(combatController.EndCombat(this));
    }
}

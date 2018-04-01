using UnityEngine;

public class EnemyNPC : MonoBehaviour {

    public EnemyNPCTemplate myTemplate;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentTurn;
    [HideInInspector] public float currentWill;

    public Hability[] habilities;
    [HideInInspector] public CharacterState currentState;

    [HideInInspector] public int currentStrength;
    [HideInInspector] public int currentIntelligence;
    [HideInInspector] public int currentResistance;
    [HideInInspector] public int currentDexterity;

    private int timePassed = 0;
    public int pacifier = 1;

    [HideInInspector] public float currentCriticalHitProbability;

    [HideInInspector] public float currentCooldownReduction;

    [HideInInspector] public float currentHealthRegenPerSecond;

    [HideInInspector] public float currentTurnRegenPerSecond;

    [HideInInspector] public float currentEvasion;

    private float escapeProbability = 0;

    public bool isAlive = true;

    CombatController combatController;


    public void ReturnToNormalState()
    {
        currentState = myTemplate.defaultState;
        timePassed = 0;
    }

    public void StartCombat(CombatController controller)
    {
        combatController = controller;

        currentDexterity = myTemplate.defaultDexterity;
        currentIntelligence = myTemplate.defaultIntelligence;
        currentResistance = myTemplate.defaultResistance;
        currentStrength = myTemplate.defaultStrength;

        currentTurnRegenPerSecond = myTemplate.DefaultTurnRegenPerSecond;
        currentHealthRegenPerSecond = myTemplate.DefaultHealthRegenPerSecond;
        currentCriticalHitProbability = myTemplate.DefaultCriticalHitProbability;
        currentEvasion = myTemplate.DefaultEvasion;
        currentCooldownReduction = myTemplate.DefaultCooldownReduction;
        currentHealth = myTemplate.MaxHealth;

        currentTurn = 0;
        currentState = myTemplate.defaultState;
    }

    public void AttackInCombat(PlayerManager player)
    {
        if (currentTurn < myTemplate.MaxTurn)
        {
            combatController.UpdateEnemyLog("Cómo es que está atacando?");
            return;
        }
        currentTurn -= myTemplate.MaxTurn * 0.8f;
        combatController.UpdateEnemyTurn();

        int damage = currentStrength + Random.Range(1, 5) + Random.Range(0, 3);
        damage *= pacifier;

        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha atacado.");
        player.ReceiveDamage(damage);
    }

    public void ChargeBySecond()
    {
        currentTurn += currentTurnRegenPerSecond;
        currentHealth += currentHealthRegenPerSecond;

        if (currentTurn >= myTemplate.MaxTurn)
        {
            currentTurn = myTemplate.MaxTurn;

            if (currentState.GetType() == typeof(InertiaState))
            {
                currentState.DissableStateEffect(this);
            }
        }

        if (currentHealth >= myTemplate.MaxHealth)
        {
            currentHealth = myTemplate.MaxHealth;
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

    public void ReduceEvasionBySecond()
    {
        if (currentEvasion > myTemplate.DefaultEvasion)
        {
            currentEvasion--;
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
        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha recibido " + damage + " puntos de daño.");
    }

    public void TryToEscape(PlayerManager player)
    {
        currentTurn -= myTemplate.MaxTurn;
        combatController.UpdateEnemyTurn();

        float r = Random.Range(0f, 1f);

        r *= 100;

        if (r <= myTemplate.EscapeProbability(this, player))
        {
            combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha huido.");

            combatController.StopCoroutine(combatController.EndCombatByEscaping(this));
            combatController.StartCoroutine(combatController.EndCombatByEscaping(this));
        }
        else
        {
            combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha intentado huir.");
        }
    }

    public void Die()
    {
        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha muerto.");

        combatController.StopCoroutine(combatController.EndCombat(this));
        combatController.StartCoroutine(combatController.EndCombat(this));
        isAlive = false;
    }
}

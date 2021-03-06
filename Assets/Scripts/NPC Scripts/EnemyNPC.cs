﻿using UnityEngine;

/// <summary>
/// Instancia de una NPC Enemigo, creada y manejada a partir de un EnemyNPCTemplate.
/// </summary>
public class EnemyNPC : MonoBehaviour {

    [HideInInspector] public EnemyNPCTemplate myTemplate;
	[HideInInspector] public float maxHealth;
	[HideInInspector] public float maxTurn;

    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentTurn;
    [HideInInspector] public float currentWill;

    [HideInInspector] public CharacterEffectiveState currentState;

    [HideInInspector] public float currentStrength;
    [HideInInspector] public float currentIntelligence;
    [HideInInspector] public float currentResistance;
    [HideInInspector] public float currentDexterity;

    private int timePassed = 0;
    [HideInInspector] public int pacifier = 1;

    [HideInInspector] private float currentCriticalHitProbability;

    [HideInInspector] public float currentCooldownReduction;

    [HideInInspector] public float currentHealthRegenPerSecond;

    [HideInInspector] public float currentTurnRegenPerSecond;

    [HideInInspector] public float currentEvasion;

    private float escapeProbability = 0;

    [HideInInspector] public bool isAlive = true;
    private bool isTurn = false;

    [HideInInspector] public CombatController combatController;

    [HideInInspector] public EnemyNPCAI myAI;

    /// <summary>
    /// Devuelve el CharacterEffectiveState del enemigo a su default.
    /// </summary>
    public void ReturnToNormalState()
    {
        currentState = myTemplate.defaultState;
        timePassed = 0;
        combatController.ChangeEnemyState();
    }

    /// <summary>
    /// Inicializa al enemigo para el combate.
    /// </summary>
    /// <param name="controller"></param>
    public void StartCombat(CombatController controller)
    {
        combatController = controller;
        isTurn = false;

		maxHealth = myTemplate.MaxHealth;
		maxTurn = myTemplate.MaxTurn;
        currentDexterity = myTemplate.defaultDexterity;
        currentIntelligence = myTemplate.defaultIntelligence;
        currentResistance = myTemplate.defaultResistance;
        currentStrength = myTemplate.defaultStrength;

        myTemplate.currentVisibility = myTemplate.defaultVisibility;

        currentTurnRegenPerSecond = myTemplate.DefaultTurnRegenPerSecond;
        currentHealthRegenPerSecond = myTemplate.DefaultHealthRegenPerSecond;
        currentCriticalHitProbability = myTemplate.DefaultCriticalHitProbability;
        currentEvasion = myTemplate.DefaultEvasion;
        currentCooldownReduction = myTemplate.DefaultCooldownReduction;
        currentHealth = myTemplate.MaxHealth;

        currentTurn = 0;
        currentState = myTemplate.defaultState;
    }

    public float CriticalHitProbability()
    {
        currentCriticalHitProbability = (myTemplate.DefaultCriticalHitProbability +
            (currentDexterity * 0.01f));

        return currentCriticalHitProbability;
    }

    /// <summary>
    /// Gasta una cantidad de turno dada;
    /// </summary>
    /// <param name="toWaste"></param>
    public void WasteTurn (float toWaste)
    {
        isTurn = false;
        currentTurn -= (myTemplate.MaxTurn * toWaste);
        combatController.UpdateEnemyTurn();
    }

    public void AttackInCombat(PlayerManager player)
    {
        if (currentTurn < myTemplate.MaxTurn)
        {
            combatController.UpdateEnemyLog("Cómo es que está atacando?");
            return;
        }

        WasteTurn(0.8f);

        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha atacado.");

        float damage = currentStrength + Random.Range(1, 5) + Random.Range(0, 3);

        float r = Random.Range(0f, 1f);

        if (r <= CriticalHitProbability())
        {
            combatController.UpdateEnemyLog("¡CRITICO!");
            damage *= 2;
        }

        damage *= pacifier;


        player.ReceiveDamage(damage);
    }

    public void RepositionInCombat()
    {
        WasteTurn(0.5f);

        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " se ha movido a un lado.");

        if (currentState.GetType() == typeof(TrailState) || currentState.GetType() == typeof(SupersonicState))
        {
            return;
        }

        if (currentEvasion < 50)
        {
            currentEvasion = 50;
        }
        else
        {
            currentEvasion = 100;
        }

        Timer timer = Timer.Instance;

        timer.StopCoroutine(timer.RepositionTime((1 + (currentIntelligence / 5)), this));
        timer.StartCoroutine(timer.RepositionTime((1 + (currentIntelligence / 5)), this));
    }

    /// <summary>
    /// Recarga el turno y la vida cada segundo según los parámetros de recarga de ambos.
    /// </summary>
    public void ChargeBySecond()
    {
        currentTurn += (currentTurnRegenPerSecond + (0.03f * currentDexterity));
        currentHealth += currentHealthRegenPerSecond;

        if (currentTurn >= myTemplate.MaxTurn && !isTurn)
        {
            currentTurn = myTemplate.MaxTurn;
            combatController.enemyUI.turnParticles.Play();
            isTurn = true;

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

    /// <summary>
    /// Revisa si ya pasó el tiempo de duración del estado actual.
    /// </summary>
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

    /// <summary>
    /// Reduce la evasión del enemigo cada segundo si esta supera su default.
    /// </summary>
    public void ReduceEvasionBySecond()
    {
        if (currentEvasion > myTemplate.DefaultEvasion)
        {
            currentEvasion--;
        }
    }

    /// <summary>
    /// Cambia el CharacterEffectiveState actual a uno dado.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(CharacterEffectiveState newState)
    {
        currentState = newState;
        currentState.ApplyStateEffect(this);
        combatController.ChangeEnemyState();
    }


    public void ReceiveDamage(float damage)
    {
        float e = Random.Range(0, 100);

        if (e <= currentEvasion)
        {
            combatController.UpdateEnemyLog("¡El " + myTemplate.npcName + " ha evadido el golpe!");
            return;
        }

        float r = Random.Range(0f, 1f);

        if (r < 0.5f)
        {
            damage = damage - ((0.05f * currentResistance) * damage);
        }
        else if (r < 0.75f)
        {
            damage = damage - ((0.05f * (currentResistance - 1)) * damage);
        }

        currentHealth -= damage;

        if (currentEvasion < myTemplate.DefaultEvasion)
        {
            currentEvasion = myTemplate.DefaultEvasion;
        }

        combatController.UpdateEnemyLife();

        if (currentState.GetType() == typeof(SupersonicState))
        {
            currentState.DissableStateEffect(this);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }
        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha recibido " + damage + " puntos de daño.");
        combatController.StopCoroutine(combatController.AnimateHitEnemy());
        combatController.StartCoroutine(combatController.AnimateHitEnemy());
    }

    public void TryToEscape(PlayerManager player)
    {
        WasteTurn(1);

        float r = Random.Range(0f, 1f);

        r *= 100;

        if (r <= myTemplate.EscapeProbability(this, player))
        {
            combatController.UpdateEnemyLog("El " + myTemplate.npcName + " se ha rendido.");

            combatController.StopCoroutine(combatController.EndCombatByGivingUp(this));
            combatController.StartCoroutine(combatController.EndCombatByGivingUp(this));
        }
        else
        {
            combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha intentado rendirse.");
        }
    }

    /// <summary>
    /// "Mata" al enemigo, terminando el combate.
    /// </summary>
    public void Die()
    {
        combatController.UpdateEnemyLog("El " + myTemplate.npcName + " ha muerto.");

        combatController.StopCoroutine(combatController.EndCombat(this));
        combatController.StartCoroutine(combatController.EndCombat(this));
        isAlive = false;
    }
}

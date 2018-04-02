using System.Collections;
using UnityEngine;

/// <summary>
/// El controlador del jugador. El principal componente de todo jugador, controla vida, nivel, nombre, y demás
/// parametros relevantes. 
/// </summary>
public class PlayerManager : MonoBehaviour {

    public string playerName = "jugador";
    public string gender = "macho";
    public int playerLevel = 0;

    private bool isAlive = true;
    public CharacterState defaultState;
    [HideInInspector] public CharacterState currentState;

    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float maxTurn = 100;
    [HideInInspector] public float currentTurn;
    [SerializeField] private float maxWill = 10;
    [HideInInspector] public float currentWill;

    private int timePassed = 0;
    public int pacifier = 1;

    public PlayerCharacteristics characteristics;

    public GameController controller;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

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

    /// <summary>
    /// Inicializa las variables del jugador, entre otras características.
    /// </summary>
    public void Initialize()
    {
        currentHealth = maxHealth;
        currentTurn = maxTurn;
        currentWill = maxWill;
        defaultState.ApplyStateEffect(this);

        characteristics.InitializeCharacteristics();
        characteristics.other.InitializeOthers();

        if (characteristics.playerRace != null)
        {
            characteristics.playerRace.ActivatePassiveHability(this);
        }

        StartCoroutine(ChargeLife());
    }

    public void ReturnToNormalState()
    {
        currentState = defaultState;
        timePassed = 0;
        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            controller.combatController.ChangePlayerState();
        }
    }

    public void SelectRace(Race raceToBe)
    {
        characteristics.playerRace = raceToBe;
        characteristics.playerRace.ChangePlayerStats(characteristics);
        characteristics.playerRace.ActivatePassiveHability(this);
    }

    public void SelectJob(Job jobToBe)
    {
        characteristics.playerJob = jobToBe;
    }


    public void StartCombat()
    {
        currentTurn = 0;
        currentState = defaultState;
    }

    public void WasteTurn(float percentage)
    {
        currentTurn -= (maxTurn * percentage);
        controller.combatController.UpdatePlayerTurn();
    }


    public void AttackInCombat(EnemyNPC enemy)
    {
        WasteTurn(0.8f);

        controller.combatController.UpdatePlayerLog( "Has atacado.");

        int damage = characteristics.currentStrength + Random.Range(1, 5) + Random.Range(0, 3);

        float r = Random.Range(0f, 1f);

        if (r <= characteristics.other.CriticalHitProbability(this))
        {
            controller.combatController.UpdatePlayerLog("¡CRITICO!");
            damage *= 2;
        }

        damage *= pacifier;
    

        enemy.ReceiveDamage(damage);
    }

    public void RepositionInCombat()
    {
        WasteTurn(0.5f);

        controller.combatController.UpdatePlayerLog("Te mueves hacia un lado");

        if (currentState.GetType() == typeof(TrailState) || currentState.GetType() == typeof(SupersonicState))
        {
            return;
        }

        if (characteristics.other.currentEvasion < 50)
        {
            characteristics.other.currentEvasion = 50;
        }
        else
        {
            characteristics.other.currentEvasion = 100;
        }

        Timer timer = Timer.Instance;

        timer.StopCoroutine(timer.RepositionTime((1 * (characteristics.currentIntelligence / 5)), this));
        timer.StartCoroutine(timer.RepositionTime((1 * (characteristics.currentIntelligence / 5)), this));
    }

    public void ChargeTurn()
    {
        currentTurn += (characteristics.other.currentTurnRegenPerSecond + (0.03f * characteristics.currentDexterity));

        if (currentTurn >= maxTurn)
        {
            currentTurn = maxTurn;

            if (currentState.GetType() == typeof(InertiaState))
            {
                currentState.DissableStateEffect(this);
            }
        }
        controller.combatController.UpdatePlayerTurn();

        CheckForStateDuration();
        controller.combatController.SetPlayerHabilities();
        controller.combatController.SetPlayerOptions();
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

    public IEnumerator ChargeLife()
    {
        while (isAlive)
        {
            yield return new WaitForSecondsRealtime(1);
            currentHealth += characteristics.other.currentHealthRegenPerSecond;

            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                controller.combatController.UpdatePlayerLife();
            }
        }
    }

    public void ChangeState(CharacterState newState)
    {
        currentState = newState;
        currentState.ApplyStateEffect(this);
        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            controller.combatController.ChangePlayerState();
        }
    }

    public void ReceiveDamage(float damage)
    {
        float e = Random.Range(0, 100);

        if (e <= characteristics.other.currentEvasion)
        {
            controller.combatController.UpdatePlayerLog("¡Has evadido el golpe!");
            return;
        }

        float r = Random.Range(0f, 1f);

        if (r < 0.5f)
        {
            damage = damage - ((0.05f * characteristics.currentResistance) * damage); 
        }
        else if (r < 0.75f)
        {
            damage = damage - ((0.05f * (characteristics.currentResistance-1)) * damage);
        }

        currentHealth -= damage;
        controller.combatController.UpdatePlayerLife();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        if (currentState.GetType() == typeof(SupersonicState))
        {
            currentState.DissableStateEffect(this);
        }

        controller.combatController.UpdatePlayerLog("Has recibido " + damage.ToString("0.#") + " puntos de daño.");
    }

    public void TryToEscape(EnemyNPC enemy)
    {
        WasteTurn(1);

        float r = Random.Range(0f, 1f);

        r *= 100;

        if (r < characteristics.other.EscapeProbability(this, enemy))
        {
            controller.combatController.UpdatePlayerLog("Has huido.");
            controller.combatController.StopCoroutine(controller.combatController.EndCombatByEscaping(this));
            controller.combatController.StartCoroutine(controller.combatController.EndCombatByEscaping(this));
        }
        else
        {
            controller.combatController.UpdatePlayerLog("No has podido huir.");
        }
    }

    private void Die()
    {
        isAlive = false;
        controller.combatController.UpdatePlayerLog("Has muerto.");
        controller.combatController.StopCoroutine(controller.combatController.EndCombat(this));
        controller.combatController.StartCoroutine(controller.combatController.EndCombat(this));
    }
}

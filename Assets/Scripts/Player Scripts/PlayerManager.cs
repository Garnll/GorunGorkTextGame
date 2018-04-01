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


    public void AttackInCombat(EnemyNPC enemy)
    {
        currentTurn -= (maxTurn * 0.8f);
        controller.combatController.UpdatePlayerTurn();

        int damage = characteristics.currentStrength + Random.Range(1, 5) + Random.Range(0, 3);

        damage *= pacifier;
    
        controller.combatController.UpdatePlayerLog("Has atacado.");
        enemy.ReceiveDamage(damage);
    }

    public void ChargeTurn()
    {
        currentTurn += characteristics.other.currentTurnRegenPerSecond;

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
            yield return new WaitForSeconds(1);
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

    public void ReceiveDamage(int damage)
    {
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

        controller.combatController.UpdatePlayerLog("Has recibido " + damage + " puntos de daño.");
    }

    public void TryToEscape(EnemyNPC enemy)
    {
        currentTurn -= MaxTurn;
        controller.combatController.UpdatePlayerTurn();

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

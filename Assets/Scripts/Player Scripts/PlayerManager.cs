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
    public CharacteristicsChanger characteristicsChanger;
    public CharacterEffectiveState defaultState;
	
    [HideInInspector] public CharacterEffectiveState currentState;

    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float maxTurn = 100;
    [HideInInspector] public float currentTurn;
    [SerializeField] private float maxWill = 10;
    [HideInInspector] public float currentWill;

    public int defaultVisibility = 0;
    [HideInInspector] public int currentVisibility;

    private int experiencePoints = 0;

    private int timePassed = 0;
    public int pacifier = 1;

    public PlayerCharacteristics characteristics;
	public PlayerText playerText;
	public InventoryManager inventoryManager;
	
	
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
        experiencePoints = 0;
        currentHealth = maxHealth;
        currentTurn = maxTurn;
        currentWill = maxWill;
        currentVisibility = defaultVisibility;
        currentState = defaultState;
        defaultState.ApplyStateEffect(this);

        characteristics.InitializeCharacteristics();
        characteristics.other.InitializeOthers();

        if (characteristics.playerRace != null)
        {
            characteristics.playerRace.ActivatePassiveHability(this);
        }

        StartCoroutine(ChargeLife());
    }

    public int CurrentExperiencePoints
    {
        get
        {
            return experiencePoints;
        }
    }

    public void AddExperiencePoints(int howMuch)
    {
        experiencePoints += howMuch;
        controller.LogStringWithReturn("Ganaste " + howMuch + " puntos de experiencia.");
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (playerLevel == 0)
        {
            if (experiencePoints >= CheckLevelUpFormulae(playerLevel + 1))
            {
                playerLevel++;
            }
            else
            {
                return;
            }
        }
        else
        {
            int tempLevelCheck = playerLevel;
            int z = 0;

            while (tempLevelCheck >= 1)
            {
                z += CheckLevelUpFormulae(playerLevel + 1);

                tempLevelCheck--;
            }

            if (experiencePoints >= z)
            {
                playerLevel++;
            }
            else
            {
                return;
            }
        }

        controller.LogStringWithReturn("¡Avanzaste al nivel " + playerLevel + "!");
        characteristics.playerJob.CheckLevelPerks(playerLevel, controller);
    }

    private int CheckLevelUpFormulae(int level)
    {
        return (2 * (level ^ 2) + 8);
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
    }

    public void StartCombat(PlayerInstance enemy)
    {
        currentTurn = 0;
        NetworkManager.Instance.UpdatePlayerData(enemy, this);
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

        float damage = characteristics.currentStrength + Random.Range(1, 5) + Random.Range(0, 3);

        float r = Random.Range(0f, 1f);

        if (r <= characteristics.other.CriticalHitProbability(this))
        {
            controller.combatController.UpdatePlayerLog("¡CRÍTICO!");
            damage *= 2;
        }

        damage *= pacifier;
    

        enemy.ReceiveDamage(damage);
    }

    public void AttackInCombat(PlayerInstance enemy)
    {
        WasteTurn(0.8f);

        controller.combatController.UpdatePlayerLog("Has atacado.");
        NetworkManager.Instance.UpdateOtherPlayersEnemyLog(enemy, "¡" + playerName + " ha atacado!");

        float damage = characteristics.currentStrength + Random.Range(1, 5) + Random.Range(0, 3);

        float r = Random.Range(0f, 1f);

        if (r <= characteristics.other.CriticalHitProbability(this))
        {
            controller.combatController.UpdatePlayerLog("¡CRÍTICO!");
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(enemy, "!CRÍTICO!");
            damage *= 2;
        }

        damage *= pacifier;


        enemy.enemyStats.ReceiveDamage(damage, enemy);
    }

    public void RepositionInCombat()
    {
        WasteTurn(0.5f);

        controller.combatController.UpdatePlayerLog("Te mueves hacia un lado.");
        if (controller.combatController.vsPlayer)
        {
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer, 
                playerName + " se mueve hacia un lado.");
        }

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

        timer.StopCoroutine(timer.RepositionTime((1 + (characteristics.currentIntelligence / 5)), this));
        timer.StartCoroutine(timer.RepositionTime((1 + (characteristics.currentIntelligence / 5)), this));
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

			playerText.updateText();

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

    public void ChangeState(CharacterEffectiveState newState)
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
            if (controller.combatController.vsPlayer)
            {
                NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer,
                    "¡" + playerName + " ha evadido el golpe!");
            }
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
        if (controller.combatController.vsPlayer)
        {
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer,
                playerName + " ha recibido " + damage.ToString("0.#") + " puntos de daño.");
        }
    }

    public void GiveUp(EnemyNPC enemy)
    {
        WasteTurn(1);

        float r = Random.Range(0f, 1f);

        r *= 100;

        if (r < characteristics.other.EscapeProbability(this, enemy))
        {
            controller.combatController.UpdatePlayerLog("Te has rendido.");
            controller.combatController.StopCoroutine(controller.combatController.EndCombatByGivingUp(this));
            controller.combatController.StartCoroutine(controller.combatController.EndCombatByGivingUp(this));
        }
        else
        {
            controller.combatController.UpdatePlayerLog("No has podido rendirte.");
        }
    }

    public void GiveUp(PlayerInstance enemy)
    {
        WasteTurn(1);

        float r = Random.Range(0f, 1f);

        r *= 100;

        if (r < characteristics.other.EscapeProbability(this, enemy))
        {
            controller.combatController.UpdatePlayerLog("Te has rendido.");
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer,
                playerName + " se ha rendido.");
            NetworkManager.Instance.PlayerEscapedBattle(enemy);

            controller.combatController.StopCoroutine(controller.combatController.EndCombatByGivingUp(this));
            controller.combatController.StartCoroutine(controller.combatController.EndCombatByGivingUp(this));
        }
        else
        {
            controller.combatController.UpdatePlayerLog("No has podido rendirte.");
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer,
                "¡" + playerName + " ha intentado rendirse!");
        }
    }

    private void Die()
    {
        isAlive = false;
        controller.combatController.UpdatePlayerLog("Has muerto.");
        if (controller.combatController.vsPlayer)
        {
            NetworkManager.Instance.UpdateOtherPlayersEnemyLog(controller.combatController.enemyPlayer,
                playerName + " ha muerto.");
            NetworkManager.Instance.MyPlayerLoses(controller.combatController.enemyPlayer);
        }

        controller.combatController.StopCoroutine(controller.combatController.EndCombat(this));
        controller.combatController.StartCoroutine(controller.combatController.EndCombat(this));
    }

	public void ModifyHealthBy(float amount) {
		maxHealth += amount;
	}
}


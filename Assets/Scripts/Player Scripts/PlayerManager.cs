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

    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float maxTurn = 100;
    [HideInInspector] public float currentTurn;
    [SerializeField] private float maxWill = 10;
    [HideInInspector] public float currentWill;

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
        characteristics.InitializeCharacteristics();
        characteristics.other.InitializeOthers();

        if (characteristics.playerRace != null)
        {
            characteristics.playerRace.ActivatePassiveHability(this);
        }

        StartCoroutine(ChargeLife());
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


    public void AttackInCombat(EnemyNPC enemy)
    {
        if (currentTurn < maxTurn)
        {
            controller.combatController.UpdatePlayerLog("Aún no es tu turno.");
            return;
        }
        currentTurn -= maxTurn;
        controller.combatController.UpdatePlayerLife();

        int damage = Random.Range(40, 50);
        controller.combatController.UpdatePlayerLog("Has atacado.");
        enemy.ReceiveDamage(damage);
    }

    public void ChargeTurn()
    {
        currentTurn += characteristics.other.currentTurnRegenPerSecond;

        if (currentTurn >= maxTurn)
        {
            currentTurn = maxTurn;
        }
        controller.combatController.UpdatePlayerTurn();

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

        controller.combatController.UpdatePlayerLog("Has recibido " + damage + " puntos de daño.");
    }

    private void Die()
    {
        isAlive = false;
        controller.combatController.UpdatePlayerLog("Has muerto.");
        controller.combatController.StartCoroutine(controller.combatController.EndCombat(this));
    }
}

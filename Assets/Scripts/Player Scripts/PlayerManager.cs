using UnityEngine;

/// <summary>
/// El controlador del jugador. El principal componente de todo jugador, controla vida, nivel, nombre, y demás
/// parametros relevantes. 
/// </summary>
public class PlayerManager : MonoBehaviour {

    public string playerName = "jugador";
    public string gender = "macho";
    public int playerLevel = 0;

    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    [SerializeField] private float maxTurn = 100;
    [HideInInspector] public float currentTurn;
    [SerializeField] private float maxWill = 10;
    [HideInInspector] public float currentWill;

    public PlayerCharacteristics characteristics;

    public GameController controller;


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


}

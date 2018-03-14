using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public string playerName = "jugador";
    public string gender = "macho";
    public int playerLevel = 0;

    [SerializeField] private float maxHealth = 100;
    private float currentHealth;
    [SerializeField] private float maxTurn = 100;
    private float currentTurn;
    [SerializeField] private float maxWill = 10;
    private float currentWill;

    public PlayerCharacteristics characteristics;

    public GameController controller;

    public void Initialize()
    {
        currentHealth = maxHealth;
        currentTurn = maxTurn;
        currentWill = maxWill;
    }


    public void SelectRace(Race raceToBe)
    {
        characteristics.playerRace = raceToBe;
        characteristics.playerRace.ChangePlayerStats(characteristics);
    }

    public void SelectJob(string jobName)
    {

    }
}

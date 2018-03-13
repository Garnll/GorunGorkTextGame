using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private string playerName = "player";
    public int playerLevel = 0;

    [SerializeField] private float maxHealth = 100;
    private float currentHealth = 100;
    [SerializeField] private float maxTurn = 100;
    private float currentTurn = 100;
    [SerializeField] private float maxWill = 10;
    private float currentWill = 10;

    public PlayerCharacteristics characteristics;

    public GameController controller;

    public void Initialize()
    {
        
    }

    public void SelectRace(string raceName)
    {

    }

    public void SelectJob(string jobName)
    {

    }
}

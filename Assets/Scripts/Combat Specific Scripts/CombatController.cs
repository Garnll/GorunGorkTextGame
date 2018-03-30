using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador de todo lo que haría el gamecontroller durante el combate.
/// </summary>
public class CombatController : MonoBehaviour {

    public GameObject combatLayout;
    public RectTransform contentContainer;
    [Space(10)]
    public PlayerUIDuringCombat playerUI;
    public EnemyUIDuringCombat enemyUI;

    private EnemyNPC enemy;
    private Queue<string> enemyLog = new Queue<string>();
    private PlayerManager player;
    private Queue<string> playerLog = new Queue<string>();

    public NPCTemplate TryToFight(string keywordGiven, Room currentRoom)
    {
        for (int i = 0; i < currentRoom.npcsInRoom.Count; i++)
        {
            NPCTemplate npc = currentRoom.npcsInRoom[i];

            if (npc.GetType() == typeof(EnemyNPC))
            {
                foreach (string keyword in npc.keyword)
                {
                    if (keyword == keywordGiven)
                    {
                        return npc;
                    }
                }
            }
        }

        return null;
    }


    public void PrepareFight(NPCTemplate npc, PlayerManager thisPlayer)
    {
        enemy = npc as EnemyNPC;
        player = thisPlayer;

        GameState.Instance.ChangeCurrentState(GameState.GameStates.combat);
    }

    public void StartFight()
    {
        ChangeLayout();
        CancelInvoke();
        ClearCollections();
        StopAllCoroutines();
        InvokeRepeating("UpdateTurns", 1, 1);
    }

    private void ClearCollections()
    {
        enemyLog.Clear();
        playerLog.Clear();
    }

    private void ChangeLayout()
    {
        player.controller.PrepareForCombat();

        GameObject newCombat = Instantiate(combatLayout, contentContainer);

        playerUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        InitializePlayer();
        enemyUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        InitializeEnemy();
    }

    private void UpdateTurns()
    {
        player.ChargeTurn();
        enemy.ChargeBySecond();
    }

    /// <summary>
    /// Recibe y maneja el input del jugador
    /// </summary>
    /// <param name="input"></param>
    public void ReceiveInput(string[] input)
    {
        if (input.Length >= 1)
        {
            if (input.Length == 1)
            {
                switch (input[0])
                {
                    case "0":
                        player.AttackInCombat(enemy);
                        break;

                    default:
                        UpdatePlayerLog("-");
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Inicializar GUI del jugador
    /// </summary>
    private void InitializePlayer()
    {
        player.StartCombat();

        ChangePlayerState();

        UpdatePlayerLife();

        UpdatePlayerTurn();

        UpdatePlayerWill();

        SetPlayerHabilities();

        SetPlayerOptions();

        playerUI.logText.text = ("");

    }

    public void ChangePlayerState ()
    {
        string state = "";

        if (player.currentState != player.defaultState)
        {
            state = " <" + TextConverter.MakeFirstLetterUpper(player.currentState.stateName) + ">";
        }

        playerUI.title.text = TextConverter.MakeFirstLetterUpper(player.playerName) + "\n" +
            TextConverter.MakeFirstLetterUpper(player.characteristics.playerJob.jobName) +
            state;
    }

    public void UpdatePlayerLife()
    {
        playerUI.lifeSlider.maxValue = player.MaxHealth;
        playerUI.lifeSlider.value = player.currentHealth;
        playerUI.lifeText.text = ((player.currentHealth / player.MaxHealth) * 100).ToString() + "%";
    }

    public void UpdatePlayerTurn()
    {
        playerUI.turnSlider.maxValue = player.MaxTurn;
        playerUI.turnSlider.value = player.currentTurn;
    }

    public void UpdatePlayerWill()
    {
        playerUI.willText.text = "V[" + player.currentWill + "/" + player.MaxWill + "]";
    }


    public void SetPlayerHabilities()
    {
        playerUI.habilitiesText.text = "[0] Atacar";
        for (int i = 0; i < player.characteristics.playerJob.habilities.Count; i++)
        {
            Hability currentHability = player.characteristics.playerJob.habilities[i];

            playerUI.habilitiesText.text = "[0." + 
                currentHability.jobIdentifier + "." + 
                currentHability.habilityID + "] " + 
                TextConverter.MakeFirstLetterUpper(currentHability.habilityName);
        }
    }

    public void SetPlayerOptions()
    {
        playerUI.optionsText.text = "[1] Inventario \n" +
            "[2] Reposicionamiento \n" +
            "[3] Escapar (" + player.characteristics.other.EscapeProbability() + "%)";
    }


    public void UpdatePlayerLog(string newLog)
    {
        playerLog.Enqueue(newLog);

        if (playerLog.Count > 3)
        {
            playerLog.Dequeue();
        }

        playerUI.logText.text = string.Join("\n", playerLog.ToArray());
        UpdateEnemyLogOnly("-");
    }

    public void UpdatePlayerLogOnly(string newLog)
    {
        playerLog.Enqueue(newLog);

        if (playerLog.Count > 3)
        {
            playerLog.Dequeue();
        }

        playerUI.logText.text = string.Join("\n", playerLog.ToArray());
    }

    /// <summary>
    /// Inicializar GUI del enemigo
    /// </summary>
    private void InitializeEnemy()
    {
        enemy.StartCombat(this);

        ChangeEnemyState();

        UpdateEnemyLife();

        UpdateEnemyTurn();

        SetEnemyDescription();

        enemyUI.logText.text = ("");
      
    }

    public void ChangeEnemyState()
    {
        string state = "";

        if (enemy.currentState != enemy.defaultState)
        {
            state = " <" + TextConverter.MakeFirstLetterUpper(enemy.currentState.stateName) + ">";
        }

        enemyUI.title.text = TextConverter.MakeFirstLetterUpper(enemy.npcName) + "\n" +
            TextConverter.MakeFirstLetterUpper(enemy.npcJob.jobName) +
            state;
    }

    public void UpdateEnemyLife()
    {
        enemyUI.lifeSlider.maxValue = enemy.MaxHealth;
        enemyUI.lifeSlider.value = enemy.currentHealth;
        enemyUI.lifeText.text = ((enemy.currentHealth / enemy.MaxHealth) * 100).ToString() + "%";
    }

    public void UpdateEnemyTurn()
    {
        enemyUI.turnSlider.maxValue = enemy.MaxTurn;
        enemyUI.turnSlider.value = enemy.currentTurn;
    }


    public void SetEnemyDescription()
    {
        enemyUI.descriptionText.text = enemy.npcDetailedDescription;
    }


    public void UpdateEnemyLog(string newLog)
    {
        enemyLog.Enqueue(newLog);

        if (playerLog.Count > 3)
        {
            enemyLog.Dequeue();
        }

        enemyUI.logText.text = string.Join("\n", enemyLog.ToArray());

        UpdatePlayerLogOnly("-");
    }

    public void UpdateEnemyLogOnly(string newLog)
    {
        enemyLog.Enqueue(newLog);

        if (playerLog.Count > 3)
        {
            enemyLog.Dequeue();
        }

        enemyUI.logText.text = string.Join("\n", enemyLog.ToArray());
    }


    public IEnumerator EndCombat(EnemyNPC loser)
    {
        CancelInvoke();
        GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        player.controller.CreateNewDisplay();
        yield return new WaitForSeconds(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Ganaste!");
    }


    public IEnumerator EndCombat(PlayerManager loser)
    {
        CancelInvoke();
        GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        player.controller.CreateNewDisplay();
        yield return new WaitForSeconds(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Perdiste!");
    }

    private void ReturnToRoom(string endMessage)
    {
        player.controller.LogStringWithoutReturn(" ");
        player.controller.LogStringWithoutReturn("Fin del combate.");
        player.controller.LogStringWithoutReturn(player.controller.RefreshCurrentRoomDescription());
        player.controller.LogStringWithoutReturn(endMessage);
    }
}

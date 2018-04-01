using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador de todo lo que haría el gamecontroller durante el combate.
/// </summary>
public class CombatController : MonoBehaviour {

    public InventoryManager inventoryManager;
    [Space(10)]
    public GameObject combatLayout;
    public RectTransform contentContainer;
    [Space(10)]
    public PlayerUIDuringCombat playerUI;
    public EnemyUIDuringCombat enemyUI;

    List<string> habilitiesText = new List<string>();
    private EnemyNPC enemy;
    private Queue<string> enemyLog = new Queue<string>();
    private PlayerManager player;
    private Queue<string> playerLog = new Queue<string>();

    private bool inInventory = false;
    private int inventoryPage = 1;

    public NPCTemplate TryToFight(string keywordGiven, Room currentRoom)
    {
        for (int i = 0; i < currentRoom.npcTemplatesInRoom.Count; i++)
        {
            NPCTemplate npc = currentRoom.npcTemplatesInRoom[i];

            if (npc.GetType() == typeof(EnemyNPCTemplate))
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


    public void PrepareFight(EnemyNPC npc, PlayerManager thisPlayer)
    {
        enemy = npc;
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
        enemyUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());

        InitializeEnemy();
        InitializePlayer();
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
    public void ReceiveInput(string[] input, HabilitiesTextInput habilitiesInput)
    {
        if (inInventory)
        {
            switch (input[0])
            {
                case "<":
                    if (inventoryManager.DisplayInventory(this, inventoryPage - 1))
                    {
                        inventoryPage -= 1;
                    }
                    break;

                case ">":
                    if (inventoryManager.DisplayInventory(this, inventoryPage + 1))
                    {
                        inventoryPage += 1;
                    }
                    break;

                case "s":
                    ExitInventory();
                    break;

                default:
                    UpdatePlayerLog("-");
                    break;
            }
        }

        if (player.currentTurn < player.MaxTurn && !inInventory)
        {
            UpdatePlayerLog("Aún no es tu turno.");
            return;
        }

        if (input.Length >= 1 && !inInventory)
        {
            if (input.Length == 1)
            {
                switch (input[0])
                {
                    case "0":
                        player.AttackInCombat(enemy);
                        break;

                    case "1":
                        EnterInInventory();
                        break;

                    case "2":
                        UpdatePlayerLog("Aún no sé cómo funcionará reposicionar");
                        break;

                    case "3":
                        player.TryToEscape(enemy);
                        break;

                    default:
                        UpdatePlayerLog("-");
                        break;
                }
            }
            else
            {
                habilitiesInput.CheckHabilitiesInputDuringCombat(input, player.controller, enemy);
                UpdatePlayerTurn();
            }
        }
        else if (inInventory)
        {
            switch (input[0])
            {
                case "0":
                    
                    break;

                case "1":
                    
                    break;

                case "2":
                    
                    break;

                default:
                    UpdatePlayerLog("-");
                    break;
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
            UpdatePlayerLog("¡Has entrado en " + TextConverter.MakeFirstLetterUpper(player.currentState.stateName) 
                + "!");
        }

        playerUI.title.text = "<b>" + TextConverter.MakeFirstLetterUpper(player.playerName) + "</b>" + "\n" +
            TextConverter.MakeFirstLetterUpper(player.characteristics.playerJob.jobName) +
            state;
    }

    public void UpdatePlayerLife()
    {
        playerUI.lifeSlider.maxValue = player.MaxHealth;
        playerUI.lifeSlider.value = player.currentHealth;
        playerUI.lifeText.text = ((player.currentHealth / player.MaxHealth) * 100).ToString("#") + "%";
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
        if (inInventory)
        {
            return;
        }

        habilitiesText.Clear();

        habilitiesText.Add("[0] Atacar");
        for (int i = 0; i < player.characteristics.playerJob.habilities.Count; i++)
        {
            Hability currentHability = player.characteristics.playerJob.habilities[i];

            if (currentHability.isAvailable)
            {
                habilitiesText.Add("[0." +
                    currentHability.jobIdentifier + "." +
                    currentHability.habilityID + "] " +
                    TextConverter.MakeFirstLetterUpper(currentHability.habilityName));
            }
            else
            {
                habilitiesText.Add("<color=#696969>" + 
                    "[0." +
                    currentHability.jobIdentifier + "." +
                    currentHability.habilityID + "] " +
                    TextConverter.MakeFirstLetterUpper(currentHability.habilityName) +
                    ". . . . . . "+
                    "</color>");
            }
        }

        playerUI.habilitiesText.text = string.Join("\n", habilitiesText.ToArray());
    }

    public void SetPlayerHabilities(string newText)
    {
        playerUI.habilitiesText.text = newText;
    }

    public void SetPlayerOptions()
    {
        if (inInventory)
        {
            return;
        }

        inventoryPage = 1;

        playerUI.optionsText.text = "[1] Inventario \n" +
            "[2] Reposicionamiento \n" +
            "[3] Escapar (" + player.characteristics.other.EscapeProbability(player, enemy).ToString("#") + "%)";
    }

    public void EnterInInventory()
    {
        inInventory = true;
        UpdatePlayerLog("Abres el inventario");
        player.currentTurn -= player.MaxTurn * 0.5f;
        inventoryManager.DisplayInventory(this, inventoryPage);
    }

    public void ExitInventory()
    {
        inInventory = false;
        UpdatePlayerLog("Sales del inventario");
        SetPlayerOptions();
        SetPlayerHabilities();
    }

    public void SetPlayerInventoryOptions(string newText)
    {
        playerUI.optionsText.text = newText;
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

        if (enemy.currentState != enemy.myTemplate.defaultState)
        {
            state = " <" + TextConverter.MakeFirstLetterUpper(enemy.currentState.stateName) + ">";
            UpdatePlayerLog("El " + TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcName) +
               " ha entrado en " + TextConverter.MakeFirstLetterUpper(enemy.currentState.stateName)
                + "!");
        }

        enemyUI.title.text = TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcName) + "\n" +
            TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcJob.jobName) +
            state;
    }

    public void UpdateEnemyLife()
    {
        enemyUI.lifeSlider.maxValue = enemy.myTemplate.MaxHealth;
        enemyUI.lifeSlider.value = enemy.currentHealth;
        enemyUI.lifeText.text = ((enemy.currentHealth / enemy.myTemplate.MaxHealth) * 100).ToString("#") + "%";
    }

    public void UpdateEnemyTurn()
    {
        enemyUI.turnSlider.maxValue = enemy.myTemplate.MaxTurn;
        enemyUI.turnSlider.value = enemy.currentTurn;
    }


    public void SetEnemyDescription()
    {
        enemyUI.descriptionText.text = enemy.myTemplate.npcDetailedDescription;
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

    public IEnumerator EndCombatByEscaping(EnemyNPC runner)
    {
        CancelInvoke();
        GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        player.controller.CreateNewDisplay();
        yield return new WaitForSeconds(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡El enemigo huyó!");
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

    public IEnumerator EndCombatByEscaping(PlayerManager loser)
    {
        CancelInvoke();
        GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        player.controller.CreateNewDisplay();
        yield return new WaitForSeconds(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Huiste!");
    }


    private void ReturnToRoom(string endMessage)
    {
        player.controller.LogStringWithoutReturn(" ");
        player.controller.LogStringWithoutReturn("Fin del combate.");
        player.controller.LogStringWithoutReturn(player.controller.RefreshCurrentRoomDescription());
        player.controller.LogStringWithoutReturn(endMessage);
    }
}

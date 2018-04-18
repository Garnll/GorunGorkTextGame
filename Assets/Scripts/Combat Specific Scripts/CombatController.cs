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

    /// <summary>
    /// Revisa entre los templates de la habitación actual buscando el keyword dado por el jugador.
    /// Devuelve el enemigo que encuentre.
    /// </summary>
    /// <param name="keywordGiven"></param>
    /// <param name="currentRoom"></param>
    /// <returns></returns>
    public NPCTemplate TryToFight(string[] keywordGiven, Room currentRoom)
    {
		string[] newString = new string[keywordGiven.Length - 1];

		for (int i = 1; i < keywordGiven.Length; i++) {
			newString[i - 1] = keywordGiven[i];
		}

		for (int i = 0; i < currentRoom.npcTemplatesInRoom.Count; i++)
        {
            NPCTemplate npc = currentRoom.npcTemplatesInRoom[i];

            if (npc.GetType() == typeof(EnemyNPCTemplate))
            {
				foreach (string keyword in npc.keyword) {
					foreach (string keyworkByPlayer in newString) {
						if (keyword == keyworkByPlayer) {
							return npc;
						}
					}
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Comienza preparaciones del combate, inicializa variables de enemy y player.
    /// </summary>
    /// <param name="npc"></param>
    /// <param name="thisPlayer"></param>
    public void PrepareFight(EnemyNPC npc, PlayerManager thisPlayer)
    {
        enemy = npc;
        player = thisPlayer;

        GameState.Instance.ChangeCurrentState(GameState.GameStates.combatPreparation);
    }

    /// <summary>
    /// Inicia la batalla una vez la UI de exploración haya acabado de copiar.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartFight()
    {
        yield return new WaitUntil(() => player.controller.writing == false && player.controller.HasFinishedWriting());

        GameState.Instance.ChangeCurrentState(GameState.GameStates.combat);

        ChangeLayout();

        InitializeEnemy();
        InitializePlayer();

        CancelInvoke();
        ClearCollections();
        StopAllCoroutines();
        StartCoroutine(UpdateTurns());

        if (enemy.myAI == null)
        {
            enemy.myAI = enemy.GetComponent<EnemyNPCAI>();
        }
        enemy.myAI.player = player;
        enemy.myAI.myNPC = enemy;
        enemy.myAI.StartAI();

    }

    /// <summary>
    /// Limpia los logs antiguos de jugador y enemigo.
    /// </summary>
    private void ClearCollections()
    {
        enemyLog.Clear();
        playerLog.Clear();
    }

    /// <summary>
    /// Cambia el layout del juego para volverse el que debe aparecer durante el comabte.
    /// </summary>
    private void ChangeLayout()
    {
        player.controller.PrepareForCombat();

        GameObject newCombat = Instantiate(combatLayout, contentContainer);

        playerUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        enemyUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
    }

    private IEnumerator UpdateTurns()
    {
        while (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            yield return new WaitForSecondsRealtime(1);
            player.ChargeTurn();
            enemy.ChargeBySecond();
        }
    }

    /// <summary>
    /// Recibe y maneja el input del jugador durante este estado de combate.
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
                        player.RepositionInCombat();
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
    /// Inicializa GUI del jugador.
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

    /// <summary>
    /// Cambia el tituloo del jugador para reflejar su CharacterState actual.
    /// </summary>
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
            TextConverter.MakeFirstLetterUpper(player.characteristics.playerRace.raceName) + " " +
            TextConverter.MakeFirstLetterUpper(player.characteristics.playerJob.jobName) +
            state;
    }

    /// <summary>
    /// Cambia la GUI del jugador para mostrar la vida actual.
    /// </summary>
    public void UpdatePlayerLife()
    {
        playerUI.lifeSlider.maxValue = player.MaxHealth;
        playerUI.lifeSlider.value = player.currentHealth;
        playerUI.lifeText.text = ((player.currentHealth / player.MaxHealth) * 100).ToString("0") + "%";
    }

    /// <summary>
    /// Cambia la GUI del jugador para mostrar su carga de turno actual.
    /// </summary>
    public void UpdatePlayerTurn()
    {
        playerUI.turnSlider.maxValue = player.MaxTurn;
        playerUI.turnSlider.value = player.currentTurn;
    }

    /// <summary>
    /// Cambia la GUI del jugador para mostrar su voluntad actual.
    /// </summary>
    public void UpdatePlayerWill()
    {
        playerUI.willText.text = "V[" + player.currentWill + "/" + player.MaxWill + "]";
    }

    /// <summary>
    /// Muestra las habilidades disponibles del jugador.
    /// </summary>
    public void SetPlayerHabilities()
    {
        if (inInventory)
        {
            return;
        }

        habilitiesText.Clear();

        habilitiesText.Add("[0] Atacar");
        for (int i = 0; i < player.characteristics.playerJob.jobHabilities.Count; i++)
        {
            Hability currentHability = player.characteristics.playerJob.jobHabilities[i];

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

    /// <summary>
    /// Pone un texto alterno en el lugar donde irían las habilidades del jugador.
    /// </summary>
    public void SetPlayerHabilities(string newText)
    {
        playerUI.habilitiesText.text = newText;
    }

    /// <summary>
    /// Cambia la GUI del jugador para mostrar sus opciones actuales.
    /// </summary>
    public void SetPlayerOptions()
    {
        if (inInventory)
        {
            return;
        }

        inventoryPage = 1;

        playerUI.optionsText.text = "[1] Inventario \n" +
            "[2] Reposicionamiento \n" +
            "[3] Escapar (" + player.characteristics.other.EscapeProbability(player, enemy).ToString("0") + "%)";
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

    /// <summary>
    /// Actualiza el log actual del jugador, mostrando eventos que él realice o le sucedan.
    /// </summary>
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

    /// <summary>
    /// Actualiza el log actual del jugador sin enviarle un mensaje extra al log del enemigo.
    /// </summary>
    /// <param name="newLog"></param>
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
    /// Inicializa GUI del enemigo.
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

    /// <summary>
    /// Cambia la GUI del npc enemigo para reflejar su CharacterState actual.
    /// </summary>
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

        enemyUI.title.text = "<b>" + TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcName) + "</b>" + "\n" +
              TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcRace.raceName) + " " +
            TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcJob.jobName) +
            state;

        enemy.myTemplate.npcRace.ActivatePassiveHability(enemy);
    }

    /// <summary>
    /// Cambia la GUI del npc enemigo mostrando su vida actual.
    /// </summary>
    public void UpdateEnemyLife()
    {
        enemyUI.lifeSlider.maxValue = enemy.myTemplate.MaxHealth;
        enemyUI.lifeSlider.value = enemy.currentHealth;
        enemyUI.lifeText.text = ((enemy.currentHealth / enemy.myTemplate.MaxHealth) * 100).ToString("0") + "%";
    }

    /// <summary>
    /// Cambia la GUI del npc enemigo mostrando su carga de turno actual.
    /// </summary>
    public void UpdateEnemyTurn()
    {
        enemyUI.turnSlider.maxValue = enemy.myTemplate.MaxTurn;
        enemyUI.turnSlider.value = enemy.currentTurn;
    }

    /// <summary>
    /// Cambia la GUI del npc enemigo mostrando su descipción detallada.
    /// </summary>
    public void SetEnemyDescription()
    {
        enemyUI.descriptionText.text = enemy.myTemplate.npcDetailedDescription;
    }

    /// <summary>
    /// Actualiza el Log del enemigo mostrando sus acciones y cosas que recibe. Envia un mensaje "-" al log del jugador.
    /// </summary>
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

    /// <summary>
    /// Actualiza el Log del enemigo mostrando acciones y consecuencias de acciones. No envía mensajes al log del jugador.
    /// </summary>
    public void UpdateEnemyLogOnly(string newLog)
    {
        enemyLog.Enqueue(newLog);

        if (playerLog.Count > 3)
        {
            enemyLog.Dequeue();
        }

        enemyUI.logText.text = string.Join("\n", enemyLog.ToArray());
    }


    /// <summary>
    /// Termina el combate con el Npc Enemigo como perdedor.
    /// </summary>
    public IEnumerator EndCombat(EnemyNPC loser)
    {
        EndAllCombat();
        yield return new WaitForSecondsRealtime(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Ganaste!");
    }

    /// <summary>
    /// Termina el combate cuando el npc enemigo huye.
    /// </summary>
    public IEnumerator EndCombatByEscaping(EnemyNPC runner)
    {
        EndAllCombat();
        yield return new WaitForSecondsRealtime(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡El enemigo huyó!");
    }

    /// <summary>
    /// Termina el combate con el jugador como perdedor.
    /// </summary>
    public IEnumerator EndCombat(PlayerManager loser)
    {
        EndAllCombat();
        yield return new WaitForSecondsRealtime(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Perdiste!");
    }

    /// <summary>
    /// Termina el combate cuando el jugador huye.
    /// </summary>
    public IEnumerator EndCombatByEscaping(PlayerManager runner)
    {
        EndAllCombat();
        yield return new WaitForSecondsRealtime(2);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        ReturnToRoom("¡Huiste!");
    }

    /// <summary>
    /// Termina el estado del combate en todos sus sentidos.
    /// </summary>
    private void EndAllCombat()
    {
        enemy.myAI.StopAI();
        CancelInvoke();
        GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        player.controller.CreateNewDisplay();
    }

    /// <summary>
    /// Devuelve el jugador a la habitación en la que estaba antes del combate.
    /// </summary>
    /// <param name="endMessage"></param>
    private void ReturnToRoom(string endMessage)
    {
        player.controller.PrepareForCombat();
        player.controller.LogStringWithReturn(" ");
        player.controller.LogStringWithReturn("Fin del combate.");
        player.controller.LogStringWithReturn(player.controller.RefreshCurrentRoomDescription());
        player.controller.LogStringWithoutReturn(endMessage);
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Text;

/// <summary>
/// Controlador del juego. Maneja gran parte del texto y parte de las mecanicas.
/// </summary>
public class GameController : MonoBehaviour {

    public GameObject displayTextTemplate;
    [Range(0, 0.1f)] public float textvelocity = 0.01f;
    public int textMaxLength = 1000;
    public int textMaxDisplays = 50;
    public RectTransform contentContainer;
    public InputActions[] inputActions;
    public PlayerManager playerManager;
    public PlayerRoomNavigation playerRoomNavigation;
    public CombatController combatController;
	public DialogueController dialogueController;
	public CraftingController craftController;
	public QuestManager questManager;
	public PlayerText playerText;

    [HideInInspector] public List<string> exitDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<string> npcDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<string> playerDescriptionssInRoom = new List<string>();
    [HideInInspector] public ItemHandler itemHandler;

    TextMeshProUGUI currentDisplayText;
    List<string> actionLog = new List<string>();
    List<string> roomExtraLog = new List<string>();
    string currentRoomDescription = "";

    [HideInInspector] public int currentCharPosition = 0;
    string oldText = "";
    [HideInInspector] public bool writing;
    [HideInInspector] public bool stopWriting;
    int displayTextCounter = 0;
    List<TextMeshProUGUI> displayTexts;

    bool isConnecting;

    private void Start()
    {
        displayTexts = new List<TextMeshProUGUI>(textMaxDisplays + 1);
        itemHandler = GetComponent<ItemHandler>();
        writing = false;
        stopWriting = false;
        isConnecting = false;

        DisplayRoomText();
        DisplayLoggedText();
    }

    /// <summary>
    /// Muestra todo lo que haya en el string logAsText en el GUI.
    /// </summary>
    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());

        if (currentDisplayText == null)
        {
            StopCoroutine("AnimateText");
            oldText = "\n";
            currentCharPosition = 0;
            CreateNewDisplay();
            writing = false;
            stopWriting = false;
            currentDisplayText.autoSizeTextContainer = false;
            currentDisplayText.autoSizeTextContainer = true;
        }

        if (writing)
        {
            stopWriting = true;
        }

		playerText.updateText();

        StartCoroutine(AnimateText(logAsText));
    }

    public bool HasFinishedWriting()
    {
        if (currentCharPosition == string.Join("\n", actionLog.ToArray()).Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator AnimateText(string complete)
    {
        if (writing)
        {
            yield return new WaitUntil(() => !writing);           
        }

        if (!writing)
        {
            writing = true;
            StringBuilder str = new StringBuilder();
            str.Append(oldText);
            while (currentCharPosition < complete.Length)
            {
                if (stopWriting)
                {
                    str.Length = 0;
                    str.Append(complete);
                    currentCharPosition = complete.Length;
                    currentDisplayText.text = str.ToString();
                    stopWriting = false;
                    oldText = currentDisplayText.text;
                    writing = false;
                    currentDisplayText.autoSizeTextContainer = false;
                    currentDisplayText.autoSizeTextContainer = true;
                    yield break;
                }

                if (currentCharPosition < complete.Length)
                {
                    if (complete[currentCharPosition] == '<')
                    {
                        while (complete[currentCharPosition] != '>')
                        {
                            str.Append(complete[currentCharPosition++]);
                            if (currentCharPosition >= complete.Length)
                            {
                                break;
                            }
                        }
                    }
                }

                str.Append(complete[currentCharPosition++]);

                currentDisplayText.text = str.ToString();
                yield return new WaitForSeconds(textvelocity);
            }

            if (writing && currentDisplayText != null)
            {
                oldText = currentDisplayText.text;
            }
            writing = false;

            if (currentCharPosition >= textMaxLength)
            {
                NullCurrentDisplay();
            }
        }
    }

    public IEnumerator StopTextWhileConnecting()
    {
        LogStringWithoutReturn("<b><color=#E5825E>Conectando.</color></b>\n" + 
			"Despiertas dentro de una cabina cerrada. Espera a que se abra.");

        yield return new WaitUntil(() => NetworkManager.Instance.connected);

        LogStringWithoutReturn("<b><color=#ABCA3CFF>En línea.</color></b>\n" +
			"La cabina se abre,acompañada de vapor. Ya puedes salir.");
    }

    public void CreateNewDisplay()
    {
        if (displayTexts.Count < textMaxDisplays)
        {
            GameObject newDisplay = Instantiate(displayTextTemplate, contentContainer);
            currentDisplayText = newDisplay.GetComponent<TextMeshProUGUI>();
            displayTexts.Add(currentDisplayText);       
        }
        else
        {
            currentDisplayText = displayTexts[displayTextCounter];
            currentDisplayText.transform.SetAsLastSibling();

            displayTextCounter++;

            if (displayTextCounter >= textMaxDisplays)
            {
                displayTextCounter = 0;
            }
        }

        currentDisplayText.text = "";
    }

    public void NullCurrentDisplay()
    {
        if (writing)
        {
            stopWriting = true;
        }

        currentDisplayText = null;
        actionLog.Clear();
    }

    /// <summary>
    /// Muestra todo el texto que la habitación (y pertinentes) envíe, justo cuando el jugador
    /// entra en una nueva habitación.
    /// </summary>
    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();

        string combinedText = RoomDescription();

        currentRoomDescription = combinedText;

        combinedText += string.Join("\n", roomExtraLog.ToArray());

        LogStringWithReturn(combinedText);
    }

    /// <summary>
    /// De la habitación actual, se sacan las salidas, los objetos y las respuestas a ser mostradas
    /// por DisplayRoomText.
    /// </summary>
    private void UnpackRoom()
    {
        playerRoomNavigation.AddExitsToController();

        PrepareObjectsToBeInteracted(playerRoomNavigation.currentRoom);

        playerRoomNavigation.CheckForNPCs();

        playerRoomNavigation.CheckPlayersInRoom();

        playerRoomNavigation.TriggerRoomResponse();
    }

    private void PrepareObjectsToBeInteracted(RoomObject currentRoom)
    {
        for (int i = 0; i < currentRoom.visibleObjectsInRoom.Count; i++)
        {
            if (currentRoom.visibleObjectsInRoom[i].visionRange >= playerManager.characteristics.vision.x &&
                currentRoom.visibleObjectsInRoom[i].visionRange <= playerManager.characteristics.vision.y)
            {
                string descriptionNotInInventory = currentRoom.visibleObjectsInRoom[i].interactableObject.description;
                if (descriptionNotInInventory != null)
                {
                    interactionDescriptionsInRoom.Add(descriptionNotInInventory);
                }
            }
        }
    }

    private string RoomDescription()
    {
		string combinedText = "<color=#F9EEC1><b>" + playerRoomNavigation.currentRoom.roomName + ".</b></color>\n"
			+ playerRoomNavigation.currentRoom.roomDescription + "\n";

		string joinedExitDescriptions = string.Join("\n", exitDescriptionsInRoom.ToArray());
        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());
        string joinedNPCDescriptions = string.Join("\n", npcDescriptionsInRoom.ToArray());
        string joinedPlayers = string.Join("\n", playerDescriptionssInRoom.ToArray());

		if (interactionDescriptionsInRoom.ToArray().Length != 0) {
			combinedText += joinedInteractionDescriptions + "\n";
		}


		if (npcDescriptionsInRoom.ToArray().Length != 0) {
			combinedText += "\n";
			combinedText += joinedNPCDescriptions + "\n";
		}

		if (playerDescriptionssInRoom.ToArray().Length != 0) {
			combinedText += joinedPlayers + "\n";
		}

		if (exitDescriptionsInRoom.ToArray().Length != 0) {
			combinedText += "\n";
			combinedText += joinedExitDescriptions + "\n";
		}

		return combinedText;
    }

    /// <summary>
    /// Función que devuelve una string que contiene una versión actualizada de la información
    /// de la habitación.
    /// </summary>
    /// <returns></returns>
    public string RefreshCurrentRoomDescription()
    {
        ClearCollectionsForNewRoom();
        UnpackRoom();

        string combinedText = RoomDescription();

        currentRoomDescription = combinedText;

        if (playerRoomNavigation.currentRoom.roomResponse != null)
        {
            combinedText += string.Join("\n", playerRoomNavigation.currentRoom.roomResponse.responses);
        }


        return combinedText;
    }

    /// <summary>
    /// Revisa que el input dado por el jugador si sea pertinente al keyword de alguno de los InputAction.
    /// </summary>
    /// <param name="verbDictionary"></param>
    /// <param name="verb"></param>
    /// <param name="noun"></param>
    /// <returns></returns>
    public string TestVerbDictionaryWithNoun(Dictionary<string, string> verbDictionary, string verb, string noun)
    {
        if (noun == "habitacion" || noun == "" || noun == "lugar")
        {
            return currentRoomDescription;
        }

        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        string objectToDisplay = noun;

        return "No se puede " + verb + " " + objectToDisplay;
    }

    void ClearCollectionsForNewRoom()
    {
        roomExtraLog.Clear();
        interactionDescriptionsInRoom.Clear();
        playerRoomNavigation.ClearExits();
        exitDescriptionsInRoom.Clear();
        npcDescriptionsInRoom.Clear();
        playerDescriptionssInRoom.Clear();
    }

    /// <summary>
    /// Añade una linea de texto entre dos espacios en blanco. Útil para mandar mensajes de acciones.
    /// </summary>
    /// <param name="stringToAdd"></param>
    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

    /// <summary>
    /// Añade una linea de texto después del texto pertinene a la habitación. Útil para mandar mensajes extra
    /// al entrar en una habitacion.
    /// </summary>
    /// <param name="stringToAdd"></param>
    public void LogStringAfterRoom(string stringToAdd)
    {
        roomExtraLog.Add(stringToAdd + "\n");
    }

    public void LogStringWithoutReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
        DisplayLoggedText();
    }
}

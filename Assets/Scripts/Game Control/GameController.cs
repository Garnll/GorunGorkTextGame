using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Controlador del juego. Maneja gran parte del texto y parte de las mecanicas.
/// </summary>
public class GameController : MonoBehaviour {

    public GameObject displayTextTemplate;
    [Range(0, 0.1f)] public float textvelocity = 0.01f;
    public RectTransform contentContainer;
    public InputActions[] inputActions;
    public PlayerManager playerManager;
    public PlayerRoomNavigation playerRoomNavigation;
    public CombatController combatController;

    [HideInInspector] public List<string> exitDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    [HideInInspector] public List<string> npcDescriptionsInRoom = new List<string>();
    [HideInInspector] public ItemHandler itemHandler;

    TextMeshProUGUI displayText;
    List<string> actionLog = new List<string>();
    List<string> roomExtraLog = new List<string>();
    string currentRoomDescription = "";

    [HideInInspector] public int currentCharPosition = 0;
    string oldText = "";
    [HideInInspector] public bool writing;
    [HideInInspector] public bool stopWriting;

    private void Start()
    {
        itemHandler = GetComponent<ItemHandler>();
        writing = false;
        stopWriting = false;

        DisplayRoomText();
        DisplayLoggedText();
    }

    /// <summary>
    /// Muestra todo lo que haya en el string logAsText en el GUI.
    /// </summary>
    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());

        if (displayText == null)
        {
            StopAllCoroutines();

            oldText = "";
            currentCharPosition = 0;
            CreateNewDisplay();
            writing = false;
            stopWriting = false;
        }

        if (writing)
        {
            stopWriting = true;
        }

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
            while (writing)
            {
                yield return new WaitUntil(() => !writing);
            }
        }

        if (!writing)
        {
            writing = true;
            string str = oldText;
            while (currentCharPosition < complete.Length)
            {
                if (stopWriting)
                {
                    str = complete;
                    currentCharPosition = complete.Length;
                    displayText.text = str;
                    stopWriting = false;
                    oldText = displayText.text;
                    writing = false;
                   
                    yield break;
                }

                if (currentCharPosition < complete.Length)
                {
                    if (complete[currentCharPosition] == '<')
                    {
                        while (complete[currentCharPosition] != '>')
                        {
                            str += complete[currentCharPosition++];
                            if (currentCharPosition >= complete.Length)
                            {
                                break;
                            }
                        }
                    }
                }

                str += complete[currentCharPosition++];

                displayText.text = str;
                yield return new WaitForSeconds(textvelocity);
            }

            if (writing && displayText != null)
            {
                oldText = displayText.text;
            }
            writing = false;
        }
    }

    public void CreateNewDisplay()
    {
        GameObject newDisplay = Instantiate(displayTextTemplate, contentContainer);
        displayText = newDisplay.GetComponent<TextMeshProUGUI>();
        displayText.text = "";
    }

    public void PrepareForCombat()
    {
        displayText = null;
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


        //if (playerRoomNavigation.currentRoom.roomResponse != null)
        //{
        //    combinedText += string.Join("\n", playerRoomNavigation.currentRoom.roomResponse.responses);
        //}
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

        playerRoomNavigation.currentPosition = playerRoomNavigation.currentRoom.roomPosition;

        playerRoomNavigation.miniMapper.MovePlayerInMap(playerRoomNavigation.currentPosition);

        playerRoomNavigation.TriggerRoomResponse();
    }

    private void PrepareObjectsToBeInteracted(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.visibleObjectsInRoom.Count; i++)
        {
            if (currentRoom.visibleObjectsInRoom[i].visionRange >= playerManager.characteristics.vision.x &&
                currentRoom.visibleObjectsInRoom[i].visionRange <= playerManager.characteristics.vision.y)
            {
                string descriptionNotInInventory = currentRoom.visibleObjectsInRoom[i].interactableObject.description;
                if (descriptionNotInInventory != null)
                {
                    interactionDescriptionsInRoom.Add(descriptionNotInInventory + ".");
                }
            }
        }
    }

    private string RoomDescription()
    {
        string joinedExitDescriptions = string.Join("\n", exitDescriptionsInRoom.ToArray());
        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());
        string joinedNPCDescriptions = string.Join("\n", npcDescriptionsInRoom.ToArray());

        string combinedText = playerRoomNavigation.currentRoom.roomDescription + "\n" + joinedExitDescriptions + "\n"
            + "\n" + joinedInteractionDescriptions + "\n" + joinedNPCDescriptions;

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

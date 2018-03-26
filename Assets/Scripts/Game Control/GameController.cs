using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controlador del juego. Maneja gran parte del texto y parte de las mecanicas.
/// </summary>
public class GameController : MonoBehaviour {

    public TextMeshProUGUI displayTextTemplate;
    public RectTransform contentContainer;
    public InputActions[] inputActions;
    public PlayerManager playerManager;
    public PlayerRoomNavigation playerRoomNavigation;
    public NPCController npcController;

    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    [HideInInspector] public ItemHandler itemHandler;

    TextMeshProUGUI displayText;
    List<string> actionLog = new List<string>();
    List<string> roomExtraLog = new List<string>();
    string currentRoomDescription = "";

    private void Start()
    {
        itemHandler = GetComponent<ItemHandler>();

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
            displayText = Instantiate(displayTextTemplate, contentContainer);
        }

        displayText.text = logAsText;
    }

    public void PrepareForCombat()
    {

    }

    /// <summary>
    /// Muestra todo el texto que la habitación (y pertinentes) envíe, justo cuando el jugador
    /// entra en una nueva habitación.
    /// </summary>
    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();

        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());

        currentRoomDescription = playerRoomNavigation.currentRoom.roomDescription 
            + " " + joinedInteractionDescriptions;

        string combinedText = playerRoomNavigation.currentRoom.roomDescription 
            + "\n" + joinedInteractionDescriptions;


        if (playerRoomNavigation.currentRoom.roomResponse != null)
        {
            combinedText += string.Join("\n", playerRoomNavigation.currentRoom.roomResponse.responses);
        }
        combinedText += string.Join("\n", roomExtraLog.ToArray());

        LogStringWithReturn(combinedText);
    }

    /// <summary>
    /// De la habitación actual, se sacan las salidas, los objetos y las respuestas a ser mostradas
    /// por DisplayRoomText.
    /// </summary>
    private void UnpackRoom()
    {
        PrepareObjectsToBeInteracted(playerRoomNavigation.currentRoom);
        playerRoomNavigation.AddExitsToController();

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
                    interactionDescriptionsInRoom.Add(descriptionNotInInventory);
                }
            }
        }
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
        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());

        currentRoomDescription = playerRoomNavigation.currentRoom.roomDescription + " " + joinedInteractionDescriptions;

        string combinedText = playerRoomNavigation.currentRoom.roomDescription + "\n" + joinedInteractionDescriptions;
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
}

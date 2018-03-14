using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public TextMeshProUGUI displayText;
    public InputActions[] inputActions;
    public PlayerManager playerManager;
    public PlayerRoomNavigation playerRoomNavigation;

    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    //[HideInInspector] public InteractableItems interactableItems;
    [HideInInspector] public ItemHandler itemHandler;

    List<string> actionLog = new List<string>();
    List<string> roomExtraLog = new List<string>();
    string currentRoomDescription = "";

    private void Start()
    {
        //interactableItems = GetComponent<InteractableItems>();
        itemHandler = GetComponent<ItemHandler>();

        DisplayRoomText();
        DisplayLoggedText();
    }

    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());

        displayText.text = logAsText;
    }

    public void DisplayRoomText()
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
        combinedText += string.Join("\n", roomExtraLog.ToArray());

        LogStringWithReturn(combinedText);
    }

    private void UnpackRoom()
    {
        playerRoomNavigation.AddExitsToController();
        PrepareObjectsToBeInteracted(playerRoomNavigation.currentRoom);

        playerRoomNavigation.TriggerRoomResponse();
    }

    private void PrepareObjectsToBeInteracted(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            string descriptionNotInInventory = currentRoom.interactableObjectsInRoom[i].description;
            if (descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }
        }
        }

    public string RefreshedCurrentRoomDescription()
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

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

    public void LogStringAfterRoom(string stringToAdd)
    {
        roomExtraLog.Add(stringToAdd + "\n");
    }
}

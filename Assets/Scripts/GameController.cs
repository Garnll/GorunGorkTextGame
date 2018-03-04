using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public TextMeshProUGUI displayText;
    public InputActions[] inputActions;
    public PlayerRoomNavigation playerRoomNavigation;

    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    [HideInInspector] public InteractableItems interactableItems;

    List<string> actionLog = new List<string>();

    private void Start()
    {
        interactableItems = GetComponent<InteractableItems>();

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

        string combinedText = playerRoomNavigation.currentRoom.roomDescription + "\n" + joinedInteractionDescriptions;

        LogStringWithReturn(combinedText);
    }

    private void UnpackRoom()
    {
        playerRoomNavigation.UnpackedExitsInRoom();
        PrepareObjectsToBeInteracted(playerRoomNavigation.currentRoom);
    }

    private void PrepareObjectsToBeInteracted(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Count; i++)
        {
            string descriptionNotInInventory = interactableItems.GetObjectNotInInventory(currentRoom, i);
            if (descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }

            if (!currentRoom.interactableObjectsInRoom[i].isTaken)
            {
                InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i].interactableObject;

                SetInteractions(interactableInRoom);
            }
        }

        for (int i = 0; i < interactableItems.inventoryManager.nounsInInventory.Count; i++)
        {
            InteractableObject interactableInRoom = interactableItems.inventoryManager.nounsInInventory[i];
            SetInteractions(interactableInRoom);
        }

        interactableItems.GetObjectsInInventory();
    }

    private void SetInteractions(InteractableObject interactable)
    {
        for (int j = 0; j < interactable.interactions.Length; j++)
        {
            Interaction interaction = interactable.interactions[j];

            if (interaction.isInverseInteraction)
            {
                interactableItems.inverseNouns.Add(interactable.noun + interaction.textResponse, interaction);
            }

            if (interaction.inputAction.GetType() == typeof(Examine))
            {
                interactableItems.examineDictionary.Add(interactable.noun, interaction.textResponse);
            }
            else if (interaction.inputAction.GetType() == typeof(Take))
            {
                interactableItems.takeDictionary.Add(interactable.noun, interaction.textResponse);
            }
            else if (interaction.inputAction.GetType() == typeof(Throw))
            {
                interactableItems.throwDictionary.Add(interactable.noun, interaction.textResponse);
            }
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string, string> verbDictionary, string verb, string noun)
    {
        if (verbDictionary.ContainsKey(noun))
        {
            return verbDictionary[noun];
        }

        string objectToDisplay = noun;

        if (interactableItems.objectsWithinReachDictionary.ContainsKey(objectToDisplay))
        {
            if (interactableItems.objectsWithinReachDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + noun;
            else
                objectToDisplay = "la " + noun;
        }

        return "No se puede " + verb + " " + objectToDisplay;
    }

    void ClearCollectionsForNewRoom()
    {
        interactableItems.ClearCollections();
        interactionDescriptionsInRoom.Clear();
        playerRoomNavigation.ClearExits();
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

}

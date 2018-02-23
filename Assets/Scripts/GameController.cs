using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour {

    public TextMeshProUGUI displayText;
    //public Text displayText;
    public InputActions[] inputActions;
    public PlayerRoomNavigation playerRoomNavigation;

    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();

    List<string> actionLog = new List<string>();

    private void Start()
    {
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
    }

    void ClearCollectionsForNewRoom()
    {
        interactionDescriptionsInRoom.Clear();
        playerRoomNavigation.ClearExits();
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomNavigation : MonoBehaviour {

    public GameController controller;
    public Room currentRoom;

    Vector3 currentPosition;

    Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    public void UnpackedExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].myKeyword, currentRoom.exits[i].conectedRoom);

            controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
        }
    }

    public void AttemptToChangeRooms(string directionNoun)
    {
        if (exitDictionary.ContainsKey(directionNoun))
        {
            currentRoom = exitDictionary[directionNoun];
            controller.LogStringWithReturn("Te dirijes hacia el " + directionNoun);
            controller.DisplayRoomText();
        }
        else
        {
            controller.LogStringWithReturn("No hay camino hacia el " + directionNoun);
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }

}

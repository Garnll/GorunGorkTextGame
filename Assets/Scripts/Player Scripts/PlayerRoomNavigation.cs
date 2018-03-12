using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomNavigation : MonoBehaviour {

    public GameController controller;
    public Room currentRoom;

    KeywordToStringConverter converter;

    Vector3 currentPosition;

    Dictionary<DirectionKeyword, Room> exitDictionary = new Dictionary<DirectionKeyword, Room>();

    public void AddExitsToController()
    {
        for (int i = 0; i < currentRoom.exits.Count; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].myKeyword, currentRoom.exits[i].conectedRoom);

            if (currentRoom.exits[i].exitDescription != "")
            {
                controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
            }
        }
    }

    public void AttemptToChangeRooms(DirectionKeyword directionNoun)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (exitDictionary.ContainsKey(directionNoun))
        {
            currentRoom = exitDictionary[directionNoun];
            controller.LogStringWithReturn("Te dirijes hacia el " + converter.ConvertFromKeyword(directionNoun));
            controller.DisplayRoomText();
        }
        else if (directionNoun != DirectionKeyword.unrecognized)
        {
            controller.LogStringWithReturn("No hay caminos hacia el " + converter.ConvertFromKeyword(directionNoun));
        }
        else
        {
            controller.LogStringWithReturn("Pensaste en una dirección dificil de alcanzar fisicamente...");
        }
    }

    public void AttemptToChangeRooms(string directionNoun)
    {
        controller.LogStringWithReturn("Pensaste en una dirección imposible...");
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }

}

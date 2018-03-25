using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El movimiento del jugador. Este se mueve unicamente entre habitaciones.
/// </summary>
public class PlayerRoomNavigation : MonoBehaviour {

    public GameController controller;
    public Room currentRoom;

    KeywordToStringConverter converter;

    Vector3 currentPosition;

    Dictionary<DirectionKeyword, Exit> exitDictionary = new Dictionary<DirectionKeyword, Exit>();

    /// <summary>
    /// Le envía al GameController las salidas de la habitación actual.
    /// </summary>
    public void AddExitsToController()
    {
        for (int i = 0; i < currentRoom.exits.Count; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].myKeyword, currentRoom.exits[i]);

            if (currentRoom.exits[i].exitDescription != "")
            {
                controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
            }
        }
    }


    public void TriggerRoomResponse()
    {
        if (currentRoom.roomResponse == null)
            return;

        currentRoom.roomResponse.TriggerResponse(controller);
    }

    /// <summary>
    /// Intenta moverse en la dirección dada. Si esta dirección no pertenece a una salida, no se mueve.
    /// </summary>
    /// <param name="directionNoun"></param>
    public void AttemptToChangeRooms(DirectionKeyword directionNoun)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (exitDictionary.ContainsKey(directionNoun))
        {
            Exit exitToGo = exitDictionary[directionNoun];

            if (exitToGo.exitActionDescription == "")
            {
                controller.LogStringWithReturn("Te dirijes hacia el " + converter.ConvertFromKeyword(directionNoun));
            }
            else
            {
                controller.LogStringWithReturn(exitToGo.exitActionDescription);
            }

            currentRoom = exitDictionary[directionNoun].conectedRoom;
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

    /// <summary>
    /// Da una respuesta erronea por no haber entrado correctamente a dirección
    /// </summary>
    /// <param name="directionNoun"></param>
    public void AttemptToChangeRooms(string directionNoun)
    {
        controller.LogStringWithReturn("Pensaste en una dirección imposible...");
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsChecker : MonoBehaviour {

    public static Dictionary<Vector3, RoomObject> roomsDictionary = new Dictionary<Vector3, RoomObject>();

	void Awake () {
        RoomSprite[] roomSprites = FindObjectsOfType<RoomSprite>();

        for (int i = 0; i < roomSprites.Length; i++)
        {
            roomsDictionary.Add(roomSprites[i].myRoom.roomPosition, roomSprites[i].myRoom);
        }
	}


    public static RoomObject RoomObjectFromVector (Vector3 roomPosition)
    {
        if (roomsDictionary.ContainsKey(roomPosition))
        {
            return roomsDictionary[roomPosition];
        }
        else
        {
            Debug.LogError("Jugador nuevo entrando en habitación no disponible, Vector: " + roomPosition.ToString());
            return null;
        }
    }

    public static Vector3 RoomPositionFromText(string vectorString)
    {
        string[] temp = vectorString.Substring(1, vectorString.Length - 2).Split(',');
        return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }
}

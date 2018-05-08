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

}

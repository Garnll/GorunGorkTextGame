using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (UNITY_EDITOR)
[ExecuteInEditMode]
#endif
public class RoomVisualMapper : MonoBehaviour {

    [SerializeField] private Transform mapParent;
    [SerializeField]private GameObject roomSprite;
    private Dictionary<Room, GameObject> roomImagesDictionary = new Dictionary<Room, GameObject>();

	public void PutRoomInPlace(Room roomToPut)
    {
        if (roomImagesDictionary.ContainsKey(roomToPut))
        {
            roomImagesDictionary[roomToPut].transform.Translate(roomToPut.roomPosition);
        }
        else
        {
            GameObject newRoomImage = Instantiate(roomSprite, roomToPut.roomPosition, Quaternion.identity, mapParent);

            newRoomImage.GetComponent<RoomSprite>().myRoom = roomToPut;
            roomImagesDictionary.Add(roomToPut, newRoomImage);
        }
    }
}

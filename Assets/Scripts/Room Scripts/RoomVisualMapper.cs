﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR)
[ExecuteInEditMode]
public class RoomVisualMapper : MonoBehaviour {

    public Transform map0Parent;
    public Transform map10Parent;
    public GameObject roomSprite;
    public GameObject roomExitCorner;
    public GameObject roomExitSides;

    private Dictionary<Room, GameObject> roomImagesDictionary = new Dictionary<Room, GameObject>();

    private void OnEnable()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        GameObject[] mappedRooms = GameObject.FindGameObjectsWithTag("RoomSprite");

        for (int i = 0; i < mappedRooms.Length; i++)
        {
            roomImagesDictionary.Add(mappedRooms[i].GetComponent<RoomSprite>().myRoom, mappedRooms[i]);
        }

        RoomSprite.OnDestroyed += EliminateRoomFromDictionary;
    }

    private void OnDisable()
    {
        roomImagesDictionary.Clear();
        RoomSprite.OnDestroyed -= EliminateRoomFromDictionary;
    }

    private void EliminateRoomFromDictionary(Room imageFromRoom)
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        if (roomImagesDictionary.ContainsKey(imageFromRoom))
        {
            roomImagesDictionary.Remove(imageFromRoom);
        }
    }

    public void PutRoomInPlace(Room roomToPut)
    {
        if (roomImagesDictionary.ContainsKey(roomToPut))
        {
            roomImagesDictionary[roomToPut].transform.SetPositionAndRotation(roomToPut.roomPosition, Quaternion.identity);
        }
        else
        {
            GameObject newRoomImage = PrefabUtility.InstantiatePrefab(roomSprite) as GameObject;
            newRoomImage.transform.position = roomToPut.roomPosition;
            newRoomImage.name = roomToPut.name + " Image";
            if (newRoomImage.transform.position.z == 0)
            {
                newRoomImage.transform.parent = map0Parent;
            }
            else if (newRoomImage.transform.position.z == 10)
            {
                newRoomImage.transform.parent = map10Parent;
            }

            newRoomImage.GetComponent<RoomSprite>().myRoom = roomToPut;
            roomImagesDictionary.Add(roomToPut, newRoomImage);
        }

        PutExitsInPlace(roomToPut);
    }

    private void PutExitsInPlace(Room roomToPutExits)
    {
        RoomSprite imageRoom = roomImagesDictionary[roomToPutExits].GetComponent<RoomSprite>();

        Transform[] childs = imageRoom.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < childs.Length; i++)
        {
            if (childs[i] != imageRoom.gameObject.transform)
            {
                DestroyImmediate(childs[i].gameObject);
            }
        }

        GameObject newExitImage = null;

        for (int i = 0; i < roomToPutExits.exits.Count; i++)
        {
            switch (roomToPutExits.exits[i].myKeyword)
            {
                default:
                    break;

                case DirectionKeyword.east:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitSides) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, 0.5f, 0f);
                    newExitImage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;

                case DirectionKeyword.north:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitSides) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, 0f, 0.5f);
                    break;

                case DirectionKeyword.south:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitSides) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, 0f, -0.5f);
                    break;
                case DirectionKeyword.west:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitSides) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, -0.5f, 0f);
                    newExitImage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;

                    
                case DirectionKeyword.northEast:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitCorner) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, 0.5f, 0.5f);
                    newExitImage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case DirectionKeyword.northWest:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitCorner) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, -0.5f, 0.5f);
                    break;
                case DirectionKeyword.southEast:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitCorner) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, 0.5f, -0.5f);
                    break;
                case DirectionKeyword.southWest:
                    newExitImage = PrefabUtility.InstantiatePrefab(roomExitCorner) as GameObject;
                    newExitImage.transform.position = ChangeVector3(roomToPutExits.roomPosition, -0.5f, -0.5f);
                    newExitImage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
            if (newExitImage != null)
            {
                newExitImage.name = imageRoom.gameObject.name + " Exit " +
                     KeywordToStringConverter.Instance.ConvertFromKeyword(roomToPutExits.exits[i].myKeyword);
                newExitImage.transform.parent = imageRoom.transform;
            }
        }
    }

    private Vector3 ChangeVector3(Vector3 original,float x, float y)
    {
        return new Vector3(original.x + x, original.y + y, original.z);
    }
}
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RoomEditionController : MonoBehaviour {
#if (UNITY_EDITOR)

    private bool isEventSuscribed = false;

    Dictionary<Vector3, Room> existingRooms = new Dictionary<Vector3, Room>();
    List<Room> roomsToLoad = new List<Room>();

    private void OnEnable()
    {
        if (Application.isPlaying)
            return;

        if (!isEventSuscribed)
        {
            Room.OnChangePosition += RoomPositionChanged;
            isEventSuscribed = true;
        }
        existingRooms.Clear();

        string[] path = new string[1];
        path[0] = "Assets/Scripts/ScriptableObjects/Rooms";

        var findRooms = AssetDatabase.FindAssets("t:Room", path);
        for (int i = 0; i < findRooms.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(findRooms[i]);
            Room newRoom = AssetDatabase.LoadAssetAtPath<Room>(assetPath);
            roomsToLoad.Add(newRoom);

            if (!existingRooms.ContainsKey(newRoom.roomPosition))
            {
                existingRooms.Add(newRoom.roomPosition, newRoom);
            }
            else
            {
                Debug.LogError("The room '" + newRoom.roomName + "' couldn't be added to Dictionary. Please change it's position");
            }
        }

        for (int i = 0; i < roomsToLoad.Count; i++)
        {
            Room loadRoom = RoomDataSave.LoadData(roomsToLoad[i]);
            Debug.Log(loadRoom);
            if (loadRoom != null)
            {
                roomsToLoad[i] = loadRoom;
                Debug.Log("Loaded Room " + loadRoom.name);
            }
        }
    }


    private void RoomPositionChanged(Room currentAnalizedRoom, Vector3 newRoomPosition)
    {
        if (Application.isPlaying)
            return;

        Vector3 oldRoomPosition = currentAnalizedRoom.roomPosition;

        if (existingRooms.ContainsKey(oldRoomPosition))
        {
            existingRooms.Remove(oldRoomPosition);
        }

        if (existingRooms.ContainsKey(newRoomPosition))
        {
            Debug.LogError("There's already a Room on position " + newRoomPosition.ToString());
            currentAnalizedRoom.roomPosition = oldRoomPosition;
        }
        else
        {
            currentAnalizedRoom.roomPosition = newRoomPosition;
            CheckForOtherRoomsInArea(currentAnalizedRoom);
        }

        existingRooms.Add(currentAnalizedRoom.roomPosition, currentAnalizedRoom);

        RoomDataSave.SaveData(currentAnalizedRoom);
    }

    private void CheckForOtherRoomsInArea(Room roomBeingAnalized)
    {
        Vector3 centerRoom = roomBeingAnalized.roomPosition;
        List<string> directions = new List<string>();
        List<Vector3> positions = new List<Vector3>();

        int posX;
        int posY;
        int posZ = (int)centerRoom.z;

        for (int i = -1; i < 2; i++)
        {
            posX = (int)centerRoom.x + i;

            for (int f = -1; f < 2; f++)
            {
                posY = (int)centerRoom.y + f;

                Vector3 positionToCheck = new Vector3(posX, posY, posZ);
                if (positionToCheck != roomBeingAnalized.roomPosition)
                {
                    if (existingRooms.ContainsKey(positionToCheck))
                    {
                        directions.Add(CheckOtherRoomDirection(centerRoom, positionToCheck));
                        positions.Add(positionToCheck);
                    }
                }
            }
        }

        if (directions.Count > 0)
        {
            CreateExits(roomBeingAnalized, directions.ToArray(), positions.ToArray());
            directions.Clear();
        }
        else
        {
            Debug.LogWarning("Couldn't Detect any Exits");
        }
    }

    private string CheckOtherRoomDirection(Vector3 roomPosition, Vector3 exitPosition)
    {
        string direction;

        if (exitPosition.x > roomPosition.x)
        {
            direction = "este";

            if (exitPosition.y > roomPosition.y)
            {
                direction = "noreste";
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = "sureste";
            }
        }
        else if (exitPosition.x < roomPosition.x)
        {
            direction = "oeste";

            if (exitPosition.y > roomPosition.y)
            {
                direction = "noroeste";
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = "suroeste";
            }
        }
        else
        {
            if (exitPosition.y > roomPosition.y)
            {
                direction = "norte";
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = "sur";
            }
            else
            {
                direction = "en todo el centro, esto no debería pasar";
            }
        }

        return direction;
    }

    private void CreateExits(Room currentlyExaminedRoom, string[] exitDirections, Vector3[] otherRoomsPositions)
    {
        currentlyExaminedRoom.exits.Clear();

        for (int i = 0; i < exitDirections.Length; i++)
        {
            Exit newExitToCreate = new Exit()
            {
                exitDescription = "Hay una salida al " + exitDirections[i],
                myKeyword = exitDirections[i],
                conectedRoom = existingRooms[otherRoomsPositions[i]]
            };

            currentlyExaminedRoom.exits.Add(newExitToCreate);
        }
    }

    private void OnDisable()
    {
        Room.OnChangePosition -= RoomPositionChanged;
        isEventSuscribed = false;
        existingRooms.Clear();
    }
#endif
}

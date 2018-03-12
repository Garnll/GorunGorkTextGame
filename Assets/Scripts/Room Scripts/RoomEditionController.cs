using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RoomEditionController : MonoBehaviour {
#if (UNITY_EDITOR)

    public static Dictionary<Vector3, Room> existingRooms = new Dictionary<Vector3, Room>();

    List<Room> roomsToLoad = new List<Room>();
    private bool isEventSuscribed = false;

    KeywordToStringConverter converter;

    private void OnEnable()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        if (!isEventSuscribed)
        {
            Room.OnChangePosition += RoomPositionChanged;
            Room.OnChangeStuff += SaveChanges;
            isEventSuscribed = true;
        }
        existingRooms.Clear();
        roomsToLoad.Clear();

        string[] path = new string[1];
        path[0] = "Assets/Scripts/_ScriptableObjects Assets/Rooms";

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
            RoomDataSaver.Room_Data loadRoom = RoomDataSaver.LoadData(roomsToLoad[i]);
            if (loadRoom != null)
            {
                LoadRoomParameters(roomsToLoad[i], loadRoom);
            }
        }
    }

    private void LoadRoomParameters(Room roomToChange, RoomDataSaver.Room_Data roomDataLoad)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (roomDataLoad != null)
        {
            roomToChange.roomPosition = roomDataLoad.roomPositionData;
            roomToChange.roomDescription = roomDataLoad.roomDescriptionData;
            roomToChange.roomName = roomDataLoad.roomNameData;
            roomToChange.exits.Clear();
            for (int i = 0; i < roomDataLoad.exitsData.Length; i++)
            {
                Exit loadExit = new Exit();

                loadExit.myKeyword = converter.ConvertFromString(roomDataLoad.exitsData[i].myKeywordData);
                loadExit.conectedRoom = existingRooms[roomDataLoad.exitsData[i].connectedRoomPosition];
                loadExit.exitDescription = roomDataLoad.exitsData[i].exitDescriptionData;

                roomToChange.exits.Add(loadExit);
            }
        }
    }

    private void RoomPositionChanged(Room currentAnalizedRoom, Vector3 newRoomPosition)
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
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

        SaveChanges(currentAnalizedRoom);
    }

    private void SaveChanges(Room currentAnalizedRoom)
    {
        RoomDataSaver.SaveData(currentAnalizedRoom);
        Debug.Log("Saved Room: " + currentAnalizedRoom.name.ToString());
    }

    private void CheckForOtherRoomsInArea(Room roomBeingAnalized)
    {
        Vector3 centerRoom = roomBeingAnalized.roomPosition;
        List<DirectionKeyword> directions = new List<DirectionKeyword>();
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

    private DirectionKeyword CheckOtherRoomDirection(Vector3 roomPosition, Vector3 exitPosition)
    {
        DirectionKeyword direction;

        if (exitPosition.x > roomPosition.x)
        {
            direction = DirectionKeyword.east;

            if (exitPosition.y > roomPosition.y)
            {
                direction = DirectionKeyword.northEast;
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = DirectionKeyword.southEast;
            }
        }
        else if (exitPosition.x < roomPosition.x)
        {
            direction = DirectionKeyword.west;

            if (exitPosition.y > roomPosition.y)
            {
                direction = DirectionKeyword.northWest;
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = DirectionKeyword.southWest;
            }
        }
        else
        {
            if (exitPosition.y > roomPosition.y)
            {
                direction = DirectionKeyword.north;
            }
            else if (exitPosition.y < roomPosition.y)
            {
                direction = DirectionKeyword.south;
            }
            else
            {
                direction = DirectionKeyword.unrecognized;
            }
        }

        return direction;
    }

    private void CreateExits(Room currentlyExaminedRoom, DirectionKeyword[] exitDirections, Vector3[] otherRoomsPositions)
    {
        currentlyExaminedRoom.exits.Clear();

        for (int i = 0; i < exitDirections.Length; i++)
        {
            Exit newExitToCreate = new Exit()
            {
                exitDescription = "Hay una salida al " + exitDirections[i] + ".",
                myKeyword = exitDirections[i],
                conectedRoom = existingRooms[otherRoomsPositions[i]]
            };

            currentlyExaminedRoom.exits.Add(newExitToCreate);
        }
    }

    private void OnDisable()
    {
        Room.OnChangePosition -= RoomPositionChanged;
        Room.OnChangeStuff -= SaveChanges;
        isEventSuscribed = false;
        existingRooms.Clear();
        roomsToLoad.Clear();
    }
#endif
}

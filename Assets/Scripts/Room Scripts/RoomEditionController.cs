using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Usado durante edicion. Recibe los eventos de las habitaciones para cambiarlas de lugar, detectar posiciones,
/// detectar salidas, etc. para hacer más facil la creación y mantenimiento de habitaciones.
/// </summary>
[ExecuteInEditMode]
public class RoomEditionController : MonoBehaviour {
#if (UNITY_EDITOR)

    public RoomVisualMapper mapper;

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
                Debug.LogError("The room '" + existingRooms[newRoom.roomPosition] + "' already Is there");
            }
        }

        for (int i = 0; i < roomsToLoad.Count; i++)
        {
            RoomDataSaver.LoadData(roomsToLoad[i]);
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
            if (currentAnalizedRoom.changeExits)
            {
                CheckForOtherRoomsInArea(currentAnalizedRoom);
            }
        }

        existingRooms.Add(currentAnalizedRoom.roomPosition, currentAnalizedRoom);

        SaveChanges(currentAnalizedRoom);
    }

    private void SaveChanges(Room currentAnalizedRoom)
    {
        RoomDataSaver.SaveData(currentAnalizedRoom);
        Debug.Log("Saved Room: " + currentAnalizedRoom.name.ToString());
        PutVisualRepresentation(currentAnalizedRoom);
    }

    private void PutVisualRepresentation(Room currentAnalizedRoom)
    {
        mapper.PutRoomInPlace(currentAnalizedRoom);
    }

    private void CheckForOtherRoomsInArea(Room roomBeingAnalized)
    {
        Vector3 centerRoom = roomBeingAnalized.roomPosition;
        List<DirectionKeyword> directions = new List<DirectionKeyword>();
        List<Vector3> positions = new List<Vector3>();

        List<Room> roomsToChangeExit = new List<Room>();

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
                        roomsToChangeExit.Add(existingRooms[positionToCheck]);
                        directions.Add(CheckOtherRoomDirection(centerRoom, positionToCheck));
                        positions.Add(positionToCheck);
                    }
                }
            }
        }

        if (directions.Count > 0)
        {
            CreateCurrentRoomExits(roomBeingAnalized, directions.ToArray(), positions.ToArray());
            ChangeOtherRoomExits(roomsToChangeExit.ToArray(), roomBeingAnalized, centerRoom);
            directions.Clear();
        }
        else
        {
            Debug.LogWarning("Couldn't Detect any Exits");
        }
    }

    /// <summary>
    /// Recibe una posición de inicio y una posición de salida, y devuelve la dirección pertinente.
    /// </summary>
    /// <param name="roomPosition"></param>
    /// <param name="exitPosition"></param>
    /// <returns></returns>
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

    private void CreateCurrentRoomExits(Room currentlyExaminedRoom, DirectionKeyword[] exitDirections, Vector3[] otherRoomsPositions)
    {
        currentlyExaminedRoom.exits.Clear();

        for (int i = 0; i < exitDirections.Length; i++)
        {
            if (existingRooms.ContainsKey(otherRoomsPositions[i]))
            {
                currentlyExaminedRoom.exits.Add(CreateExit(exitDirections[i], existingRooms[otherRoomsPositions[i]]));
            }
            else
            {
                currentlyExaminedRoom.exits.Add(CreateExit(exitDirections[i], null));
            }


        }
    }

    private Exit CreateExit(DirectionKeyword direction, Room myConnectedRoom)
    {
        Exit newExitToCreate = new Exit()
        {
            exitDescription = "Hay una salida al "  
            + KeywordToStringConverter.Instance.ConvertFromKeyword(direction) 
            + ".",
            myKeyword = direction,
            conectedRoom = myConnectedRoom
        };

        return newExitToCreate;
    }

    private void ChangeOtherRoomExits(Room[] otherRooms, Room currentAnalizedRoom, Vector3 centerRoom)
    {
        for (int i = 0; i < otherRooms.Length; i++)
        {
            if (otherRooms[i].exits.Count == 0)
            {
                otherRooms[i].exits.Add(CreateExit(
                    CheckOtherRoomDirection(otherRooms[i].roomPosition, centerRoom),
                    currentAnalizedRoom)
                    );
                continue;
            }

            for (int f = 0; f < otherRooms[i].exits.Count; f++)
            {
                if (otherRooms[i].exits[f].conectedRoom == currentAnalizedRoom)
                {
                    otherRooms[i].exits[f].myKeyword = CheckOtherRoomDirection(otherRooms[i].roomPosition,
                        centerRoom);
                    break;
                }
                else if (f >= otherRooms[i].exits.Count - 1)
                {
                    otherRooms[i].exits.Add(CreateExit(
                        CheckOtherRoomDirection(otherRooms[i].roomPosition, centerRoom),
                        currentAnalizedRoom)
                        );
                    break;
                }
            }
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

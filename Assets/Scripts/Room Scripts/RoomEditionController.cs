#if (UNITY_EDITOR)
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

/// <summary>
/// Usado durante edicion. Recibe los eventos de las habitaciones para cambiarlas de lugar, detectar posiciones,
/// detectar salidas, etc. para hacer más facil la creación y mantenimiento de habitaciones.
/// </summary>
[ExecuteInEditMode]
public class RoomEditionController : MonoBehaviour {


    public RoomVisualMapper mapper;

    public static Dictionary<Vector3, RoomObject> existingRooms = new Dictionary<Vector3, RoomObject>();

    [ShowInInspector]
    private Dictionary<Vector3, RoomObject> ShowSomeStaticVariableInTheInspector
    {
        get { return RoomEditionController.existingRooms; }
        //set { MyComponent.SomeStaticVariable = value; }
    }

    List<RoomObject> roomsToLoad = new List<RoomObject>();
    private bool isEventSuscribed = false;

    KeywordToStringConverter converter;

    private void OnEnable()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        if (!isEventSuscribed)
        {
            RoomObject.OnChangePosition += RoomPositionChanged;
            RoomObject.OnChangeStuff += SaveChanges;
            isEventSuscribed = true;
        }
        existingRooms.Clear();
        roomsToLoad.Clear();

        string[] path = new string[1];
        path[0] = "Assets/Scripts/_ScriptableObjects Assets/Rooms";

        var findRooms = AssetDatabase.FindAssets("t: RoomObject", path);    

        for (int i = 0; i < findRooms.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(findRooms[i]);
            RoomObject newRoom = AssetDatabase.LoadAssetAtPath(assetPath, typeof(RoomObject)) as RoomObject;
            roomsToLoad.Add(newRoom);

            if (!existingRooms.ContainsKey(newRoom.roomPosition))
            {
                existingRooms.Add(newRoom.roomPosition, newRoom);
            }
            else
            {
                Debug.LogError("Habitación '" + newRoom.roomName + "' no pudo añadirse al Diccionario. Por favor cambiar su posición.");
                Debug.LogError("La habitación '" + existingRooms[newRoom.roomPosition] + "' ya está ahi.");
            }
        }

        for (int i = 0; i < roomsToLoad.Count; i++)
        {
            RoomDataSaver.LoadData(roomsToLoad[i]);
        }
    }

    private void RoomPositionChanged(RoomObject currentAnalizedRoom, Vector3 newRoomPosition)
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
            Debug.LogError("Ya hay una habitación en la posición: " + newRoomPosition.ToString());
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

    private void SaveChanges(RoomObject currentAnalizedRoom)
    {
        RoomDataSaver.SaveData(currentAnalizedRoom);
        Debug.Log("Habitación salvada: " + currentAnalizedRoom.name.ToString());
        PutVisualRepresentation(currentAnalizedRoom);
    }

    private void PutVisualRepresentation(RoomObject currentAnalizedRoom)
    {
        mapper.PutRoomInPlace(currentAnalizedRoom);
    }

    private void CheckForOtherRoomsInArea(RoomObject roomBeingAnalized)
    {
        Vector3 centerRoom = roomBeingAnalized.roomPosition;
        List<DirectionKeyword> directions = new List<DirectionKeyword>();
        List<Vector3> positions = new List<Vector3>();

        List<RoomObject> roomsToChangeExit = new List<RoomObject>();

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
            Debug.LogWarning("No se encontraton salidas.");
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

    private void CreateCurrentRoomExits(RoomObject currentlyExaminedRoom, DirectionKeyword[] exitDirections, Vector3[] otherRoomsPositions)
    {
        //currentlyExaminedRoom.exits.Clear();

        for (int i = 0; i < exitDirections.Length; i++)
        {
            bool exitFound = false;

            for (int f = 0; f < currentlyExaminedRoom.exits.Count; f++)
            {
                if (currentlyExaminedRoom.exits[f].myKeyword == exitDirections[i])
                {
                    if (existingRooms.ContainsKey(otherRoomsPositions[i]))
                    {
                        currentlyExaminedRoom.exits[i].conectedRoom = existingRooms[otherRoomsPositions[i]];
                    }
                    else
                    {
                        currentlyExaminedRoom.exits[i].conectedRoom = null;
                    }
                    exitFound = true;
                    break;
                }
            }

            if (exitFound)
            {
                continue;
            }

            if (existingRooms.ContainsKey(otherRoomsPositions[i]))
            {
                currentlyExaminedRoom.exits.Add(CreateExit(exitDirections[i], existingRooms[otherRoomsPositions[i]]));
            }
            else
            {
                currentlyExaminedRoom.exits.Add(CreateExit(exitDirections[i], null));
            }
        }

        for (int i = 0; i < currentlyExaminedRoom.exits.Count; i++)
        {
            bool hasKeyword = false;
            for (int f = 0; f < exitDirections.Length; f++)
            {
                if (exitDirections[f] == currentlyExaminedRoom.exits[i].myKeyword)
                {
                    hasKeyword = true;
                    break;
                }
            }

            if (!hasKeyword)
            {
                currentlyExaminedRoom.exits[i].myKeyword = DirectionKeyword.unrecognized;
                currentlyExaminedRoom.exits[i].conectedRoom = null;
            }
        }
    }

    private Exit CreateExit(DirectionKeyword direction, RoomObject myConnectedRoom)
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

    private void ChangeOtherRoomExits(RoomObject[] otherRooms, RoomObject currentAnalizedRoom, Vector3 centerRoom)
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
        RoomObject.OnChangePosition -= RoomPositionChanged;
        RoomObject.OnChangeStuff -= SaveChanges;
        isEventSuscribed = false;
        existingRooms.Clear();
        roomsToLoad.Clear();
    }
}
#endif

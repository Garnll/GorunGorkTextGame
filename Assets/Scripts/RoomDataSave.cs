using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class RoomDataSave {
    
    public class Exit_Data
    {
        public string myKeywordData;
        public Vector3 connectedRoomPosition;
        public string exitDescriptionData;
    }

    public class Room_Data
    {
        public Vector3 roomPositionData;
        public string roomDescriptionData;
        public string roomNameData;
        public Exit_Data[] exitsData;

    }

	public static void SaveData(Room roomToSave)
    {
        Room_Data roomData = new Room_Data();
        roomData.roomPositionData = roomToSave.roomPosition;
        roomData.roomDescriptionData = roomToSave.roomDescription;
        roomData.roomNameData = roomToSave.roomName;
        roomData.exitsData = new Exit_Data[roomToSave.exits.Count];
        for (int i = 0; i < roomData.exitsData.Length; i++)
        {
            roomData.exitsData[i] = new Exit_Data();
            roomData.exitsData[i].myKeywordData = roomToSave.exits[i].myKeyword;
            roomData.exitsData[i].connectedRoomPosition = roomToSave.exits[i].conectedRoom.roomPosition;
            roomData.exitsData[i].exitDescriptionData = roomToSave.exits[i].exitDescription;
        }

        SaveGame.Save<Room_Data>(roomToSave.name, roomData, SaveGamePath.RoomDataPath);
    }

    public static Room_Data LoadData(Room roomToLoad)
    {
        Room_Data roomDataLoad = null;

        if (roomToLoad.name != null)
        {
            if (SaveGame.Exists(roomToLoad.name, SaveGamePath.RoomDataPath))
            {
                roomDataLoad = SaveGame.Load<Room_Data>(roomToLoad.name, SaveGamePath.RoomDataPath);
            }
        }

        return roomDataLoad;
    }
}

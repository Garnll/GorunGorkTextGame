using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class RoomDataSave {

	public static void SaveData(Room roomToSave)
    {
        SaveGame.Save<Room>(roomToSave.name, roomToSave, SaveGamePath.RoomDataPath);
    }

    public static Room LoadData(Room roomToLoad)
    {
        Room returnRoom = null;

        if (SaveGame.Exists(roomToLoad.name, SaveGamePath.RoomDataPath))
        {
            Debug.Log("Exists");
            roomToLoad = SaveGame.Load<Room>(roomToLoad.name, SaveGamePath.RoomDataPath);
        }

        return returnRoom;
    }
}

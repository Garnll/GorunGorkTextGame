using UnityEngine;
using BayatGames.SaveGameFree;

/// <summary>
/// Usado para la persistencia de datos de la habitación mientras el juego esté en desarrollo.
/// </summary>
public class RoomDataSaver {

    static KeywordToStringConverter converter;

    public class Exit_Data
    {
        public string myKeywordData;
        public Vector3 connectedRoomPosition;
        public string exitDescriptionData;
        public string exitActionDescriptionData;
    }

    public class Room_Data
    {
        public Vector3 roomPositionData;
        public string roomDescriptionData;
        public string roomNameData;
        public Exit_Data[] exitsData;

    }

    /// <summary>
    /// Salva todos los datos que se puedan cambiar de una habitación.
    /// </summary>
    /// <param name="roomToSave"></param>
	public static void SaveData(Room roomToSave)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;


        Room_Data roomData = new Room_Data();
        roomData.roomPositionData = roomToSave.roomPosition;
        roomData.roomDescriptionData = roomToSave.roomDescription;
        roomData.roomNameData = roomToSave.roomName;
        roomData.exitsData = new Exit_Data[roomToSave.exits.Count];
        if (roomToSave.exits.Count > 0)
        {
       
            for (int i = 0; i < roomData.exitsData.Length; i++)
            {
                roomData.exitsData[i] = new Exit_Data();
                roomData.exitsData[i].myKeywordData = converter.ConvertFromKeyword(roomToSave.exits[i].myKeyword);
                roomData.exitsData[i].connectedRoomPosition = roomToSave.exits[i].conectedRoom.roomPosition;
                roomData.exitsData[i].exitDescriptionData = roomToSave.exits[i].exitDescription;
                roomData.exitsData[i].exitActionDescriptionData = roomToSave.exits[i].exitActionDescription;
            }
        }
        SaveGame.Save<Room_Data>(roomToSave.name, roomData, SaveGamePath.RoomDataPath);
    }


    public static void LoadData(Room roomToLoad)
    {
        Room_Data roomDataLoad = null;

        if (roomToLoad.name != null)
        {
            if (SaveGame.Exists(roomToLoad.name, SaveGamePath.RoomDataPath))
            {
                if (converter == null)
                    converter = KeywordToStringConverter.Instance;

                roomDataLoad = SaveGame.Load<Room_Data>(roomToLoad.name, SaveGamePath.RoomDataPath);

                roomToLoad.roomPosition = roomDataLoad.roomPositionData;
                roomToLoad.roomDescription = roomDataLoad.roomDescriptionData;
                roomToLoad.roomName = roomDataLoad.roomNameData;
                if (roomDataLoad.exitsData.Length > 0)
                {
                    roomToLoad.exits.Clear();
                    for (int i = 0; i < roomDataLoad.exitsData.Length; i++)
                    {
                        Exit loadExit = new Exit();

                        loadExit.myKeyword = converter.ConvertFromString(roomDataLoad.exitsData[i].myKeywordData);
                        if (roomDataLoad.exitsData[i].connectedRoomPosition != null)
                        {
                            if (RoomEditionController.existingRooms.ContainsKey(roomDataLoad.exitsData[i].connectedRoomPosition))
                            {
                                loadExit.conectedRoom = RoomEditionController.existingRooms[roomDataLoad.exitsData[i].connectedRoomPosition];
                            }
                            else
                            {
                                Debug.LogWarning("Se está intentando acceder a una habitación no existente. Vector: " 
                                    + roomDataLoad.exitsData[i].connectedRoomPosition + ", Habitación: " +
                                    roomDataLoad.roomNameData);
                            }
                        }
                        loadExit.exitDescription = roomDataLoad.exitsData[i].exitDescriptionData;
                        loadExit.exitActionDescription = roomDataLoad.exitsData[i].exitActionDescriptionData;

                        roomToLoad.exits.Add(loadExit);
                    }
                }
            }
        }
    }
}

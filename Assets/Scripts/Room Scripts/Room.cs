using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Gorun Gork/Room")]
public class Room : SerializedScriptableObject {

    [HideInInspector] public Vector3 roomPosition = new Vector3(0,0,358);

    [TextArea] public string roomDescription;
    public string roomName = "template name";
    public RoomResponse roomResponse;
    public List<InteractableObject> interactableObjectsInRoom = new List<InteractableObject>();
    public List<Exit> exits = new List<Exit>();

    private List<InteractableObject> savedInteractableObjects = new List<InteractableObject>();



    public delegate void RoomChanges(Room thisRoom, Vector3 newPosition);
    public static event RoomChanges OnChangePosition;

    public delegate void DescriptionChanges(Room thisRoom);
    public static event DescriptionChanges OnChangeStuff;


    private void OnEnable()
    {
        RoomObjectSaver.OnSaveObjects += SaveMyObjects;
        RoomObjectSaver.OnLoadObjects += LoadMyObjects;
    }

    private void OnDisable()
    {
        RoomObjectSaver.OnSaveObjects -= SaveMyObjects;
        RoomObjectSaver.OnLoadObjects -= LoadMyObjects;
    }

    public void SaveMyObjects()
    {
        savedInteractableObjects.Clear();

        for (int i = 0; i < interactableObjectsInRoom.Count; i++)
        {
            savedInteractableObjects.Add(interactableObjectsInRoom[i]);
        }
    }

    public void LoadMyObjects()
    {
        interactableObjectsInRoom.Clear();

        for (int i = 0; i < savedInteractableObjects.Count; i++)
        {
            interactableObjectsInRoom.Add(savedInteractableObjects[i]);
        }
    }


    /// <summary>
    /// Usado en edición. No se debería usar para cosas dentro del juego.
    /// </summary>
    /// <param name="newRoomPosition"></param>
    public void ChangePosition(Vector3 newRoomPosition)
    {
        if (OnChangePosition != null)
        {
            OnChangePosition(this, newRoomPosition);
        }
        else
        {
            Debug.LogWarning("Evento OnChangePosition no está funcionando");
        }
    }

    /// <summary>
    /// Usado en edición, no debería tocarse dentro del juego.
    /// </summary>
    public void ChangeStuff()
    {
        if (OnChangeStuff != null)
        {
            OnChangeStuff(this);
        }
        else
        {
            Debug.LogWarning("Evento OnChangeStuff no está funcionando");
        }
    }

    

}

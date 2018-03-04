using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Room")]
public class Room : ScriptableObject {

    [HideInInspector] public Vector3 roomPosition = new Vector3(0,0,358);

    [TextArea] public string roomDescription;
    public string roomName;
    //public List<InteractableObject> interactableObjectsInRoom = new List<InteractableObject>();
    public List<InteractableObjectsForRoom> interactableObjectsInRoom = new List<InteractableObjectsForRoom>();
    public List<Exit> exits = new List<Exit>();

    //test
    [System.Serializable]
    public class InteractableObjectsForRoom
    {
        public InteractableObject interactableObject;
        public bool isTaken = false;
    }


    public delegate void RoomChanges(Room thisRoom, Vector3 newPosition);
    public static event RoomChanges OnChangePosition;

    public delegate void DescriptionChanges(Room thisRoom);
    public static event DescriptionChanges OnChangeStuff;

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

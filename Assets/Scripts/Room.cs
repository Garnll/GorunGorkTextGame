using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Room")]
public class Room : ScriptableObject {

    [HideInInspector] public Vector3 roomPosition;

    [TextArea] public string roomDescription;
    public string roomName;
    public Exit[] exits;
    

    public delegate void RoomChanges(Room thisRoom);
    public static event RoomChanges OnChangePosition;

    public void ChangePosition(Vector3 newRoomPosition)
    {
        roomPosition = newRoomPosition;

        if (OnChangePosition != null)
        {
            OnChangePosition(this);
        }
        else
        {
            Debug.LogWarning("Evento OnChangePosition no está funcionando");
        }
    }

    

}

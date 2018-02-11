using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Room")]
public class Room : ScriptableObject {

    public Vector3 roomPosition;
    public string roomDescription;
    public string roomName;

    public void ChangePosition(Vector3 newRoomPosition)
    {
        roomPosition = newRoomPosition;
        Debug.Log("Done");
    }

    

}

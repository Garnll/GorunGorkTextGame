using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Room))]
public class RoomCustonEditor : Editor {

    public Vector3Int newPosition = new Vector3Int();
    Room targetRoom;

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        targetRoom = (Room)target;
        newPosition = Vector3Int.CeilToInt(targetRoom.roomPosition);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Vector3Field("Room Position", targetRoom.roomPosition);

        base.OnInspectorGUI();

        GUILayout.Space(10);

        newPosition = EditorGUILayout.Vector3IntField("New Position", newPosition);

        ///Changes and saves the new position of the room.
        ///Detects other rooms in the area and creates exits.
        ///However, deletes existing exits.
        if (GUILayout.Button("Change Position / Detect Exits"))
        {
            targetRoom.ChangePosition(newPosition);
        }

        GUILayout.Space(20);

        ///Saves descriptions, connected rooms, keywords, etc.
        ///Does not change the room position.
        if (GUILayout.Button("Save Other Changes"))
        {
            targetRoom.ChangeStuff();
        }
    }
}

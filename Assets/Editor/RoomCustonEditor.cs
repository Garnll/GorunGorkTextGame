using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Room))]
public class RoomCustonEditor : Editor {

    public Vector3Int newPosition = new Vector3Int();

    public override void OnInspectorGUI()
    {
        Room targetRoom = (Room)target;

        EditorGUILayout.Vector3Field("Room Position", targetRoom.roomPosition);

        base.OnInspectorGUI();

        newPosition = EditorGUILayout.Vector3IntField("New Position", newPosition);

        if (GUILayout.Button("Change Position"))
        {
            targetRoom.ChangePosition(newPosition);
        }
    }
}

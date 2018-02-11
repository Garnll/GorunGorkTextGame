using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Room))]
public class RoomCustonEditor : Editor {

    public Vector3 newPosition = new Vector3();

    public override void OnInspectorGUI()
    {
        Room targetRoom = (Room)target;

        base.OnInspectorGUI();

        newPosition = EditorGUILayout.Vector3Field("New Position", newPosition);

        if (GUILayout.Button("Change Position"))
        {
            targetRoom.ChangePosition(newPosition);
        }
    }
}

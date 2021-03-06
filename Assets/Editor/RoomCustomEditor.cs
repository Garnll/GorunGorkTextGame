﻿using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

/// <summary>
/// Clase responsable del inspector customizado de as habitaciones.
/// </summary>
[CustomEditor(typeof(RoomObject))]
public class RoomCustomEditor : OdinEditor {

    public Vector3Int newPosition = new Vector3Int();
    RoomObject targetRoom;

    protected override void OnEnable()
    {
        targetRoom = (RoomObject)target;
        newPosition = Vector3Int.CeilToInt(targetRoom.roomPosition);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("RoomObject Position:");
        EditorGUILayout.LabelField("X: " + targetRoom.roomPosition.x 
            + "\tY: " + targetRoom.roomPosition.y
            + "\tZ: " + targetRoom.roomPosition.z);

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

        targetRoom.changeExits = GUILayout.Toggle(targetRoom.changeExits, "Change Exits?");

        GUILayout.Space(20);

        ///Saves descriptions, connected rooms, keywords, etc.
        ///Does not change the room position.
        if (GUILayout.Button("Save Other Changes"))
        {
            targetRoom.ChangeStuff();
        }


        GUILayout.Space(50);

        ///Saves descriptions, connected rooms, keywords, etc.
        ///Does not change the room position.
        if (GUILayout.Button("DELETE ROOM"))
        {
            if (EditorUtility.DisplayDialog("Deleting Room", "Are you sure you want to delete " + targetRoom.roomName + "?",
                "Yes", "No"))
            {
                targetRoom.DeleteRoomAsset();
            }
        }
    }
}

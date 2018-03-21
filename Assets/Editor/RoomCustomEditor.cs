using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

/// <summary>
/// Clase responsable del inspector customizado de as habitaciones.
/// </summary>
[CustomEditor(typeof(Room))]
public class RoomCustomEditor : OdinEditor {

    public Vector3Int newPosition = new Vector3Int();
    Room targetRoom;

    protected override void OnEnable()
    {
        targetRoom = (Room)target;
        newPosition = Vector3Int.CeilToInt(targetRoom.roomPosition);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Room Position:");
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

        GUILayout.Space(20);

        ///Saves descriptions, connected rooms, keywords, etc.
        ///Does not change the room position.
        if (GUILayout.Button("Save Other Changes"))
        {
            targetRoom.ChangeStuff();
        }
    }
}

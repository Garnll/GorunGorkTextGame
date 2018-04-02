using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextColorCoding))]
public class TextColorCodingCustomEditor : Editor
{
    Color[] colorsInTarget;
    TextColorCoding targetAsset;

    protected void OnEnable()
    {
        targetAsset = (TextColorCoding)target;
    }

    public override void OnInspectorGUI()
    {
        colorsInTarget = targetAsset.GetColors();

        DrawDefaultInspector();
        GUILayout.Space(10);

        for (int i = 0; i < colorsInTarget.Length; i++)
        {
            EditorGUILayout.TextField("Color " + (i + 1) + ":", "#" + ColorUtility.ToHtmlStringRGB(colorsInTarget[i]));
            GUILayout.Space(1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomEditionController : MonoBehaviour {
#if (UNITY_EDITOR)

    private bool isEventSuscribed = false;

    private void Awake()
    {
        if (Application.isPlaying)
            return;

        if (!isEventSuscribed)
        {
            Room.OnChangePosition += RoomPositionChanged;
            isEventSuscribed = true;
        }
    }


    private void RoomPositionChanged(Room currentAnalizedRoom)
    {
        if (Application.isPlaying)
            return;

        Debug.Log("It's working. It's working!");
    }

    private void OnDisable()
    {
        Room.OnChangePosition -= RoomPositionChanged;
        isEventSuscribed = false;
    }
#endif
}

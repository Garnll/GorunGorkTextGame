using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomSprite : MonoBehaviour {

    [HideInInspector] public Room myRoom;

    [HideInInspector] public List<GameObject> myExits = new List<GameObject>();

    public delegate void DestroyEvent(Room thisRoom);
    public static event DestroyEvent OnDestroyed;

    private void OnDestroy()
    {
        if (OnDestroyed != null)
        {
            OnDestroyed(myRoom);
        }
    }

}

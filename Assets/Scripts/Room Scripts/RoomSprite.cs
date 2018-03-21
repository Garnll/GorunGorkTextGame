using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contenido por el prefab de la visualización de las habitaciones. Indica la habitación correspondiente
/// y las salidas de esta en forma de GameObject.
/// </summary>
[ExecuteInEditMode]
public class RoomSprite : MonoBehaviour {

    [HideInInspector] public Room myRoom;

    [HideInInspector] public List<GameObject> myExits = new List<GameObject>();

    ///Enviado a RoomVisuaMapper en caso de que el GameObject sea destruido.
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

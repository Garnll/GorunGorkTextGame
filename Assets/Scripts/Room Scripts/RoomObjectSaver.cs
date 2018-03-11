using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectSaver : MonoBehaviour {


    /// <summary>
    /// Enviados específicamente a Room, para que puedan activar sus eventos de salvar y cargar los objetos.
    /// </summary>
    public delegate void RoomSaving();
    public static event RoomSaving OnSaveObjects;
    public static event RoomSaving OnLoadObjects;

    private void Awake()
    {
        OnSaveObjects();
    }

    private void OnDestroy()
    {
        OnLoadObjects();
    }

}

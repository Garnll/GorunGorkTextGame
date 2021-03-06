﻿using UnityEngine;

/// <summary>
/// Envia eventos a las habitaciones para que salven o carguen sus objetos.
/// </summary>
public class RoomObjectSaver : MonoBehaviour {

    /// <summary>
    /// Enviados específicamente a RoomObject, para que puedan activar sus eventos de salvar y cargar los objetos.
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

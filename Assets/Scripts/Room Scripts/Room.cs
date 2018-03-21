﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Habitación, o lugar donde puede estar un jugador. Contiene su posición, descripción, objetos, etc.
/// Esta es de las clases más importantes del juego, pues es aqui donde se desarrollan casi todos los 
/// posibles eventos.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Room")]
public class Room : SerializedScriptableObject {

    [HideInInspector] public Vector3 roomPosition = new Vector3(0,0,358);

    [TextArea] public string roomDescription;
    public string roomName = "template name";
    public RoomResponse roomResponse;
    public List<InteractableObject> interactableObjectsInRoom = new List<InteractableObject>();
    public List<Exit> exits = new List<Exit>();

    private List<InteractableObject> savedInteractableObjects = new List<InteractableObject>();


    //Eventos enviados a RoomEditionController, para poder salvar, cambiar posiciones, etc.
    public delegate void RoomChanges(Room thisRoom, Vector3 newPosition);
    public static event RoomChanges OnChangePosition;

    public delegate void DescriptionChanges(Room thisRoom);
    public static event DescriptionChanges OnChangeStuff;


    private void OnEnable()
    {
        RoomObjectSaver.OnSaveObjects += SaveMyObjects;
        RoomObjectSaver.OnLoadObjects += LoadMyObjects;
    }

    private void OnDisable()
    {
        RoomObjectSaver.OnSaveObjects -= SaveMyObjects;
        RoomObjectSaver.OnLoadObjects -= LoadMyObjects;
    }

    /// <summary>
    /// Salva los objetos al inicio del juego.
    /// </summary>
    public void SaveMyObjects()
    {
        savedInteractableObjects.Clear();

        for (int i = 0; i < interactableObjectsInRoom.Count; i++)
        {
            savedInteractableObjects.Add(interactableObjectsInRoom[i]);
        }
    }

    /// <summary>
    /// Carga los objetos que se salvaron al inicio del juego para no perderlos.
    /// </summary>
    public void LoadMyObjects()
    {
        interactableObjectsInRoom.Clear();

        for (int i = 0; i < savedInteractableObjects.Count; i++)
        {
            interactableObjectsInRoom.Add(savedInteractableObjects[i]);
        }
    }


    /// <summary>
    /// Cambia la posición de la habitación.
    /// Sobreescribe salidas existentes.
    /// Usado en edición. No se debería usar para cosas dentro del juego.
    /// </summary>
    /// <param name="newRoomPosition"></param>
    public void ChangePosition(Vector3 newRoomPosition)
    {
        if (OnChangePosition != null)
        {
            OnChangePosition(this, newRoomPosition);
        }
        else
        {
            Debug.LogWarning("Evento OnChangePosition no está funcionando");
        }
    }

    /// <summary>
    /// Guarda todos los cambios hechos.
    /// Usado en edición, no debería tocarse dentro del juego.
    /// </summary>
    public void ChangeStuff()
    {
        if (OnChangeStuff != null)
        {
            OnChangeStuff(this);
        }
        else
        {
            Debug.LogWarning("Evento OnChangeStuff no está funcionando");
        }
    }

    

}

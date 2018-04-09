using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla el comportamieto de los objetos interactuables.
/// </summary>
public class ItemHandler : MonoBehaviour {

    public InventoryManager inventoryManager;
    [SerializeField] private ItemKeywordHandler itemKeywordHandler;

    private GameController controller;

    private void Start()
    {
        controller = GetComponent<GameController>();
    }

    /// <summary>
    /// Busca objetos, sea ya en la habitación, en el inventario, o en ambos (o en ninguno).
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="searchInRoom"></param>
    /// <param name="searchInInventory"></param>
    /// <returns></returns>
    public InteractableObject SearchObjectInRoomOrInventory(string[] objectName, bool searchInRoom, bool searchInInventory)
    {
        string noun = string.Join(" ", SeparateKeyWords(objectName));

        InteractableObject[] objectsFound = itemKeywordHandler.GetObjectWithNoun(noun);

        if (objectsFound == null)
        {
            controller.LogStringWithReturn("No entiendes cómo " + objectName[0] + " un \"" + noun + "\".");
            return null;
        }


        InteractableObject objectToInteract = null;
        if (searchInRoom)
        {
            objectToInteract = SearchObjectInRoom(objectsFound);

            if (objectToInteract == null && !searchInInventory)
            {
                string objectToDisplay = noun;

                if (objectsFound[0].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "un " + noun;
                else
                    objectToDisplay = "una " + noun;

                controller.LogStringWithReturn("No hay " + objectToDisplay + " en este lugar.");
            }
        }

        if (searchInInventory && objectToInteract == null)
        {
            objectToInteract = SearchObjectInInventory(objectsFound);

            if (objectToInteract == null && !searchInRoom)
            {
                string objectToDisplay = noun;

                if (objectsFound[0].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "un " + noun;
                else
                    objectToDisplay = "una " + noun;

                controller.LogStringWithReturn("No hay " + objectToDisplay + " en tu inventario.");
            }
        }

        if (objectToInteract == null)
        {
            string objectToDisplay = noun;

            if (objectsFound[0].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "un " + noun;
            else
                objectToDisplay = "una " + noun;

            controller.LogStringWithReturn("No hay " + objectToDisplay + " por acá.");
        }


        return objectToInteract;

    }


    /// <summary>
    /// Busca la existencia de un objeto dado en la habitación.
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    private InteractableObject SearchObjectInRoom(InteractableObject[] objectsFound)
    {
        InteractableObject objectToInteract = null;
        for (int i = 0; i < objectsFound.Length; i++)
        {
            for (int f = 0; f < controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Count; f++)
            {
                if (objectsFound[i].nouns == 
                    controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom[f].interactableObject.nouns)
                {
                    objectToInteract = controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom[f].interactableObject;
                    break;
                }
            }

            if (objectToInteract != null)
            {
                break;
            }
        }
        
        return objectToInteract;

    }

    /// <summary>
    /// Busca la existencia de un objeto dado en el inventario.
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    private InteractableObject SearchObjectInInventory(InteractableObject[] objectsFound)
    {
        InteractableObject objectToInteract = null;

        for (int i = 0; i < objectsFound.Length; i++)
        {
            for (int f = 0; f < inventoryManager.nounsInInventory.Count; f++)
            {
                if (objectsFound[i].nouns == inventoryManager.nounsInInventory[f].nouns)
                {
                    objectToInteract = inventoryManager.nounsInInventory[f];
                    break;
                }
            }

            if (objectToInteract != null)
            {
                break;
            }
        }

        return objectToInteract;

    }

    //A partir de aqui, se ponen las funciones que realizan acciones.

    /// <summary>
    /// Muestra el inventario en el display principal.
    /// </summary>
    public void DisplayInventoryByCommand()
    {
        List<string> combinedText = new List<string>();

        combinedText.Add("Miras en tu bolsillo e, increiblemente, ves: ");
        string objectToDisplay = "";

        if (inventoryManager.nounsInInventory.Count == 0)
        {
            combinedText.Add("-Nada");
        }

        for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++)
        {
            objectToDisplay = inventoryManager.nounsInInventory[i].nouns[0];

            if (inventoryManager.nounsInInventory[i].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "-Un " + inventoryManager.nounsInInventory[i].nouns[0];
            else
                objectToDisplay = "-Una " + inventoryManager.nounsInInventory[i].nouns[0];

            combinedText.Add(objectToDisplay);
        }

        controller.LogStringWithReturn(string.Join("\n", combinedText.ToArray()));
    }

    public void ExamineObject(InteractableObject objectToExamine)
    {

        Interaction examineInteraction = null;

        for (int i = 0; i < objectToExamine.interactions.Length; i++)
        {
            if (objectToExamine.interactions[i].inputAction.GetType() == typeof(ExamineInput))
            {
                examineInteraction = objectToExamine.interactions[i];
                break;
            }
        }

        if (examineInteraction == null)
        {
            string objectToDisplay = objectToExamine.nouns[0];

            if (objectToExamine.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToExamine.nouns[0];
            else
                objectToDisplay = "la " + objectToExamine.nouns[0];

            controller.LogStringWithReturn("No logras examinar " + objectToDisplay + ".");
            return;
        }

        controller.LogStringWithReturn(examineInteraction.textResponse);
    }

    public void TakeObject(InteractableObject objectToTake)
    {
        Interaction takeInteraction = null;

        for (int i = 0; i < objectToTake.interactions.Length; i++)
        {
            if (objectToTake.interactions[i].inputAction.GetType() == typeof(TakeInput))
            {
                takeInteraction = objectToTake.interactions[i];
                break;
            }
        }

        if (takeInteraction == null)
        {
            string objectToDisplay = objectToTake.nouns[0];

            if (objectToTake.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToTake.nouns[0];
            else
                objectToDisplay = "la " + objectToTake.nouns[0];

            controller.LogStringWithReturn("No se puede coger " + objectToDisplay + ".");
            return;
        }

        if (takeInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(takeInteraction.textResponse);
            return;
        }

        inventoryManager.nounsInInventory.Add(objectToTake);

        for (int i = 0; i < controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Count; i++)
        {
            if (controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom[i].interactableObject ==
                objectToTake)
            {
                controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.RemoveAt(i);
                break;
            }
        }

        inventoryManager.DisplayInventory();

        controller.LogStringWithReturn(takeInteraction.textResponse);
    }

    public void ThrowObject(InteractableObject objectToThrow)
    {
        Interaction throwInteraction = null;

        for (int i = 0; i < objectToThrow.interactions.Length; i++)
        {
            if (objectToThrow.interactions[i].inputAction.GetType() == typeof(ThrowInput))
            {
                throwInteraction = objectToThrow.interactions[i];
                break;
            }
        }

        if (throwInteraction == null)
        {
            string objectToDisplay = objectToThrow.nouns[0];

            if (objectToThrow.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToThrow.nouns[0];
            else
                objectToDisplay = "la " + objectToThrow.nouns[0];

            controller.LogStringWithReturn("No se puede tirar " + objectToDisplay + ".");
            return;
        }

        if (throwInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(throwInteraction.textResponse);
            return;
        }

        Room.RoomVisibleObjects newObjectInRoom = new Room.RoomVisibleObjects();
        newObjectInRoom.interactableObject = objectToThrow;
        newObjectInRoom.visionRange = (int)Random.Range(-4,4);

        controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Add(newObjectInRoom);
        inventoryManager.nounsInInventory.Remove(objectToThrow);

        inventoryManager.DisplayInventory();

        controller.LogStringWithReturn(throwInteraction.textResponse);
    }

    public void UseObject(InteractableObject objectToUse)
    {
        Interaction useInteraction = null;

        InteractableConsumableObject consumable = null;

        for (int i = 0; i < objectToUse.interactions.Length; i++)
        {
            if (objectToUse.interactions[i].inputAction.GetType() == typeof(UseInput))
            {
                useInteraction = objectToUse.interactions[i];

                if (objectToUse.GetType() == typeof(InteractableConsumableObject))
                {
                    consumable = objectToUse as InteractableConsumableObject;
                }
                break;
            }
        }

        if (useInteraction == null)
        {
            string objectToDisplay = objectToUse.nouns[0];

            if (objectToUse.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToUse.nouns[0];
            else
                objectToDisplay = "la " + objectToUse.nouns[0];

            controller.LogStringWithReturn("No se puede usar " + objectToDisplay + ".");
            return;
        }

        if (useInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(useInteraction.textResponse);
            return;
        }

        controller.LogStringWithReturn(useInteraction.textResponse);
        bool action = useInteraction.actionResponse.DoActionResponse(controller);
        if (action)
        {
            consumable.UseObject();
            controller.LogStringWithReturn(useInteraction.actionResponse.responseDescription);

            if (!consumable.IsUseful)
            {
                consumable.StopWorking(controller);
                inventoryManager.nounsInInventory.Remove(consumable);

                inventoryManager.DisplayInventory();
            }
        }
        else
        {
            controller.LogStringWithReturn(useInteraction.actionResponse.negationDescription);
        }
    }

    /// <summary>
    /// Separa las palabras enviadas desde el input, dejando el input a un lado,
    /// y el nombre del objeto se devuelve para ser usado.
    /// </summary>
    /// <param name="completeInput"></param>
    /// <returns></returns>
    private string[] SeparateKeyWords(string[] completeInput)
    {
        string[] newString = new string[completeInput.Length - 1];

        for (int i = 1; i < completeInput.Length; i++)
        {
            newString[i - 1] = completeInput[i];
        }

        return newString;
    }
}

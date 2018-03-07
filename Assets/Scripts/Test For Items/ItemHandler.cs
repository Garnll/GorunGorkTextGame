using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour {

    public InventoryManager inventoryManager;
    [SerializeField] private ItemKeywordHandler itemKeywordHandler;

    private GameController controller;

    private void Start()
    {
        controller = GetComponent<GameController>();
    }

    /// <summary>
    /// Busca la existencia de un objeto con el keyword dado en la habitación.
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public InteractableObject SearchObjectInRoom(string[] objectName)
    {
        string noun = string.Join(" ", SeparateWords(objectName));

        InteractableObject[] objectsFound = itemKeywordHandler.GetObjectWithNoun(noun);

        if (objectsFound == null)
        {
            controller.LogStringWithReturn("No sabes cómo " +objectName[0] + " un \"" + noun + "\".");
            return null;
        }

        InteractableObject objectToInteract = null;
        for (int i = 0; i < objectsFound.Length; i++)
        {
            for (int f = 0; f < controller.playerRoomNavigation.currentRoom.interactableObjectsInRoom.Count; f++)
            {
                if (objectsFound[i].noun == controller.playerRoomNavigation.currentRoom.interactableObjectsInRoom[f].noun)
                {
                    objectToInteract = controller.playerRoomNavigation.currentRoom.interactableObjectsInRoom[f];
                    break;
                }
            }

            if (objectToInteract != null)
            {
                break;
            }
        }

        if (objectToInteract == null)
        {
            string objectToDisplay = noun;

            if (objectsFound[0].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "un " + noun;
            else
                objectToDisplay = "una " + noun;

            controller.LogStringWithReturn("No hay " + objectToDisplay + " en este lugar.");
        }
        

        return objectToInteract;

    }

    /// <summary>
    /// Busca la existencia de un objeto con el keyword dado en el inventario.
    /// </summary>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public InteractableObject SearchObjectInInventory(string[] objectName)
    {
        string noun = string.Join(" ", SeparateWords(objectName));

        InteractableObject[] objectsFound = itemKeywordHandler.GetObjectWithNoun(noun);

        if (objectsFound == null)
        {
            controller.LogStringWithReturn("No ves cómo podrías " + objectName[0] + " un \"" + noun + "\".");
            return null;
        }

        InteractableObject objectToInteract = null;

        for (int i = 0; i < objectsFound.Length; i++)
        {
            for (int f = 0; f < inventoryManager.nounsInInventory.Count; f++)
            {
                if (objectsFound[i].noun == inventoryManager.nounsInInventory[f].noun)
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

        if (objectToInteract == null)
        {
            string objectToDisplay = noun;

            if (objectsFound[0].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "un " + noun;
            else
                objectToDisplay = "una " + noun;

            controller.LogStringWithReturn("No hay " + objectToDisplay + " en tu inventario.");
        }

        return objectToInteract;

    }

    //A partir de aqui, se ponen las funciones que realizan acciones.

    public void ExamineObject(InteractableObject objectToExamine)
    {

        Interaction examineInteraction = null;

        for (int i = 0; i < objectToExamine.interactions.Length; i++)
        {
            if (objectToExamine.interactions[i].inputAction.GetType() == typeof(Examine))
            {
                examineInteraction = objectToExamine.interactions[i];
                break;
            }
        }

        if (examineInteraction == null)
        {
            string objectToDisplay = objectToExamine.noun;

            if (objectToExamine.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToExamine.noun;
            else
                objectToDisplay = "la " + objectToExamine.noun;

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
            if (objectToTake.interactions[i].inputAction.GetType() == typeof(Take))
            {
                takeInteraction = objectToTake.interactions[i];
                break;
            }
        }

        if (takeInteraction == null)
        {
            string objectToDisplay = objectToTake.noun;

            if (objectToTake.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToTake.noun;
            else
                objectToDisplay = "la " + objectToTake.noun;

            controller.LogStringWithReturn("No se puede coger " + objectToDisplay + ".");
            return;
        }

        if (takeInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(takeInteraction.textResponse);
            return;
        }

        inventoryManager.nounsInInventory.Add(objectToTake);
        controller.playerRoomNavigation.currentRoom.interactableObjectsInRoom.Remove(objectToTake);

        inventoryManager.DisplayInventory();

        controller.LogStringWithReturn(takeInteraction.textResponse);
    }

    public void ThrowObject(InteractableObject objectToThrow)
    {
        Interaction throwInteraction = null;

        for (int i = 0; i < objectToThrow.interactions.Length; i++)
        {
            if (objectToThrow.interactions[i].inputAction.GetType() == typeof(Throw))
            {
                throwInteraction = objectToThrow.interactions[i];
                break;
            }
        }

        if (throwInteraction == null)
        {
            string objectToDisplay = objectToThrow.noun;

            if (objectToThrow.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToThrow.noun;
            else
                objectToDisplay = "la " + objectToThrow.noun;

            controller.LogStringWithReturn("No se puede tirar " + objectToDisplay + ".");
            return;
        }

        if (throwInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(throwInteraction.textResponse);
            return;
        }

        controller.playerRoomNavigation.currentRoom.interactableObjectsInRoom.Add(objectToThrow);
        inventoryManager.nounsInInventory.Remove(objectToThrow);

        inventoryManager.DisplayInventory();

        controller.LogStringWithReturn(throwInteraction.textResponse);
    }

    /// <summary>
    /// Separa las palabras enviadas desde el input, dejando el input a un lado, y el nombre del objeto se devuelve para ser usado.
    /// </summary>
    /// <param name="completeInput"></param>
    /// <returns></returns>
    private string[] SeparateWords(string[] completeInput)
    {
        string[] newString = new string[completeInput.Length - 1];

        for (int i = 1; i < completeInput.Length; i++)
        {
            newString[i - 1] = completeInput[i];
        }

        return newString;
    }
}

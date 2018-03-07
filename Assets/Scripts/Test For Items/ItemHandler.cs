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

    public InteractableObject SearchObjectInInventory(string[] objectName)
    {
        string noun = string.Join(" ", SeparateWords(objectName));

        InteractableObject[] objectsFound = itemKeywordHandler.GetObjectWithNoun(noun);

        if (objectsFound == null)
        {
            controller.LogStringWithReturn("No ves cómo " + objectName[0] + " un \"" + noun + "\".");
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

    public void TakeObject(InteractableObject objectToTake)
    {
        Interaction takeInteraction = null;

        for (int i = 0; i < objectToTake.interactions.Length; i++)
        {
            Debug.Log(objectToTake.interactions[i].inputAction.GetType());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour {

    public List<InteractableObject> usableItemList;

    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> throwDictionary = new Dictionary<string, string>();

    [HideInInspector] public List<string> nounsInRoom = new List<string>();
    [HideInInspector] public Dictionary<string, InteractableObject> objectsInRoomDictionary = new Dictionary<string, InteractableObject>();
    [HideInInspector] public Dictionary<string, Interaction> inverseNouns = new Dictionary<string, Interaction>();

    Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    List<string> nounsInInventory = new List<string>();
    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public string GetObjectNotInInventory (Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

        if (!nounsInInventory.Contains (interactableInRoom.noun))
        {
            nounsInRoom.Add(interactableInRoom.noun);
            objectsInRoomDictionary.Add(interactableInRoom.noun, interactableInRoom);

            return interactableInRoom.description;
        }

        return null;
    }

    public void AddActionResponsesToUseDictionary()
    {
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            string noun = nounsInInventory[i];

            InteractableObject interactableObjectInInventory = GetInteractableObjectFromUsableList(noun);
            if (interactableObjectInInventory == null)
                continue;

            for (int j = 0; j < interactableObjectInInventory.interactions.Length; j++)
            {
                Interaction interaction = interactableObjectInInventory.interactions[j];

                if (interaction.actionResponse == null)
                    continue;

                if (!useDictionary.ContainsKey(noun))
                {
                    useDictionary.Add(noun, interaction.actionResponse);
                }
            }
        }
    }

    InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for (int i = 0; i < usableItemList.Count; i++)
        {
            if (usableItemList[i].noun == noun)
            {
                return usableItemList[i];
            }
        }
        return null;
    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("Miras en tu bolsillo e, increiblemente, ves: ");
        string objectToDisplay = "";
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            objectToDisplay = nounsInInventory[i];

            if (objectsInRoomDictionary.ContainsKey(objectToDisplay))
            {
                if (objectsInRoomDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "-Un " + nounsInInventory[i];
                else
                    objectToDisplay = "-Una " + nounsInInventory[i];
            }

            controller.LogStringWithReturn(objectToDisplay);
        }
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        throwDictionary.Clear();
        objectsInRoomDictionary.Clear();
        inverseNouns.Clear();
        nounsInRoom.Clear();
    }

    public Dictionary<string, string> Take (string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];

        if (!inverseNouns.ContainsKey(noun))
        {
            if (nounsInRoom.Contains(noun))
            {
                nounsInInventory.Add(noun);
                AddActionResponsesToUseDictionary();

                nounsInRoom.Remove(noun);

                return takeDictionary;
            }
            else
            {
                string objectToDisplay = noun;
                if (objectsInRoomDictionary.ContainsKey(objectToDisplay))
                {
                    if (objectsInRoomDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                        objectToDisplay = "un " + noun;
                    else
                        objectToDisplay = "una " + noun;
                }


                controller.LogStringWithReturn("No hay " + objectToDisplay + " por acá");
                return null;
            }
        }
        else
        {
            controller.LogStringWithReturn(inverseNouns[noun].textResponse);
            return null;
        }

    }

    public bool Throw (string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];

        if (!inverseNouns.ContainsKey(noun))
        {
            if (nounsInInventory.Contains(noun))
            {
                nounsInRoom.Add(noun);
                if (useDictionary.ContainsKey(noun))
                {
                    useDictionary.Remove(noun);
                }

                nounsInInventory.Remove(noun);
                return true;
            }
            else
            { 
                controller.LogStringWithReturn("No puedes lanzar algo que no tienes.");
                return false;
            }
        }
        else
        {
            controller.LogStringWithReturn(inverseNouns[noun].textResponse);
            return false;
        }
    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];
        string objectToDisplay = nounToUse;


        if (nounsInInventory.Contains(nounToUse))
        {
            if (!inverseNouns.ContainsKey(nounToUse))
            {
                if (useDictionary.ContainsKey(nounToUse))
                {
                    bool actionResult = useDictionary[nounToUse].DoActionResponse(controller);
                    if (!actionResult)
                    {
                        controller.LogStringWithReturn("Hmm. No parece ocurrir nada");
                    }
                }
                else
                {
                    if (objectsInRoomDictionary.ContainsKey(objectToDisplay))
                    {
                        if (objectsInRoomDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                            objectToDisplay = "el " + nounToUse;
                        else
                            objectToDisplay = "la " + nounToUse;
                    }

                    controller.LogStringWithReturn("No se puede usar " + objectToDisplay);
                }
            }
            else
            {
                controller.LogStringWithReturn(inverseNouns[nounToUse].textResponse);
            }
        } else
        {
            if (objectsInRoomDictionary.ContainsKey(objectToDisplay))
            {
                if (objectsInRoomDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "un " + nounToUse;
                else
                    objectToDisplay = "una " + nounToUse;
            }

            controller.LogStringWithReturn("No hay " + objectToDisplay + " en tu inventario!");
        }
    }


}

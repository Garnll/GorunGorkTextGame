using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour {

    public InventoryManager inventoryManager;
    public List<InteractableObject> usableItemList;

    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> throwDictionary = new Dictionary<string, string>();

    [HideInInspector] public List<string> nounsInRoom = new List<string>();
    [HideInInspector] public Dictionary<string, InteractableObject> objectsWithinReachDictionary = new Dictionary<string, InteractableObject>();
    [HideInInspector] public Dictionary<string, Interaction> inverseNouns = new Dictionary<string, Interaction>();

    Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        inventoryManager.interactableItems = this;
    }

    public string GetObjectNotInInventory (Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

        if (!inventoryManager.nounsInInventory.Contains (interactableInRoom))
        {
            nounsInRoom.Add(interactableInRoom.noun);
            objectsWithinReachDictionary.Add(interactableInRoom.noun, interactableInRoom);

            return interactableInRoom.description;
        }

        return null;
    }

    public void GetObjectsInInventory ()
    {
        for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++)
        {
            objectsWithinReachDictionary.Add(inventoryManager.nounsInInventory[i].noun, inventoryManager.nounsInInventory[i]);
        }

    }

    public void AddActionResponsesToUseDictionary()
    {
        for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++)
        {
            string noun = inventoryManager.nounsInInventory[i].noun;

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

    public void DisplayInventoryByCommand()
    {
        controller.LogStringWithReturn("Miras en tu bolsillo e, increiblemente, ves: ");
        string objectToDisplay = "";


        for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++)
        {
            objectToDisplay = inventoryManager.nounsInInventory[i].noun;

            if (objectsWithinReachDictionary.ContainsKey(objectToDisplay))
            {
                if (objectsWithinReachDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "-Un " + inventoryManager.nounsInInventory[i].noun;
                else
                    objectToDisplay = "-Una " + inventoryManager.nounsInInventory[i].noun;
            }



        }


    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        throwDictionary.Clear();
        objectsWithinReachDictionary.Clear();
        inverseNouns.Clear();
        nounsInRoom.Clear();
    }

    public Dictionary<string, string> Take (string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];

        if (!inverseNouns.ContainsKey(noun + takeDictionary[noun]))
        {
            if (nounsInRoom.Contains(noun))
            {
                inventoryManager.nounsInInventory.Add(objectsWithinReachDictionary[noun]);
                AddActionResponsesToUseDictionary();

                nounsInRoom.Remove(noun);

                inventoryManager.DisplayInventory();

                return takeDictionary;
            }
            else
            {
                string objectToDisplay = noun;
                if (objectsWithinReachDictionary.ContainsKey(objectToDisplay))
                {
                    if (objectsWithinReachDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                        objectToDisplay = "un " + noun;
                    else
                        objectToDisplay = "una " + noun;
                }


                controller.LogStringWithReturn("No hay " + objectToDisplay + " por acá.");
                return null;
            }
        }
        else
        {
            controller.LogStringWithReturn(inverseNouns[noun + takeDictionary[noun]].textResponse);
            return null;
        }

    }

    public bool Throw (string[] separatedInputWords)
    {
        string noun = separatedInputWords[1];

        if (!inverseNouns.ContainsKey(noun + throwDictionary[noun]))
        {
            if (inventoryManager.nounsInInventory.Contains(objectsWithinReachDictionary[noun]))
            {
                nounsInRoom.Add(noun);
                if (useDictionary.ContainsKey(noun))
                {
                    useDictionary.Remove(noun);
                }

                inventoryManager.nounsInInventory.Remove(objectsWithinReachDictionary[noun]);

                inventoryManager.DisplayInventory();

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
            controller.LogStringWithReturn(inverseNouns[noun + throwDictionary[noun]].textResponse);
            return false;
        }
    }

    public void UseItem(string[] separatedInputWords)
    {
        string nounToUse = separatedInputWords[1];
        string objectToDisplay = nounToUse;


        if (inventoryManager.nounsInInventory.Contains(objectsWithinReachDictionary[nounToUse]))
        {
            if (!inverseNouns.ContainsKey(nounToUse + useDictionary[nounToUse]))
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
                    if (objectsWithinReachDictionary.ContainsKey(objectToDisplay))
                    {
                        if (objectsWithinReachDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                            objectToDisplay = "el " + nounToUse;
                        else
                            objectToDisplay = "la " + nounToUse;
                    }

                    controller.LogStringWithReturn("No se puede usar " + objectToDisplay);
                }
            }
            else
            {
                controller.LogStringWithReturn(inverseNouns[nounToUse + useDictionary[nounToUse]].textResponse);
            }

        } else
        {
            if (objectsWithinReachDictionary.ContainsKey(objectToDisplay))
            {
                if (objectsWithinReachDictionary[objectToDisplay].nounGender == InteractableObject.WordGender.male)
                    objectToDisplay = "un " + nounToUse;
                else
                    objectToDisplay = "una " + nounToUse;
            }

            controller.LogStringWithReturn("No hay " + objectToDisplay + " en tu inventario!");
        }
    }


}

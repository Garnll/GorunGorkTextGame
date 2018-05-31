using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que controla el comportamieto de los objetos interactuables.
/// </summary>
public class ItemHandler : MonoBehaviour {

    public InventoryManager inventoryManager;
	public EquipManager equipManager;
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

                controller.LogStringWithReturn("No hay " + objectToDisplay + " aquí.");
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
				if (inventoryManager.nounsInInventory[f].GetType() == typeof(Pocket)) {
					Pocket p = inventoryManager.nounsInInventory[f] as Pocket;
					if (p.have(objectsFound[i])) {
						objectToInteract = p.ingredients[p.getIndex(objectsFound[i])];
						break;
					}
				}
				else {
					if (objectsFound[i].nouns == inventoryManager.nounsInInventory[f].nouns) {
						objectToInteract = inventoryManager.nounsInInventory[f];
						break;
					}
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

        combinedText.Add("<b>Inventario.</b> \n" +
			"[" + controller.playerManager.characteristics.usedPods + "/" + controller.playerManager.characteristics.currentMaxPods
			+ "]" + "\n");
        string objectToDisplay = "";

        if (inventoryManager.nounsInInventory.Count == 0)
        {
            combinedText.Add("- Nada.");
        }

        for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++)
        {
            objectToDisplay = inventoryManager.nounsInInventory[i].objectName;

            if (inventoryManager.nounsInInventory[i].nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "- Un " + inventoryManager.nounsInInventory[i].objectName + ".";
            else
                objectToDisplay = "- Una " + inventoryManager.nounsInInventory[i].objectName + ".";

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
            string objectToDisplay = objectToExamine.objectName;

            if (objectToExamine.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToExamine.objectName;
            else
                objectToDisplay = "la " + objectToExamine.objectName;

            controller.LogStringWithReturn("No logras examinar " + objectToDisplay + ".");
            return;
        }

        controller.LogStringWithReturn(examineInteraction.textResponse);
		examineInteraction.applyEffects();
		controller.questManager.updateQuests();
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
            string objectToDisplay = objectToTake.objectName;

            if (objectToTake.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToTake.objectName;
            else
                objectToDisplay = "la " + objectToTake.objectName;

            controller.LogStringWithReturn("No se puede coger " + objectToDisplay + ".");
            return;
        }

		if (objectToTake.isIngredient) {
			if (inventoryManager.hasPockets()) {
				List<Pocket> pockets = inventoryManager.getPockets();
				Pocket currentPocket = null;

				for (int i = 0; i < pockets.Count; i++) {
					if (!pockets[i].have(objectToTake) && pockets[i].usage >= pockets[i].capacity) {
						Debug.Log("El pocket no tiene el ingrediente ni capacidad para guardarlo");
					}
					else {
						if (pockets[i].have(objectToTake)) {
							currentPocket = pockets[i];
							break;
						}
						else {
							currentPocket = pockets[i];						
						}
					}
				}
				Debug.Log(currentPocket.objectName);
				currentPocket.saveIngredient(objectToTake);
				removeFromRoom(objectToTake);
				inventoryManager.DisplayInventory();
				controller.LogStringWithReturn(takeInteraction.textResponse);
				takeInteraction.applyEffects();
				controller.questManager.updateQuests();
				return;
			} 
		}

		if (!controller.playerManager.characteristics.checkPodsWith(objectToTake.pods)) {
			string objectToDisplay = objectToTake.objectName;
			if (objectToTake.nounGender == InteractableObject.WordGender.male)
				objectToDisplay = "No hay espacio para recoger el " + objectToTake.objectName + ".";
			else
				objectToDisplay = "No hay espacio para recoger la " + objectToTake.objectName + ".";

			controller.LogStringWithReturn(objectToDisplay);
			return;
		}

		if (takeInteraction.isInverseInteraction){
            controller.LogStringWithReturn(takeInteraction.textResponse);
            return;
        }

		inventoryManager.nounsInInventory.Add(objectToTake);
		controller.playerManager.characteristics.addWeight(objectToTake.pods);

		removeFromRoom(objectToTake);
        inventoryManager.DisplayInventory();
        controller.LogStringWithReturn(takeInteraction.textResponse);
    }

	public void removeFromRoom(InteractableObject objectToRemove) {
		for (int i = 0; i < controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Count; i++) {
			if (controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom[i].interactableObject ==
				objectToRemove) {
				controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.RemoveAt(i);
				break;
			}
		}
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
            string objectToDisplay = objectToThrow.objectName;

            if (objectToThrow.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToThrow.objectName;
            else
                objectToDisplay = "la " + objectToThrow.objectName;

            controller.LogStringWithReturn("No se puede tirar " + objectToDisplay + ".");
            return;
        }

        if (throwInteraction.isInverseInteraction)
        {
            controller.LogStringWithReturn(throwInteraction.textResponse);
            return;
        }



        RoomObject.RoomVisibleObjects newObjectInRoom = new RoomObject.RoomVisibleObjects();
        newObjectInRoom.interactableObject = objectToThrow;
        newObjectInRoom.visionRange = (int)Random.Range(-4,4);

        controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Add(newObjectInRoom);


		if (objectToThrow.isIngredient && inventoryManager.hasPockets()) {
			bool throwed = false;
			try {
				foreach (Pocket p in inventoryManager.nounsInInventory) {
					if (p.have(objectToThrow)) {
						p.remove(objectToThrow, 1);
						throwed = true;
					}
				}
			} catch {

			}

			if (throwed == false) {
				inventoryManager.nounsInInventory.Remove(objectToThrow);
				controller.playerManager.characteristics.removeWeight(objectToThrow.pods);
			}
		}
		else {
			inventoryManager.nounsInInventory.Remove(objectToThrow);
			controller.playerManager.characteristics.removeWeight(objectToThrow.pods);
		}

        inventoryManager.DisplayInventory();
        controller.LogStringWithReturn(throwInteraction.textResponse);
		throwInteraction.applyEffects();
		controller.questManager.updateQuests();
	}

    public void UseObject(InteractableObject objectToUse){
        Interaction useInteraction = null;
        InteractableConsumableObject consumable = null;

        for (int i = 0; i < objectToUse.interactions.Length; i++) 
			{

			if (objectToUse.interactions[i].inputAction.GetType() == typeof(UseInput)){
                useInteraction = objectToUse.interactions[i];

				if (objectToUse.GetType() == typeof(InteractableConsumableObject)){
                    consumable = objectToUse as InteractableConsumableObject;
                }
                break;
            }
        }

        if (useInteraction == null)
			{
            string objectToDisplay = objectToUse.objectName;

            if (objectToUse.nounGender == InteractableObject.WordGender.male)
                objectToDisplay = "el " + objectToUse.objectName;
            else
                objectToDisplay = "la " + objectToUse.objectName;

            if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
            {
                controller.LogStringWithReturn("No puedes usar " + objectToDisplay + ".");
            }
            else if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                controller.combatController.UpdatePlayerLog("No puedes usar " + objectToDisplay + ".");
            }
            return;
        }

        if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
        {
            controller.LogStringWithReturn(useInteraction.textResponse);
        }
        else if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            controller.combatController.UpdatePlayerLog(useInteraction.textResponse);
        }

        if (useInteraction.isInverseInteraction)
		{
            return;
        }


        if (objectToUse.GetType() == typeof(Recipient)) {
			controller.craftController.initialize(objectToUse as Recipient);
		}


        if (useInteraction.actionResponse == null)
        {
            return;
        }

        bool action = useInteraction.actionResponse.DoActionResponse(controller);
        if (action)
        {
            consumable.UseObject();
            if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
            {
                controller.LogStringWithReturn(useInteraction.actionResponse.responseDescription);
            }
            else if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                controller.combatController.UpdatePlayerLog(useInteraction.actionResponse.responseDescription);
            }

            useInteraction.applyEffects();
			controller.questManager.updateQuests();

			if (!consumable.IsUseful)
            {
                consumable.StopWorking(controller);
                inventoryManager.nounsInInventory.Remove(consumable);
                inventoryManager.DisplayInventory();
            }


        }
        else
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
            {
                controller.LogStringWithReturn(useInteraction.actionResponse.negationDescription);
            }
            else if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                controller.combatController.UpdatePlayerLog(useInteraction.actionResponse.negationDescription);
            }
        }
    }

	public void EquipObject(InteractableObject objectToEquip) {
		Interaction equipInteraction = null;
		Equip equip = null;

		try {
			equip = objectToEquip as Equip;
		} catch {
			controller.LogStringWithReturn("Eso no es un equipo.");
		}


		for (int i=0; i<objectToEquip.interactions.Length; i++) {
			if (objectToEquip.interactions[i].inputAction.GetType() == typeof(EquipInput)) {
				equipInteraction = objectToEquip.interactions[i];
				//Revisar si debo hacerlo por cada tipo de una vez aquí.
				break;
			}
		}

		if (equipInteraction == null) {
			string objectToDisplay = objectToEquip.objectName;
			if (objectToEquip.nounGender == InteractableObject.WordGender.male)
				objectToDisplay = "el " + objectToEquip.objectName;
			else
				objectToDisplay = "la " + objectToEquip.objectName;
			controller.LogStringWithReturn("No puedes equiparte " + objectToDisplay + ".");
			return;
		}
		if (equipInteraction.isInverseInteraction) {
			controller.LogStringWithReturn(equipInteraction.textResponse);
			return;
		}

		controller.LogStringWithReturn(equipInteraction.textResponse);
		equipManager.put(equip);

		for (int i = 0; i < controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.Count; i++) {
			if (controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom[i].interactableObject ==
				objectToEquip) {
				controller.playerRoomNavigation.currentRoom.visibleObjectsInRoom.RemoveAt(i);
				break;
			}
		}

		for (int i = 0; i < inventoryManager.nounsInInventory.Count; i++) {
			if (inventoryManager.nounsInInventory[i] == objectToEquip) {
				inventoryManager.nounsInInventory.RemoveAt(i);
				break;
			}
		}
		inventoryManager.DisplayInventory();
		equipInteraction.applyEffects();
		controller.questManager.updateQuests();

		if (equipInteraction.actionResponse == null) {
			return;
		}

		bool action = equipInteraction.actionResponse.DoActionResponse(controller);
		if (action) {
			controller.LogStringWithReturn(equipInteraction.actionResponse.responseDescription);
			foreach (InteractableObject i in inventoryManager.nounsInInventory) {
				if (i == equip) {

				}
			}
		}
		else {
			controller.LogStringWithReturn(equipInteraction.actionResponse.negationDescription);
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

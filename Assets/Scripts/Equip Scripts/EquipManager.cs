using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour {

	public GameObject stats;
	public InventoryManager inventoryManager;

	PlayerCharacteristics characteristics;
	PlayerOther other;

	[Space(10)]

	[SerializeField] public Tool tool;
	[SerializeField] public Outfit outfit;
	[SerializeField] public Bag bag;
	[SerializeField] public Accesory accesory;

	public void Start() {
		characteristics = stats.GetComponent<PlayerCharacteristics>();
		other = stats.GetComponent<PlayerOther>();
	}

	public void put(Equip equip) {
		if (equip.GetType() == typeof(Tool)) {
			if (tool != null) {
				tool.remove();
			}
			tool = equip as Tool;
			characteristics.applyBuffs(tool);
			other.applyBuffs(tool);
		}

		if (equip.GetType() == typeof(Outfit)) {
			if (outfit != null) {
				outfit.remove();
			}
			outfit = equip as Outfit;
			characteristics.applyBuffs(outfit);
			other.applyBuffs(outfit);
		}

		if (equip.GetType() == typeof(Bag)) {
			if (bag != null) {
				bag.remove();
			}
			bag = equip as Bag;
			characteristics.applyBuffs(bag);
			other.applyBuffs(bag);
		}

		if (equip.GetType() == typeof(Accesory)) {
			if (accesory != null) {
				accesory.remove();
			}
			accesory = equip as Accesory;
			characteristics.applyBuffs(accesory);
			other.applyBuffs(accesory);
		}
		//Le doy los buffs del objeto.
	}


	public void remove(Equip equip) {
		//Aquí se pone el equipo actual en el inventario.
		//Y le quito los buffs del objeto.
		inventoryManager.nounsInInventory.Add(equip);
		characteristics.removeBuffs(equip);
		other.removeBuffs(equip);
	}
}

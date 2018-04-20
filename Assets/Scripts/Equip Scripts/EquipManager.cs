using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipManager : MonoBehaviour {

	public PlayerManager player;
	public GameObject stats;
	public InventoryManager inventoryManager;
	public TextMeshProUGUI text;

	PlayerCharacteristics character;
	PlayerOther other;

	[Space(10)]

	[SerializeField] public Tool tool;
	[SerializeField] public Outfit outfit;
	[SerializeField] public Bag bag;
	[SerializeField] public Accesory accesory;

	public void Start() {
		character = stats.GetComponent<PlayerCharacteristics>();
		other = stats.GetComponent<PlayerOther>();
		updateText();
	}

	public void put(Equip equip) {
		if (equip.GetType() == typeof(Tool)) {
			if (tool != null) {
				remove(tool);
			}
			tool = equip as Tool;
			applyBuffs(tool);
		}

		if (equip.GetType() == typeof(Outfit)) {
			if (outfit != null) {
				remove(outfit);
			}
			outfit = equip as Outfit;
			applyBuffs(outfit);
		}

		if (equip.GetType() == typeof(Bag)) {
			if (bag != null) {
				remove(bag);
			}
			bag = equip as Bag;
			applyBuffs(bag);
		}

		if (equip.GetType() == typeof(Accesory)) {
			if (accesory != null) {
				remove(accesory);
			}
			accesory = equip as Accesory;
			applyBuffs(accesory);
		}
		//Le doy los buffs del objeto.
		inventoryManager.DisplayInventory();
		updateText();
	}

	public void remove(Equip equip) {
		//Aquí se pone el equipo actual en el inventario.
		//Y le quito los buffs del objeto.
		inventoryManager.nounsInInventory.Add(equip);
		removeBuffs(equip);
		inventoryManager.DisplayInventory();

		if (equip.GetType() == typeof(Tool)) {
			tool = null;
		}

		if (equip.GetType() == typeof(Outfit)) {
			outfit = null;
		}

		if (equip.GetType() == typeof(Bag)) {
			bag = null;
		}

		if (equip.GetType() == typeof(Accesory)) {
			accesory = null;
		}
	}

	public void applyBuffs(Equip equip) {
		modifyIntStats(equip, 1);
		modifyFloatStats(equip, 1);

	}

	public void removeBuffs(Equip equip) {
		modifyIntStats(equip, -1);
		modifyFloatStats(equip, -1);
	}

	private void modifyIntStats(Equip equip, int value) {
		IntBuff[] buffs = equip.intBuffs;
		for (int i = 0; i < buffs.Length; i++) {
			switch (buffs[i].type) {
				case IntBuff.IntBuffType.Health:
					player.ModifyHealthBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Strength:
					character.modifyStrengthBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Dexterity:
					character.modifyDexterityBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Intelligence:
					character.modifyIntelligenceBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Resistance:
					character.modifyResistanceBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Pods:
					character.modifyPodsBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Infravision:
					character.modifyInfravisionBy(buffs[i].magnitude * value);
					break;

				case IntBuff.IntBuffType.Hypervision:
					character.modifySupravisionBy(buffs[i].magnitude * value);
					break;
			}
		}
	}

	private void modifyFloatStats(Equip equip, int value) {
		FloatBuff[] buffs = equip.floatBuffs;
		for (int i = 0; i < buffs.Length; i++) {
			switch (buffs[i].type) {
				case FloatBuff.FloatBuffType.Cooldown:
					other.modifyCoolDownBy(buffs[i].magnitude * value);
					break;

				case FloatBuff.FloatBuffType.Crit:
					other.modifyCritBy(buffs[i].magnitude * value);
					break;

				case FloatBuff.FloatBuffType.Evasion:
					other.modifyEvasionBy(buffs[i].magnitude * value);
					break;

				case FloatBuff.FloatBuffType.HealthRegen:
					other.modifyHealthRegenBy(buffs[i].magnitude * value);
					break;

				case FloatBuff.FloatBuffType.TurnRegen:
					other.modifyTurnRegenBy(buffs[i].magnitude * value);
					break;

			}
		}
	}

	public void updateText() {

		string toolName = nameOf(tool);
		string outfitName = nameOf(outfit);
		string bagName = nameOf(bag);
		string accesoryName = nameOf(accesory);


		string info = "<b>Equipo</b>\n" +
			"<b>H:</b> " + toolName + "\n" +
			"<b>T:</b> " + outfitName + "\n" +
			"<b>M:</b> " + bagName + "\n" +
			"<b>A:</b> " + accesoryName;
		text.text = info;
	}

	private string nameOf(Equip equip) {
		string temp = " ";
		try {
			temp = equip.objectName + ".";
		} catch {

		}
		return temp;
	}

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

	[HideInInspector] public DialogueNPC npc;

	public InventoryManager inventoryManager;
	public PlayerRoomNavigation roomNav;

	private Dialogue currentDialogue;

	public void setDialogue() {
		if (npc != null) {
			currentDialogue = npc.dialogueTree;
		}
	}

	public void selectChoice(string keyword) {
		if (currentDialogue.getChoiceWithKeyword(keyword) != null) {
			currentDialogue = currentDialogue.getChoiceWithKeyword(keyword);
			currentDialogue.applyEffects();
		}
	}

}

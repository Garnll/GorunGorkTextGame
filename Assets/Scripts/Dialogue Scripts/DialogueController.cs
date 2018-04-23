using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

	[HideInInspector] public DialogueNPC currentNpc;
	private List<DialogueNPC> npcsInRoom;

	public GameController controller;
	public PlayerManager player;
	public InventoryManager inventoryManager;
	public PlayerRoomNavigation roomNav;

	[HideInInspector] public string input;

	public string[] ends;

	private Dialogue currentDialogue;

	public DialogueNPC tryTalkingTo(string[] keywords) {
		npcsInRoom = roomNav.currentRoom.charactersInRoom;

		foreach (DialogueNPC npc in npcsInRoom) {
			foreach (string keyword in npc.keywords) {
				if (keywords[keywords.Length - 1] == keyword) {
					currentNpc = npc;
					setDialogue();
				}
			}
		}

		return currentNpc;
	}

	public void setDialogue() {
		if (currentNpc != null) {
			currentDialogue = currentNpc.dialogueTree;
			
		}
	}

	public void selectChoiceWith(string keyword) {
		if (currentDialogue.getChoiceWithKeyword(keyword) != null) {
			currentDialogue = currentDialogue.getChoiceWithKeyword(keyword);
			currentDialogue.applyEffects();
			controller.LogStringWithReturn(currentDialogue.getText());
		}
	}

	public bool isInLeaf() {
		if (currentDialogue.isLeaf()) {
			return true;
		}
		return false;
	}

	public void getDialogueNpcsInRoom() {
		npcsInRoom = null;
		foreach (DialogueNPC template in roomNav.currentRoom.charactersInRoom) {
			Debug.Log("Found '" + template.npcName + "' in room.");
			npcsInRoom.Add(template);

		}
	}

	public void takeInput(string input) {

		foreach (string e in ends) {
			if (input == e) {
				controller.DisplayRoomText();
				GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
			}
		}
		
		foreach (Choice c in currentDialogue.choices) {
			foreach (string keyword in c.keywords) {
				if (input == keyword) {
					selectChoiceWith(input);
				}
			}
		}

	}

	public IEnumerator talk() {
		yield return new WaitUntil(() => player.controller.writing == false && player.controller.HasFinishedWriting());
		GameState.Instance.ChangeCurrentState(GameState.GameStates.conversation);

		takeInput(input);
	}

}

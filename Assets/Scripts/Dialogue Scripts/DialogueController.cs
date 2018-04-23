using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

	[HideInInspector] public string input;
	[HideInInspector] public bool isTalking;
	[HideInInspector] public DialogueNPC currentNpc;
	private List<DialogueNPC> npcsInRoom;
	private Dialogue currentDialogue;

	public GameController controller;
	public PlayerManager player;
	public InventoryManager inventoryManager;
	public PlayerRoomNavigation roomNav;

	public string[] ends;



	public DialogueNPC tryTalkingTo(string[] keywords) {
		npcsInRoom = roomNav.currentRoom.charactersInRoom;

		string temp = keywords[keywords.Length - 1];
		for (int i = keywords.Length - 2; i > 0; i--) {
			temp = keywords[i] + " " + temp;

			foreach (DialogueNPC npc in npcsInRoom) {
				foreach (string k in npc.keywords) {

					if (k == temp) {
						currentNpc = npc;
						setDialogue();
						return currentNpc;
					}

					if (keywords[keywords.Length - 1] == k) {
						currentNpc = npc;
						setDialogue();
						return currentNpc;
					}

				}
			}

		}
		return null;	
	}

	public void setDialogue() {
		if (currentNpc != null) {
			currentDialogue = currentNpc.dialogueTree;			
		}
	}

	public void displayText() {
		controller.LogStringWithReturn("<b>" + currentNpc.npcName + ":</b> " + currentDialogue.getText());
	}

	public void selectChoiceWith(string keyword) {
		if (currentDialogue.getChoiceWithKeyword(keyword) != null) {
			currentDialogue = currentDialogue.getChoiceWithKeyword(keyword);
			currentDialogue.applyEffects();
			displayText();
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
				isTalking = false;
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

	public bool checkInput(string input) {
		foreach (Choice c in currentDialogue.choices) {
			foreach (string keyword in c.keywords) {
				if (input == keyword) {
					return true;
				}
			}
		}

		foreach (string s in ends) {
			if (input == s) {
				return true;
			}
		}
		return false;
	}


	public IEnumerator talk() {
		yield return new WaitUntil(() => player.controller.writing == false && player.controller.HasFinishedWriting());
		GameState.Instance.ChangeCurrentState(GameState.GameStates.conversation);
		isTalking = true;
		takeInput(input);
	}

}

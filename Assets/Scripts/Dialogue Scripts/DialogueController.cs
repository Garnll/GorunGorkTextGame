using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

	[HideInInspector] public string input;
	[HideInInspector] public bool isTalking;
	[HideInInspector] public DialogueNPC currentNpc;
	private List<DialogueNPC> npcsInRoom;
	public Dialogue currentDialogue;

	public GameController controller;
	public PlayerManager player;
	public InventoryManager inventoryManager;
	public PlayerRoomNavigation roomNav;

	[Space(20)]
	[TextArea]
	public string endText;

	public string[] ends;



	public DialogueNPC tryTalkingTo(string[] keywords) {
		npcsInRoom = roomNav.currentRoom.charactersInRoom;

		string temp = keywords[keywords.Length - 1];

		if (keywords.Length == 2) {
			foreach (DialogueNPC npc in npcsInRoom) {
				foreach (string k in npc.keywords) {
					if (keywords[1] == k) {
						currentNpc = npc;
						setDialogue();
						return currentNpc;
					}
				}
			}
		}


		for (int i = keywords.Length - 2; i > 0; i--) {
			temp = keywords[i] + " " + temp;

			foreach (DialogueNPC npc in npcsInRoom) {
				foreach (string k in npc.keywords) {

					if (keywords[keywords.Length - 1] == k) {
						currentNpc = npc;
						setDialogue();
						return currentNpc;
					}
					else {

						if (k == temp) {
							currentNpc = npc;
							setDialogue();
							return currentNpc;
						}
					}
					

				}
			}

		}
		return null;	
	}

	public void setDialogue() {
		if (currentNpc != null) {
			currentDialogue = currentNpc.dialogueTree;
			currentDialogue.setGlobalVariables();
		}
	}

	public void displayText() {

		string tempNarrationText = "";

		if (currentDialogue.narrator == Dialogue.NarratorType.character) {
			tempNarrationText = "<b>" + currentNpc.npcName + ":</b> ";
		}
		controller.LogStringWithReturn( tempNarrationText + currentDialogue.getText() + getChoicesText() + endText);
	}

	public string getChoicesText() {
		string temp = "";
		if (currentDialogue.choices != null) {
			foreach (Choice c in currentDialogue.choices) {
				if (c.isAble()) {
					temp += "<b>[" + c.keywords[0].ToUpper() + "]</b> " + c.text + "\n";
				}
			}
		} else {
			temp += "\n";
		}
		return temp;
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
				if (input == keyword && c.isAble()) {
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

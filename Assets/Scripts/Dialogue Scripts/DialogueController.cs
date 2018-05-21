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
	public QuestManager questManager;

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
			questManager.updateQuests();
		}
	}

	public void displayText() {

		string tempNarrationText = "";

		if (currentDialogue.narrator == Dialogue.NarratorType.character) {
			string code = ColorUtility.ToHtmlStringRGB(controller.dialogueController.currentNpc.color);
			tempNarrationText = "<b><color=#" + code + ">" + currentNpc.npcName + ":</color></b> ";
		}
		controller.LogStringWithReturn( tempNarrationText + currentDialogue.getText() + getChoicesText() + endText);
	}

	public string getChoicesText() {
		string temp = "";
		if (currentDialogue.choices != null) {
			foreach (Choice c in currentDialogue.choices) {
				if (c.isAble()) {
					temp += "<b><color=#FBEBB5>[" + c.keywords[0].ToUpper() + "]</color></b> " + c.text + "\n";
				}
			}
		} else {
			temp += "\n";
		}
		return temp;
	}

	public void selectChoiceWith(string keyword) {
		if (currentDialogue.getDialogueFromChoice(keyword) != null) {
			currentDialogue.getChoice(keyword).applyEffects();
			currentDialogue = currentDialogue.getDialogueFromChoice(keyword);
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

		questManager.updateQuests();

		foreach (string e in ends) {
			if (input == e) {
				StopCoroutine("talk");
				isTalking = false;
				controller.LogStringWithReturn("\n");
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
		GameState.Instance.ChangeCurrentState(GameState.GameStates.conversation);
		isTalking = true;
		yield return new WaitUntil(() => player.controller.writing == false && player.controller.HasFinishedWriting());
		takeInput(input);
	}

}

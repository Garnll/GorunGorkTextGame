using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/TalkTo")]
public class TalkToInput : InputActions {

	public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords) {

		if (separatedInputWords.Length > 1) {

			DialogueNPC npc = controller.dialogueController.tryTalkingTo(separatedInputWords);

			if (npc == null) {
				controller.LogStringWithReturn("No existe '" + separatedInputWords[separatedInputWords.Length - 1] + "' con quien hablar.");
				return;
			}

			string tempNarrationText = "";

			if (controller.dialogueController.currentDialogue.narrator == Dialogue.NarratorType.character) {
				tempNarrationText = "<b><color=#F9EEC1>" + controller.dialogueController.currentNpc.npcName + ":</color></b> ";
			}
			controller.dialogueController.StartCoroutine("talk");
			controller.LogStringWithReturn(tempNarrationText + npc.dialogueTree.getText()
				+ controller.dialogueController.getChoicesText() + controller.dialogueController.endText);

			controller.dialogueController.input = separatedInputWords[0];
			
		}

	}

}

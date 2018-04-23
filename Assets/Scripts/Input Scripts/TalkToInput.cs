using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/TalkTo")]
public class TalkToInput : InputActions {

	public override void RespondToInput(GameController controller, string[] separatedInputWords) {

		if (separatedInputWords.Length > 1) {

			DialogueNPC npc = controller.dialogueController.tryTalkingTo(separatedInputWords);

			if (npc == null) {
				controller.LogStringWithReturn("No existe '" + separatedInputWords[1] + "' con quien hablar.");
				return;
			}

			controller.LogStringWithReturn(npc.dialogueTree.getText() + "");

			controller.dialogueController.input = separatedInputWords[0];
			controller.dialogueController.StartCoroutine("talk");
		}

	}

}

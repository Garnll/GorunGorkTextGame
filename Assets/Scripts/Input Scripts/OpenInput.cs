using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Open")]
public class OpenInput : InputActions {

	public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords) {
		
		if (separatedInputWords.Length > 1) {

			InteractableObject i = controller.playerManager.inventoryManager.tryOpen(separatedInputWords);

			if (i != null) {
				controller.LogStringWithReturn(i.Open());
			} else {
				controller.LogStringWithReturn("Nope.");
			}

		}
	}
}

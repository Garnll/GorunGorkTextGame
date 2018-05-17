using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Equip")]
public class EquipInput : InputActions {

	public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords) {
		InteractableObject target = controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, true, true);

		if (target != null) {
			controller.itemHandler.EquipObject(target);
		}
	}
}

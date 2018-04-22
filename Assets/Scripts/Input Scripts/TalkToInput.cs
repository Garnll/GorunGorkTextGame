using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/TalkTo")]
public class TalkToInput : InputActions {

	public override void RespondToInput(GameController controller, string[] separatedInputWords) {

		//InteractableObject target = controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, true, true);
		//target - > Buscar npcs en la habitación.

		//if (target != null) {
		//	controller.itemHandler.EquipObject(target);
		//}
	}
}

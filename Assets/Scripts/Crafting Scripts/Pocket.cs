using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Craft/Pocket")]
public class Pocket : InteractableObject {

	public int capacity;
	public int usage = 0;
	public List<InteractableObject> ingredients;

	public void OnEnable() {
		ingredients = new List<InteractableObject>();
		usage = 0;
	}

	public bool isIn(InteractableObject i, List<InteractableObject> l) {
		if (l == null || l.Count == 0) {
			return false;
		}
		for (int j = 0; j < l.Count; j++) {
			if (i.objectName == l[j].objectName) {
				return true;
			}
		}
		return false;
	}

	public override string Open() {
		string text = "<b><color=#F9EEC1>" + objectName + ".</color></b>\n";
		text += "Contenido: " + usage + " de " + capacity + " ingredientes.\n";
		text += "---\n";

		List<InteractableObject> tempList = new List<InteractableObject>();
		if (ingredients.Count > 0) {
			foreach (InteractableObject i in ingredients) {
				if (!isIn(i, tempList)) {
					tempList.Add(i);
					int volume = getVolumeOf(i);
					text += "> " + i.objectName + " x" + volume + ".\n";
				}
			}

			text += "---";
		} else {
			text += "<i>Vacío.</i>\n";
			text += "---";
		}
		return text;
	}

	public void updateUsage() {
		usage = ingredients.Count;
	}

	public int getVolumeOf(InteractableObject i) {
		int count = 0;
		foreach (InteractableObject n in ingredients) {
			if (i.objectName == n.objectName) {
				count++;
			}
		}

		return count;
	}

	public void saveIngredient(InteractableObject i) {
		Debug.Log(i.objectName);
		ingredients.Add(i);
		updateUsage();
	}

	public bool have(InteractableObject i) {
		if (ingredients == null) {
			return false;
		}

		for (int k=0; k < ingredients.Count; k++) {
			if (ingredients[k].objectName == i.objectName) {
				return true;
			}
		}
		return false;
	}

	public int getIndex(InteractableObject i) {
		for (int k = 0; k < ingredients.Count; k++) {
			if (ingredients[k].objectName == i.objectName) {
				return k;
			}
		}

		return ingredients.Count;
	}

	public void remove(InteractableObject i, int times) {
		if (have(i)) {
			int t = 0;
			int volume = getVolumeOf(i);

			for (int k = 0; k < ingredients.Count; k++) {
				if (ingredients[k].objectName == i.objectName && t < times) {
					ingredients.RemoveAt(k);
					t++;
					updateUsage();

					if (t == times) {
						return;
					}
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Craft/Pocket")]
public class Pocket : InteractableObject {

	public int capacity;
	public List<InteractableObject> ingredients;

	public int getVolumeOf(InteractableObject i) {
		int count = 0;
		foreach (InteractableObject n in ingredients) {
			if (i == n) {
				count++;
			}
		}

		return count;
	}
}

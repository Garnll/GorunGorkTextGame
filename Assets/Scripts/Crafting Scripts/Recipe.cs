using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Craft/Recipe")]
public class Recipe : ScriptableObject {

	public string recipeName;
	public InteractableObject product;

	public float minTime;
	public float maxTime;

	public List<Fragment> fragments;

	[HideInInspector] public List<Fragment> beforeFragments;
	[HideInInspector] public List<Fragment> whileFragments;
	[HideInInspector] public List<Fragment> pauseFragments;
	[HideInInspector] public List<Fragment> afterFragments;

	public void fillLists() {
		foreach (Fragment f in fragments) {
			switch (f.addingInstant) {
				case CraftingController.Instant.Before:
					beforeFragments.Add(f);
					break;
				case CraftingController.Instant.While:
					whileFragments.Add(f);
					break;
				case CraftingController.Instant.Pause:
					pauseFragments.Add(f);
					break;
				case CraftingController.Instant.After:
					afterFragments.Add(f);
					break;
			}
		}
	}

}

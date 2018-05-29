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


	public void OnEnable() {
		fillLists();
	}

	public void fillLists() {
		foreach (Fragment f in fragments) {
			switch (f.addingInstant) {
				case CraftingController.AddingInstant.Before:
					beforeFragments.Add(f);
					break;
				case CraftingController.AddingInstant.While:
					whileFragments.Add(f);
					break;
				case CraftingController.AddingInstant.Pause:
					pauseFragments.Add(f);
					break;
			}
		}
	}

}

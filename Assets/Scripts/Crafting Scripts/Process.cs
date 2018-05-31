using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Process {

	public CraftingController.AddingInstant instantAdded;
	public CraftingController.frag frag;
	public float timeAdded;
	public int volume;

	public Process(CraftingController.AddingInstant i, CraftingController.frag f, float t) {
		instantAdded = i;
		frag = f;
		timeAdded = t;
		volume = 1;
	}

	public Process(CraftingController.AddingInstant i, CraftingController.frag f, float t, int v) {
		instantAdded = i;
		frag = f;
		timeAdded = t;
		volume = v;
	}
}

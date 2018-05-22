using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step {
	[Space(10)]
	public Condition[] conditions;
	[TextArea] public string description;

	public ChangeVarEffect[] vars;
	public AddQuestEffect[] quests;

	private bool done;

	public void initialize() {
		done = false;
	}

	public void score() {
		done = true;
	}

	public bool isDone() {
		foreach (Condition c in conditions) {
			if (!c.isTrue()) {
				return false;
			}
		}

		return true;
	}

	public void applyEffects() {

		foreach (ChangeVarEffect v in vars) {
			v.apply();
		}
		
		foreach (AddQuestEffect q in quests) {
			q.apply();
		}
	}

	public bool canBePassedTo() {

		return true;
	}

}

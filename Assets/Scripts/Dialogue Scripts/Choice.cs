using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Choice {

	[Space(10)]
	public string text;
	public string[] keywords;

	public Condition[] conditions;

	public ChangeVarEffect[] vars;
	public AddQuestEffect[] quests;
	public EnableExitEffect[] exits;

	public Dialogue dialogue;

	[HideInInspector] public bool able;

	

	public bool hasKeyword(string k) {
		foreach (string key in keywords) {
			if (key == k) { return true; }
		}
		return false;
	}

	public bool hasCondition() {
		if (conditions != null) {
			return true;
		}
		return false;
	}

	public bool isAble() {
		foreach (Condition c in conditions) {
			if (!c.isTrue()) {
				return false;
			}
		}
		return true;
	}

	public void setGlobalVariables() {
		foreach (Condition c in conditions) {
			c.createGlobalVariable();
		}
	}

	public void applyEffects() {
		Debug.Log("Aplicando Efectos de Opción.");

		foreach (ChangeVarEffect v in vars) {
			v.apply();
		}

		foreach (AddQuestEffect q in quests) {
			q.apply();
		}

		foreach (EnableExitEffect e in exits) {
			e.apply();
		}
	}
}

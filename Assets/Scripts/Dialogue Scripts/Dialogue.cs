using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	[Space(10)]
	public string[] keywords;
	[TextArea] public string[] text;
	public GlobalVariable[] variables;

	[Space(5)]
	public Dialogue[] choices;

	public int getNumberOfChoices() {
		int count = 0;
		if (choices == null) {
			return count;
		}
		for (int i = 0; i<choices.Length; i++) {
			count++;
		}
		return count;
	}

	public bool isLeaf() {
		if (getNumberOfChoices() == 0) {
			return true;
		}
		return false;
	}

	public bool isLineal() {
		if (getNumberOfChoices() == 1) {
			return true;
		}
		return false;
	}

	public bool hasChoices() {
		if (getNumberOfChoices() > 1) {
			return true;
		}
		return false;
	}

	public bool hasKeyword(string k) {
		foreach (string key in keywords) {
			if (key == k) { return true; }
		}
		return false;
	}

	public Dialogue getChoiceWithKeyword(string keyword) {
		foreach (Dialogue d in choices) {
			if (d.hasKeyword(keyword)) {
				return d;
			}
		}
		return null;
	}

	public void applyEffects() {
		foreach (GlobalVariable v in variables) {
			if (GlobalVariables.ContainsVariable(v.name)) {
				GlobalVariables.SetValue(v.name, v.value);
				Debug.Log("'" + v.name + "' = " + v.value + ".");
			}
			else {
				GlobalVariables.AddNewAs(v.name, v.value);
				Debug.Log("new: '" + v.name + "' = " + v.value + ".");
			}
		}
	}
}
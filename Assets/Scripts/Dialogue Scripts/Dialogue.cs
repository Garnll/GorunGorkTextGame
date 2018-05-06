using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	[Space(10)]
	[TextArea] public string[] text;

	public enum NarratorType { narrator, character}
	public NarratorType narrator;

	[System.NonSerialized]
	public GlobalVariable[] variables;

	[Space(5)]
	public Choice[] choices;

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

	public Dialogue getDialogueFromChoice(string keyword) {
		for (int i=0; i<choices.Length; i++) {
			for (int j=0; j<choices[i].keywords.Length; j++) {
				if (choices[i].keywords[j] == keyword) {
					return choices[i].dialogue;
				}
			}
		}
		return null;
	}

	public Choice getChoice(string keyword) {
		for (int i = 0; i < choices.Length; i++) {
			for (int j = 0; j < choices[i].keywords.Length; j++) {
				if (choices[i].keywords[j] == keyword) {
					return choices[i];
				}
			}
		}
		return null;
	}

	public void applyChoosenEffects(Choice c) {
		c.applyEffects();
	}

	public void applyEffects() {
		if (variables != null) {
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

	public string getText() {
		int r = Random.Range(0, text.Length - 1);
		string temp = " ";
		try {

			if (text[r] == null) {
				temp = "<i>No hay texto.</i>";
			}
			else {
				temp = text[r];
			}
		} catch {
			Debug.Log("No hay texto para mostrar.");
		}
		return temp + "\n";
	}

	public void setGlobalVariables() {
		foreach (Choice c in choices) {
			c.setGlobalVariables();
		}
	}
}
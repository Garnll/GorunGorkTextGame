using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice {

	[Space(10)]
	public string text;
	public string[] keywords;

	public Condition[] conditions;

	public Dialogue dialogue;

	public bool able;

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
}

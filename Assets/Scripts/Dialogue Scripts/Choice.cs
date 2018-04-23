using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice {

	[Space(10)]
	public string text;
	public string[] keywords;

	public Dialogue dialogue;

	public bool hasKeyword(string k) {
		foreach (string key in keywords) {
			if (key == k) { return true; }
		}
		return false;
	}
}

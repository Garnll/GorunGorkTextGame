using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	[Space(10)]
	public string[] keywords;
	[TextArea] public string[] text;
	public Dialogue[] choices;

	

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainBuff {

	public enum MainBuffType { Health, Strength, Intelligence, Resistance, Dexterity, Infravision, Hypervision, Pods}

	public MainBuffType type;
	public float magnitude;
}

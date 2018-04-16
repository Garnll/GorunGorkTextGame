using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntBuff {

	public enum IntBuffType { None, Health, Strength, Intelligence, Resistance, Dexterity, Infravision, Hypervision}

	public IntBuffType type;
	public int buffMagnitude;
}

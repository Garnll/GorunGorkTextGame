using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatBuff {

	public enum FloatBuffType { Crit, Cooldown, HealthRegen, TurnRegen, Evasion }

	public FloatBuffType type;
	public float magnitude;
}

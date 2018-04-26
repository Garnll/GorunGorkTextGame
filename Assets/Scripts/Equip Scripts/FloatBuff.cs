using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OtherBuff {

	public enum OtherBuffType { Crit, Cooldown, HealthRegen, TurnRegen, Evasion }

	public OtherBuffType type;
	public float magnitude;
}

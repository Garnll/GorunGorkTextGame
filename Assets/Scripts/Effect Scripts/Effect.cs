using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect {
	public enum EffectType { ChangeVar, AddQuest}
	[HideInInspector] public EffectType type;
}

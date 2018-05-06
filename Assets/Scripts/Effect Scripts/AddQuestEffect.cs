using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AddQuestEffect : Effect {

	public delegate void OnQuestAdd(Quest quest);
	public static OnQuestAdd addQuest;

	public Quest quest;

	private void Awake() {
		type = Effect.EffectType.AddQuest;
	}

	public void apply() {
		Debug.Log("Añadiendo Quest " + quest.questName + ".");
		addQuest(quest);
	}

}

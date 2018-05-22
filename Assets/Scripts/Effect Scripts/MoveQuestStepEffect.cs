using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveQuestStepEffect : Effect {

	public Quest quest;

	public void apply() {
		quest.pass();
	}
}

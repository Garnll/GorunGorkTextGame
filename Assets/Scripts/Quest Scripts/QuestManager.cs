using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public List<Quest> quests;

	private void Awake() {
		AddQuestEffect.addQuest += addQuest;
	}

	public void addQuest(Quest q) {
		quests.Add(q);
		quests[quests.Count - 1].initialize();
		Debug.Log("Añadida la misión " + q.questName + ".");
	}

	public void updateQuests() {
		foreach (Quest q in quests) {
			q.update();
		}
	}
}

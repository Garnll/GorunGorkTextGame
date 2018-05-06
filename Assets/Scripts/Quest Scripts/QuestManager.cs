using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public List<Quest> quests;

	private void Start() {
		AddQuestEffect.addQuest += addQuest;
	}

	public void addQuest(Quest q) {
		quests.Add(q);
	}
}

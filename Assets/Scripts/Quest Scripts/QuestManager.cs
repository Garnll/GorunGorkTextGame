using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour {

	public List<Quest> quests;
	public TextMeshProUGUI bar;
	public TextMeshProUGUI text;

	

	private void Awake() {
		AddQuestEffect.addQuest += addQuest;
	}

	public void init() {
		GlobalVariables.AddNewAs("diario", 0);
		bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0);
		text.text = "";
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

		if (GlobalVariables.ContainsVariable("diario")) {
			if (GlobalVariables.GetValueOf("diario") == 1) {
				bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 256);
				text.text = "<b>Diario</b>\n" + getQuestLog();
			} else {
				bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0);
				text.text = "";
			}
		} else {
			bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0);
			text.text = "";
		}

	}

	public string getQuestLog() {
		if (quests.Count == 0) {
			return "<i>No hay misiones.</i>";
		}
		Quest majorQuest = quests[0];

		foreach (Quest q in quests) {
			if (q.priority > majorQuest.priority && !q.isDone()) {
				majorQuest = q;
			}
		}

		if (majorQuest.isDone()) {

			foreach (Quest q in quests) {
				if (!q.isDone()) {
					majorQuest = q;
					break;
				}
			}
			
			if (majorQuest.isDone())
				return "<i>No hay misiones.</i>";
		}

		return "---\n" + majorQuest.questName + ":\n"
			+ "<i>" + majorQuest.description + "</i>\n"
			+ "[" + (majorQuest.getIndexOfCurrentStep() + 1 ) + "/" + majorQuest.getNumberOfSteps() + "] \n"
			+ majorQuest.currentStep.description;
	}
}

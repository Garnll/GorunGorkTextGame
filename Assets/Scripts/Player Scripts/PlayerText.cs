﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerText : MonoBehaviour {

	public PlayerManager player;
	public PlayerCharacteristics characteristics;
	public TextMeshProUGUI text;
	public Gradient healthGradient;

	public Color inactiveColor;
	public Color activeColor;

	
	public void updateText() {
		text.color = inactiveColor;
		text.text = "<b>- + -</b>";
		string t = "";

		if (GlobalVariables.ContainsVariable("om") && GlobalVariables.GetValueOf("om") == 1) {
			text.color = activeColor;
			text.text = "";
			if (player.playerName != "jugador")
				t += player.playerName + " | ";
			if (player.gender == "macho") {
				if (characteristics.playerRace.raceName != "ninguna") {
					t += characteristics.playerRace.raceName + " ";
				}
			} else {
				switch (characteristics.playerRace.raceName) {
					case "Búho":
						t += "Búha ";
						break;
					case "Toro":
						t += "Vaca ";
						break;
					case "Oso":
						t += "Osa ";
						break;
					case "Conejo":
						t += "Coneja ";
						break;
				}
			}

			if (characteristics.playerJob.jobName != "ninguno") {
				t += characteristics.playerJob.jobName + " ";
				t += "Nivel " + player.playerLevel + " ";
			}

			t += "--";

			text.text = t;

			if (player.currentState.stateName != "estándar")
				text.text += " [" + player.currentState.stateName + "]";

			int health = Mathf.RoundToInt(player.currentHealth / player.MaxHealth * 100);
			if (health > 100) {
				health = 100;
			}

			string code = ColorUtility.ToHtmlStringRGB(healthGradient.Evaluate(player.currentHealth / player.MaxHealth));
			text.text += "<color=#" + code + "> Salud " + health + "%</color>";

		}

		
	}
}

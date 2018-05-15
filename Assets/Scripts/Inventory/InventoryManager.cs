using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

/// <summary>
/// Maneja los objetos que están en el inventario y los muestra en una pantalla secundaria.
/// </summary>
public class InventoryManager : MonoBehaviour {

	public PlayerManager player;
	public PlayerRoomNavigation roomNav;

    [SerializeField] public List<InteractableObject> nounsInInventory = new List<InteractableObject>();
    private int lemons = 0;

    public TextMeshProUGUI text;
	public Image border;
	public TextMeshProUGUI barra;

    private void Start()
    {
        DisplayInventory();
    }

    public void AddLemons(int amount)
    {
        lemons += amount;
        DisplayInventory();
    }

    public void RemoveLemons(int amount)
    {
        lemons -= amount;
        if (lemons < 0)
        {
            lemons = 0;
        }
        DisplayInventory();
    }

    /// <summary>
    /// Actualiza el inventario en el display secundario.
    /// </summary>
    public void DisplayInventory()
    {
		string textToDisplay = "";
		if (roomNav.currentPosition.z != 8) {
			barra.color = new Color(barra.color.r, barra.color.g, barra.color.b, 256);
			border.color = new Color(border.color.r, border.color.g, border.color.b, 0);
			textToDisplay = "<b>Inventario</b>";
			textToDisplay += "\nCapacidad [" + player.characteristics.usedPods + "/" + player.characteristics.currentMaxPods + "]";
			if (lemons > 0) { textToDisplay += "\n- " + lemons.ToString() + " Limones."; }

			string newNounToDisplay;
			for (int i = 0; i < nounsInInventory.Count; i++) {
				newNounToDisplay = nounsInInventory[i].nouns[0];
				if (nounsInInventory[i].nounGender == InteractableObject.WordGender.male)
					newNounToDisplay = "\n- Un " + nounsInInventory[i].nouns[0];
				else
					newNounToDisplay = "\n- Una " + nounsInInventory[i].nouns[0];

				if (nounsInInventory[i].GetType() == typeof(Pocket)) {
					Pocket p = (Pocket) nounsInInventory[i];
					Debug.Log(p.usage);
					newNounToDisplay += " [" + p.usage + "/" + p.capacity + "]";
				}

				newNounToDisplay += ".";

				textToDisplay += newNounToDisplay;
			}
		} else {
			border.color = new Color(border.color.r, border.color.g, border.color.b, 0);
			barra.color = new Color(barra.color.r, barra.color.g, barra.color.b, 0);
		}

        text.text = textToDisplay;
    }

    public bool DisplayInventory(CombatController combatController, int page)
    {
        int totalPages = (int)Math.Round((nounsInInventory.Count / 3d), 0, MidpointRounding.AwayFromZero);

        if (totalPages == 0)
        {
            totalPages = 1;
        }

        if (page > totalPages || page < totalPages)
        {
            return false;
        }

        List<string> optionsText = new List<string>();

        string textToDisplay = "Inventario Página "+ page.ToString() + "/" + totalPages.ToString() +": ";



        string newNounToDisplay;
        for (int i = totalPages*(page-1); i < nounsInInventory.Count; i++)
        {
            int display = i - (totalPages * (page - 1));

            newNounToDisplay = nounsInInventory[i].nouns[0];

            newNounToDisplay = "\n[" + display + "] " + TextConverter.MakeFirstLetterUpper(nounsInInventory[i].nouns[0]);


            textToDisplay += newNounToDisplay;
        }

        if (page == 1)
        {
            optionsText.Add("<color=#A9A9A9>[<] Anterior Página </color>");
        }
        else
        {
            optionsText.Add("[<] Anterior Página");
        }

        if (page == totalPages)
        {
            optionsText.Add("<color=#A9A9A9>[>] Siguiente Página </color>");
        }
        else
        {
            optionsText.Add("[>] Siguiente Página ");
        }

        optionsText.Add("[S] Salir del Inventario ");

        combatController.SetPlayerHabilities(textToDisplay);
        combatController.SetPlayerInventoryOptions(string.Join("\n", optionsText.ToArray()));
        return true;
    }

	public bool hasPockets() {
		for (int i=0; i<nounsInInventory.Count; i++) {
			if (nounsInInventory[i].GetType() == typeof(Pocket)) {
				return true;
			}
		}
		return false;
	}

	public List<Pocket> getPockets() {
		List<Pocket> pockets = new List<Pocket>();
		foreach (InteractableObject i in nounsInInventory) {
			if (i.GetType() == typeof(Pocket)) {
				pockets.Add(i as Pocket);
			}
		}

		return pockets;
	}

	public InteractableObject tryOpen(string[] words) {

		if (words[0] == "" || words[0] == " ") {
			return null;
		}


		InteractableObject temp = null;

		for (int i=0; i<nounsInInventory.Count; i++) {
			if (nounsInInventory[i].isOpenable) {
				for (int j=0; j<nounsInInventory[i].nouns.Length; j++) {
						if (nounsInInventory[i].nouns[j] == words[words.Length - 1]) {
							temp = nounsInInventory[i];
						}
				}
			}
		}

		return temp;
	}
}

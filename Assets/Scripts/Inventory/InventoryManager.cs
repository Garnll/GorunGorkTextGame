using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Maneja los objetos que están en el inventario y los muestra en una pantalla secundaria.
/// </summary>
public class InventoryManager : MonoBehaviour {

    [SerializeField] public List<InteractableObject> nounsInInventory = new List<InteractableObject>();

    public TextMeshProUGUI text;

    private void Start()
    {
        DisplayInventory();
    }

    /// <summary>
    /// Actualiza el inventario en el display secundario.
    /// </summary>
    public void DisplayInventory()
    {
        string textToDisplay = "Mochila: ";
        string newNounToDisplay;
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            newNounToDisplay = nounsInInventory[i].noun;

            if (nounsInInventory[i].nounGender == InteractableObject.WordGender.male)
                newNounToDisplay = "\n-Un " + nounsInInventory[i].noun;
            else
                newNounToDisplay = "\n-Una " + nounsInInventory[i].noun;
            

            textToDisplay += newNounToDisplay;
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

        if (page > totalPages && page < totalPages)
        {
            return false;
        }

        List<string> optionsText = new List<string>();

        string textToDisplay = "Inventario página "+ page.ToString() + "/" + totalPages.ToString() +": ";



        string newNounToDisplay;
        for (int i = totalPages*(page-1); i < nounsInInventory.Count; i++)
        {
            int display = i - (totalPages * (page - 1));

            newNounToDisplay = nounsInInventory[i].noun;

            newNounToDisplay = "\n[" + display + "] " + TextConverter.MakeFirstLetterUpper(nounsInInventory[i].noun);


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

}

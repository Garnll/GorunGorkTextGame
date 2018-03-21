using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        string textToDisplay = "Backpack: ";
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

}

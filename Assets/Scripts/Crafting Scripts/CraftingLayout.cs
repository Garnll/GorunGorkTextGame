using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingLayout : MonoBehaviour {

	[SerializeField] public TextMeshProUGUI	title;

	[SerializeField] public TextMeshProUGUI	ingredientsList;
	[SerializeField] public TextMeshProUGUI	pages;
	[SerializeField] public TextMeshProUGUI	ingredientCommands;

	[SerializeField] public TextMeshProUGUI	ingredientsUsed;
	[SerializeField] public TextMeshProUGUI	time;
	[SerializeField] public Image			picture;
	[SerializeField] public TextMeshProUGUI	processDescription;
	[SerializeField] public TextMeshProUGUI	processCommands;


	[SerializeField] public TextMeshProUGUI recipeTitle;
	[SerializeField] public TextMeshProUGUI	recipes;
	[SerializeField] public TextMeshProUGUI	recipeAnotation;
	[SerializeField] public TextMeshProUGUI	recipeCommands;

	[SerializeField] public TextMeshProUGUI	exit;

}

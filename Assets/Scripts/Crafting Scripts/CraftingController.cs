using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour {

	public GameController controller;
	public InventoryManager inventory;
	public List<InteractableObject> playerIngredients;

	public enum Instant { Before, While, Pause, After }

	public List<Recipe> recipes;                        //Todas las recetas del juego.
	[HideInInspector] public List<Recipe> recipesKnown; //Recetas conocidas por el jugador.

	public float craftingTime;                          //El tiempo que lleva crafteando.
	public bool isCrafting;                             //Si está crafteando o no.
	public Instant craftingInstant;
	public bool alreadyStarted;

	public List<InteractableObject> ingredientsIn;

	public void initialize() {
		alreadyStarted = false;
		craftingInstant = Instant.Before;
		isCrafting = false;
		craftingTime = 0;
	}

	public void fillIngredients() {
		foreach (Pocket p in inventory.nounsInInventory) {
			foreach (InteractableObject i in p.ingredients) {
				playerIngredients.Add(i);
			}
		}
	}

	public void fillRecipes() {
		foreach (RecipeBook b in inventory.nounsInInventory) {
			foreach (Recipe r in b.recipes) {
				recipesKnown.Add(r);
			}
		}
	}

	public bool checkFor(InteractableObject ingredient) {
		for (int i = 0; i < playerIngredients.Count; i++) {
			if (playerIngredients[i] == ingredient) {
				return true;
			}
		}
		return false;
	}

	public bool checkFor(InteractableObject ingredient, int volume) {
		int count = 0;
		for (int i = 0; i < playerIngredients.Count; i++) {
			if (playerIngredients[i] == ingredient) {
				count++;
			}
		}
		if (count >= volume) {
			return true;
		}

		return false;
	}

	public void useIngredient(InteractableObject ingredient) {
		if (checkFor(ingredient)) {
			ingredientsIn.Add(ingredient);
			playerIngredients.Remove(ingredient);
		}
	}

	public void useIngredient(InteractableObject ingredient, int volume) {
		if (checkFor(ingredient, volume)) {
			for (int i = 0; i < volume; i++) {
				ingredientsIn.Add(ingredient);
				playerIngredients.Remove(ingredient);
			}
		}
	}

	public void play() {
		craftingInstant = Instant.While;
		alreadyStarted = true;
	}

	public void pause() {
		craftingInstant = Instant.Pause;
	}

	public void stop() {
		craftingInstant = Instant.After;
	}

}

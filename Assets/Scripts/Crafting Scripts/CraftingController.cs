using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour {

	public GameController controller;
	public InventoryManager inventory;
	public List<InteractableObject> playerIngredients;

	public enum Instant { Before, While, Pause, After }
	public enum AddingInstant { Before, While, Pause }

	public List<Recipe> recipes;                        //Todas las recetas del juego.
	[HideInInspector] public List<Recipe> recipesKnown; //Recetas conocidas por el jugador.

	public float craftingTime;                          //El tiempo que lleva crafteando.
	public bool isCrafting;                             //Si está crafteando o no.
	public Instant currentInstant;
	public bool alreadyStarted;

	public List<InteractableObject> ingredientsIn;

	public void initialize() {
		alreadyStarted = false;
		currentInstant = Instant.Before;
		isCrafting = false;
		craftingTime = 0;
	}

	//Llena playerIngredients con todos los objetos que han sido guardados en los Pockets. 
	public void fillIngredients() {
		foreach (Pocket p in inventory.nounsInInventory) {
			foreach (InteractableObject i in p.ingredients) {
				playerIngredients.Add(i);
			}
		}
	}

	//Llena recipesKnown con todas las recetas entre los diferentes libros en el inventario.
	public void fillRecipes() {
		foreach (RecipeBook b in inventory.nounsInInventory) {
			foreach (Recipe r in b.recipes) {
				recipesKnown.Add(r);
			}
		}
	}

	//Devuelve la cantidad de un ingrediente en una lista.
	public int getVolume(InteractableObject ingredient, List<InteractableObject> list) {
		int count = 0;
		if (checkFor(ingredient, list)) {
			foreach (InteractableObject i in list) {
				if (i == ingredient) {
					count++;
				}
			}
		}
		return count;
	}

	//Revisa que un ingrediente se encuentre en la lista.
	public bool checkFor(InteractableObject ingredient, List<InteractableObject> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i] == ingredient) {
				return true;
			}
		}
		return false;
	}

	//Revisa que haya al menos 'volume' ingredientes en la lista.
	public bool checkFor(InteractableObject ingredient, int volume, List<InteractableObject> list) {
		int count = 0;
		for (int i = 0; i < list.Count; i++) {
			if (list[i] == ingredient) {
				count++;
			}
		}
		if (count >= volume) {
			return true;
		}

		return false;
	}

	//Pasa un ingrediente de la lista de todos los ingredientes al proceso.
	public void useIngredient(InteractableObject ingredient) {
		if (checkFor(ingredient, playerIngredients)) {
			ingredientsIn.Add(ingredient);
			playerIngredients.Remove(ingredient);
			removeIngredient(ingredient, 1);
		}
	}

	//Pasa un ingrediente en cierta cantidad de la lista de todos los ingredientes al proceso.
	public void useIngredient(InteractableObject ingredient, int volume) {
		if (checkFor(ingredient, volume, playerIngredients)) {
			for (int i = 0; i < volume; i++) {
				ingredientsIn.Add(ingredient);
				playerIngredients.Remove(ingredient);
				removeIngredient(ingredient, volume);
			}
		}
	}

	//Remueve un ingrediente en cierta cantidad del inventario del jugador.
	public void removeIngredient(InteractableObject ingredient, int times) {
		int t = 0;
		foreach (Pocket p in inventory.nounsInInventory) {
			if (p.have(ingredient)) {
				int volume = p.getVolumeOf(ingredient);
				if (times <= volume) {
					for (int i = 0; i < p.ingredients.Count; i++) {
						if (p.ingredients[i] == ingredient && t < times) {
							p.ingredients.RemoveAt(i);
							t++;

							if (t == times && times == volume) {
								p.usage--;
							}

							if (t == times) {
								return;
							}
						}
					}
				}
			}
		}
	}

	//Devuelve un bloque de texto con los ingredientes y su cantidad en una lista.
	public string getTextIn(List<InteractableObject> list) {
		string text = "";

		List<InteractableObject> tempList = new List<InteractableObject>();
		foreach (InteractableObject i in list) {
			if (!checkFor(i, tempList)) {
				tempList.Add(i);
				int volume = getVolume(i, list);
				text += "- " + i.objectName + " x" + volume + "\n";
			}
		}
		return text;
	}

	public void play() {
		currentInstant = Instant.While;
		alreadyStarted = true;
	}

	public void pause() {
		currentInstant = Instant.Pause;
	}

	public void stop() {
		currentInstant = Instant.After;
	}

}

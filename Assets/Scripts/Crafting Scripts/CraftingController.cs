using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour {

	public GameObject craftLayout;
	public RectTransform contentContainer;

	private CraftingLayout layout;

	[Space(20)]
	public GameController controller;
	public InventoryManager inventory;
	public List<InteractableObject> playerIngredients;

	public Recipient currentRecipient;

	public enum Instant { Before, While, Pause, After }
	public enum AddingInstant { Before, While, Pause }

	public List<Recipe> recipes;                        //Todas las recetas del juego.
	[HideInInspector] public List<Recipe> recipesKnown; //Recetas conocidas por el jugador.

	public float craftingTime;                          //El tiempo que lleva crafteando.
	public bool isCrafting;                             //Si está crafteando o no.
	public Instant currentInstant;
	public bool alreadyStarted;

	public int totalPages;
	public int currentPage;
	public int slots;
	public int slotsUsed;

	private string ableColor_forText = "FFEBC6FF";
	private string disableColor_forText = "EEDAAD67";

	private string ableColor_forPicture = "F9A08B9D";
	private string disableColor_forPicture = "F9A08B3F";

	private string ableColor_forCommand = "F9CC88AA";
	private string disableColor_forCommand = "F9BE884F";

	public bool canPlay;
	public bool canPause;
	public bool canStop;

	public List<InteractableObject> ingredientsIn;

	public void initialize(Recipient r) {
		alreadyStarted = false;
		currentInstant = Instant.Before;
		isCrafting = false;
		craftingTime = 0;
		slotsUsed = 0;
		totalPages = getIngredientsPages();
		currentPage = 1;
		currentRecipient = r;

		canPlay = true;
		canPause = false;
		canStop = false;

		controller.NullCurrentDisplay();
		GameObject newCraft = Instantiate(craftLayout, contentContainer);
		layout = newCraft.GetComponent<CraftingLayout>();
	}

	//Llena playerIngredients con todos los objetos que han sido guardados en los Pockets. 
	public void fillPlayerIngredients() {
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
	public int getVolumeOf(InteractableObject ingredient, List<InteractableObject> list) {
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
	public void addIngredient(InteractableObject ingredient) {
		if (checkFor(ingredient, playerIngredients) && slotsUsed < slots) {
			ingredientsIn.Add(ingredient);
			playerIngredients.Remove(ingredient);
			slotsUsed++;
		}
	}

	//Pasa un ingrediente en cierta cantidad de la lista de todos los ingredientes al proceso.
	public void addIngredient(InteractableObject ingredient, int volume) {
		if (checkFor(ingredient, volume, playerIngredients) && slotsUsed < slots) {
			for (int i = 0; i < volume; i++) {
				ingredientsIn.Add(ingredient);
				playerIngredients.Remove(ingredient);
			}
			slotsUsed++;
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
				int volume = getVolumeOf(i, list);
				text += "- " + i.objectName + " x" + volume + "\n";
			}
		}
		return text;
	}

	public void play() {
		currentInstant = Instant.While;
		alreadyStarted = true;
		consumeIngredients();

		canPlay = false;
		canPause = true;
		canStop = true;
	}

	public void pause() {
		currentInstant = Instant.Pause;
		canPause = false;
		canPlay = true;
		canStop = true;
	}

	public void stop() {
		currentInstant = Instant.After;
		alreadyStarted = false;
		canStop = false;
		canPlay = true;
		canPause = false;
	}


	public void initLayout() {
		layout.ingredientsList.text = getTextIn(playerIngredients);
		layout.ingredientsUsed.text = getSlotsText();
		layout.recipes.text = getRecipesText();
		layout.title.text = getTitle();
	}

	public int getIngredientsPages() {
		int c = 1;
		List<InteractableObject> tempList = new List<InteractableObject>();
		foreach (InteractableObject i in playerIngredients) {
			if (!checkFor(i, tempList)) {
				tempList.Add(i);
				if (tempList.Count > 10) {
					c++;
					tempList.Clear();
				}
			}
		}

		return c;
	}

	public void setSlots() {
		slots = currentRecipient.slots;
	}

	public string getSlotsText() {
		string s = "";
		for (int i = 0; i < slots; i++) {
			s += "- ";
			if (ingredientsIn[i] != null) {
				s += ingredientsIn[i].objectName + " x" + getVolumeOf(ingredientsIn[i], ingredientsIn) + ".\n";
			}
			else {
				s += "\n";
			}
		}
		return s;
	}

	public string getRecipesText() {
		string s = "";
		foreach (Recipe r in recipesKnown) {
			s += "> " + r.recipeName + ".\n";
		}
		return s;
	}

	public void consumeIngredients() {
		foreach (InteractableObject i in ingredientsIn) {
			i.consumed = true;
			removeIngredient(i, 1);
		}
	}

	public string getTitle() {
		return currentRecipient + "<b>[" + currentRecipient.slots + "]</b>";
	}
}

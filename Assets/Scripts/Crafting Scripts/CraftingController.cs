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

	public float currentTime;
	public Recipe currentRecipe;
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

	public int resultState;

	public bool canPlay;
	public bool canPause;
	public bool canStop;
	public bool canExit;

	private string color_forTitle = "F9A08BFF";
	private string color_forRecipeTitle = "F9AA98FF";

	private string ableColor_forText = "FFEBC6FF";
	private string disableColor_forText = "EEDAAD67";

	private string ableColor_forPicture = "F9A08B9D";
	private string disableColor_forPicture = "F9A08B3F";

	private string ableColor_forCommand = "F9CC88AA";
	private string disableColor_forCommand = "F9BE884F";

	string[] addCommands = { "añadir", "agregar", "+", "echar", "poner", "a" };
	string[] removeCommands = { "quitar", "remover", "-", "retirar" };
	string[] backCommands = { "anterior", "<" };
	string[] nextCommands = { "siguiente", ">" };

	string[] playCommands = { "e", "encender", "encender." };
	string[] pauseCommands = { "p", "pausar", "pausar." };
	string[] stopCommands = { "d", "detener", "parar", "detener.", "parar." };

	string[] readCommands = { "leer", "ver", "revisar", "examinar", "mirar" };
	string[] returnCommands = { "v", "volver", "volver." };
	string[] reclaimCommands = { "r", "reclamar", "reclamar." };

	string[] exitCommands = { "x", "salir", "salir." };

	string[] one = { "1", "un", "uno", "x1" };
	string[] two = { "2", "dos", "x2" };
	string[] three = { "3", "tres", "x3" };
	string[] four = { "4", "cuatro", "x4" };
	string[] five = { "5", "cinco", "x5" };

	public enum InputType {
		addCommand, removeCommand, backCommand, nextCommand,
		playCommand, pauseCommand, stopCommand, returnCommand, reclaimCommand, exitCommand,
		readCommand, unrecognized, schrodinger
	}

	public enum SubType { mono, bi, natural, standard, unrecognized }
	//mono		-> p 
	//bi		-> + p 
	//natural	-> + # p 
	//standard	-> + p . p . p x # 

	public class frag {
		public InteractableObject ingredient;
		public bool consumed;

		public frag(InteractableObject i) {
			ingredient = i;
		}
	}

	public List<frag> ingredientsIn;

	public void initialize(Recipient r) {
		GameState.Instance.ChangeCurrentState(GameState.GameStates.crafting);
		fillPlayerIngredients();
		fillRecipes();

		alreadyStarted = false;
		currentInstant = Instant.Before;
		isCrafting = false;
		craftingTime = 0;
		slotsUsed = 0;
		totalPages = getIngredientsPages();
		currentPage = 1;
		currentRecipient = r;
		currentTime = 0;

		canPlay = true;
		canPause = false;
		canStop = false;
		canExit = true;

		ingredientsIn = new List<frag>();

		resultState = 0; //0-> Recetas, 1->Descripción, 2->Resultado.

		GameObject newCraft = Instantiate(craftLayout, contentContainer);
		layout = newCraft.GetComponent<CraftingLayout>();
		initLayout();
	}

	//Llena playerIngredients con todos los objetos que han sido guardados en los Pockets. 
	public void fillPlayerIngredients() {
		foreach (InteractableObject p in inventory.nounsInInventory) {
			if (p.GetType() == typeof(Pocket)) {
				Pocket pocket = p as Pocket;
				foreach (InteractableObject i in pocket.ingredients) {
					playerIngredients.Add(i);
				}
			}
		}

		foreach (InteractableObject i in inventory.nounsInInventory) {
			if (i.isIngredient) {
				playerIngredients.Add(i);
			}
		}
	}

	//Llena recipesKnown con todas las recetas entre los diferentes libros en el inventario.
	public void fillRecipes() {
		foreach (InteractableObject b in inventory.nounsInInventory) {
			if (b.GetType() == typeof(RecipeBook)) {
				RecipeBook book = b as RecipeBook;
				foreach (Recipe r in book.recipes) {
					recipesKnown.Add(r);
				}
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

	public int getVolumeOf(InteractableObject ingredient, List<frag> list) {
		int count = 0;
		if (checkFor(ingredient, list)) {
			foreach (frag i in list) {
				if (i.ingredient == ingredient) {
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

	public bool checkFor(InteractableObject ingredient, List<frag> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i].ingredient == ingredient) {
				return true;
			}
		}
		return false;
	}

	public bool checkFor(frag f, List<frag> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i] == f) {
				return true;
			}
		}
		return false;
	}

	public bool checkFor(string s, List<frag> list) {
		for (int i= 0; i < list.Count; i++) {
			if (list[i].ingredient.objectName == s) {
				return true;
			}
			for (int j = 0; j < list[i].ingredient.nouns.Length; j++) {
				if (s == list[i].ingredient.nouns[j]) {
					return true;
				}
			}
		}
		return false;
	}

	public bool checkFor(string s, List<Recipe> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i].recipeName == s) {
				return true;
			}
		}
		return false;
	}

	public bool checkFor(string s, List<InteractableObject> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list[i].objectName == s) {
				return true;
			}
			for (int j = 0; j < list[i].nouns.Length; j++) {
				if (s == list[i].nouns[j]) {
					return true;
				}
			}
		}
		return false;
	}

	public Recipe getRecipeWith(string s) {
		foreach (Recipe r in recipesKnown) {
			if (r.recipeName == s) {
				return r;
			}
		}
		return null;
	}

	public InteractableObject getIngredient(string s) {
		for (int i = 0; i < playerIngredients.Count; i++) {
			if (playerIngredients[i].objectName == s) {
				return playerIngredients[i];
			}
			for (int j= 0; j < playerIngredients[i].nouns.Length; j++) {
				if (s == playerIngredients[i].nouns[j]) {
					return playerIngredients[i];
				}
			}
		}
		return null;
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
			ingredientsIn.Add(new frag(ingredient));
			playerIngredients.Remove(ingredient);
			slotsUsed++;
		}
	}

	//Pasa un ingrediente en cierta cantidad de la lista de todos los ingredientes al proceso.
	public void addIngredient(InteractableObject ingredient, int volume) {
		if (checkFor(ingredient, volume, playerIngredients) && slotsUsed < slots) {
			for (int i = 0; i < volume; i++) {
				ingredientsIn.Add(new frag(ingredient));
				playerIngredients.Remove(ingredient);
			}
			slotsUsed++;
		}
	}

	public void removeIngredient(string s) {
		foreach (frag f in ingredientsIn) {
			if (s == f.ingredient.objectName && f.consumed == false) {
				playerIngredients.Add(f.ingredient);
				ingredientsIn.Remove(f);
			}
		}
	}

	//Remueve un ingrediente en cierta cantidad del inventario del jugador.
	public void destroyIngredient(InteractableObject ingredient, int times) {
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

		canExit = false;

		StartCoroutine("cook");
		refresh();
	}

	public void pause() {
		StopCoroutine("cook");
		currentInstant = Instant.Pause;
		canPause = false;
		canPlay = true;
		canStop = true;

		canExit = false;
		refresh();
	}

	public void stop() {
		StopCoroutine("cook");
		currentTime = 0;
		layout.time.text = currentTime.ToString();
		currentInstant = Instant.After;
		alreadyStarted = false;
		canStop = false;
		canPlay = true;
		canPause = false;

		canExit = true;

		refresh();
	}


	public void initLayout() {
		layout.title.text = getTitle();
		layout.ingredientsList.text = getTextIn(playerIngredients);
		layout.ingredientsUsed.text = getSlotsText();
		setResultDescriptionAsRecipesList();
		layout.pages.text = getPageText();
		layout.processCommands.text = getCommandsText();
		layout.recipeAnotation.text = getRecipeRealization();
		layout.processDescription.text = getProcessState();
		layout.recipeCommands.text = getRecipeCommandsText();
		layout.ingredientCommands.text = getIngredientsCommandsText();

		refresh();
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

		List<frag> f = new List<frag>();
		for (int i= 0; i < ingredientsIn.Count; i++) {
			if (!checkFor(ingredientsIn[i], f)) {
				f.Add(ingredientsIn[i]);
			}
		}

		for (int i = 0; i < slots; i++) {
			s += "- ";
			if (f[i] != null) {


				s += f[i].ingredient.objectName + " x" + getVolumeOf(f[i].ingredient, ingredientsIn) + ".\n";

			}
			else {
				s += "\n";
			}
		}
		return s;
	}

	public string getRecipesList() {

		if (recipesKnown.Count == 0) {
			return "<color=#" + disableColor_forText + "><i>No hay recetas aprendidas.</i></color>";
		}


		string s = "<color=#" + ableColor_forText + ">";
		foreach (Recipe r in recipesKnown) {
			s += "> " + r.recipeName + ".\n";
		}

		s += "</color>";

		if (s == "") {
			s = "<i>No hay recetas aprendidas.</i>";
		}
		return s;
	}

	public void consumeIngredients() {
		foreach (frag i in ingredientsIn) {
			i.consumed = true;
			destroyIngredient(i.ingredient, 1);
		}
	}

	public string getTitle() {
		return currentRecipient.objectName + " <b>[" + currentRecipient.slots + "]</b>";
	}

	public string getPageText() {
		return "<color=#" + disableColor_forText + "><i>Página " + currentPage + " de " + totalPages + ".</i></color>";
	}

	public string getTextOfCommand(bool b, string s) {
		string p = "";
		if (b) {
			p += "<color=#" + ableColor_forCommand + ">";
		}
		else {
			p += "<color=#" + disableColor_forCommand + ">";
		}

		p += "<b>[" + s[0] + "]</b> " + s + "</color>";

		return p;
	}

	public string getCommandsText() {
		string c = "";
		c += getTextOfCommand(canPlay, "Encender") + "\n";
		c += getTextOfCommand(canPause, "Pausar") + "  ";
		c += getTextOfCommand(canStop, "Detener");
		return c;
	}

	public string getRecipeTitle(Recipe r) {
		return "<color=#" + color_forRecipeTitle + ">" + r.recipeName + "</color>";
	}

	public string getRecipeDescription(Recipe r) {
		return "Blablabla.";
	}

	public string getProcessState() {
		switch (currentInstant) {
			case Instant.After:
				return "Detenido.";

			case Instant.Before:
				return "";	

			case Instant.Pause:
				return "Pausado.";

			case Instant.While:
				return "Procesando..";
		}

		return "";
	}

	public string getRecipeRealization() {
		if (currentRecipe == null) {
			return "";
		}

		if (checkRealization(currentRecipe)) {
			return "Realizable.";
		} else {
			return "No realizable.";
		}
	}

	public string getRecipeCommandsText() {
		if (currentRecipe == null) {
			return "";
		} else {
			return "<b>[V]</b> Volver";
		}
	}

	public string getIngredientsCommandsText() {
		string t = "";
		if (currentPage == 1) {
			t += "<color=#" + disableColor_forText + "><b>[A]</b> Anterior</color> \n";
		}
		else {
			t += "<color=#" + ableColor_forText + "><b>[A]</b> Anterior</color> \n";
		}

		if (currentPage == totalPages) {
			t += "<color=#" + disableColor_forText + "><b>[S]</b> Siguiente</color> \n";
		} else {
			t += "<color=#" + ableColor_forText + "><b>[S]</b> Siguiente</color> \n";
		}

		return t;
		
	}

	public void setResultDescriptionAsRecipesList() {
		layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">Recetario</color>";
		layout.recipes.text = getRecipesList();
		currentRecipe = null;
	}

	public void setResultDescriptionAsRecipe(Recipe r) {
		layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">" + r.recipeName + "</color>";
		layout.recipes.text = getRecipeDescription(r);
		currentRecipe = r;
	}

	public bool checkFor(string s, string[] l) {
		foreach (string _s in l) {
			if (s == _s) {
				return true;
			}
		}
		return false;
	}

	public void apply(InputType t) {
		switch (t) {
			case InputType.backCommand:
				break;

			case InputType.nextCommand:
				break;

			case InputType.playCommand:
				if (canPlay) {
					play();
				}
				break;

			case InputType.pauseCommand:
				if (canPause) {
					pause();
				}
				break;

			case InputType.stopCommand:
				if (canStop) {
					stop();
				}
				break;

			case InputType.returnCommand:
				setResultDescriptionAsRecipesList();
				break;

			case InputType.reclaimCommand:
				break;

			case InputType.exitCommand:
				end();		
				StartCoroutine(exit());
				break;
		}
	}

	public void end() {
		CancelInvoke();
		GameState.Instance.ChangeCurrentState(GameState.GameStates.none);
        controller.NullCurrentDisplay();
        playerIngredients.Clear();
		recipesKnown.Clear();
		isCrafting = false;
		currentTime = 0;
		currentInstant = Instant.Before;

        controller.LogStringWithoutReturn(" ");
    }

	public IEnumerator exit() {
		yield return new WaitForSecondsRealtime(0.5f);

		GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
		controller.DisplayRoomText();
        controller.DisplayLoggedText();
	}

	public void receiveInput(string[] input) {
		Debug.Log("Recibido '" + input[0] + "'.");
		InputType type;
		SubType subtype;
		
		type = getInputType(input[0], input.Length);

		if (type == InputType.unrecognized) {
			return;
		}
		
		if (input.Length == 1 && type != InputType.addCommand) {
			apply(type);
			return;
		}

		if (input.Length > 1) {
			subtype = getSubType(input);
		} else {
			subtype = SubType.mono;
		}

		switch (subtype) {
			case SubType.mono:
				if (checkFor(input[0], playerIngredients)) {
					addIngredient(getIngredient(input[0]));
				}
				break;

			case SubType.bi:
				switch (type) {
					case InputType.addCommand:
						if (checkFor(getBiPredicate(input).ToString(), playerIngredients)) {
							addIngredient(getIngredient(getBiPredicate(input).ToString()));
						}
						break;

					case InputType.removeCommand:
						if (checkFor(getBiPredicate(input).ToString(), ingredientsIn)) {
							removeIngredient(getBiPredicate(input).ToString());
						}
						break;

					case InputType.readCommand:
						if (checkFor(getBiPredicate(input).ToString(), recipesKnown)) {
							setResultDescriptionAsRecipe(getRecipeWith(getBiPredicate(input).ToString()));
						}
						break;
				}
				break;

			case SubType.natural:


			case SubType.standard:
				break;
		}

		refresh();
	}

	public string[] getBiPredicate(string[] s) {
		string[] _s = new string[s.Length -1];
		for (int i = 1; i < s.Length; i++) {
			_s[i - 1] = s[i];
		}
		return _s;
	}

	public SubType getSubType(string[] input) {
		if (checkFor(input[1], one) || checkFor(input[1], two) || checkFor(input[1], three)
			|| checkFor(input[1], four) || checkFor(input[1], five)) {
			return SubType.natural;
		}

		if (checkFor(input[input.Length - 1], one) || checkFor(input[input.Length - 1], two) 
			|| checkFor(input[input.Length - 1], three) || checkFor(input[input.Length - 1], four) 
			|| checkFor(input[input.Length - 1], five)) {
			return SubType.standard;
		}

		return SubType.bi;
	}

	public InputType getInputType(string input, int n) {
		if (n > 1) {
			if (checkFor(input, addCommands))
				return InputType.addCommand;

			if (checkFor(input, removeCommands))
				return InputType.removeCommand;

			if (checkFor(input, readCommands)) {
				return InputType.readCommand;

			}
		}

		if (n == 1) {
			if (checkFor(input, recipesKnown))
				return InputType.readCommand;
			
			if (checkFor(input, playerIngredients))
				return InputType.addCommand;

			if (checkFor(input, backCommands))
				return InputType.backCommand;

			if (checkFor(input, nextCommands))
				return InputType.nextCommand;

			if (checkFor(input, playCommands))
				return InputType.playCommand;

			if (checkFor(input, pauseCommands))
				return InputType.pauseCommand;

			if (checkFor(input, stopCommands))
				return InputType.stopCommand;

			if (checkFor(input, returnCommands))
				return InputType.returnCommand;

			if (checkFor(input, reclaimCommands))
				return InputType.reclaimCommand;

			if (checkFor(input, exitCommands))
				return InputType.exitCommand;
		}

		return InputType.unrecognized;
	}

	public void refresh() {
		layout.ingredientsList.text = getTextIn(playerIngredients);
		layout.ingredientsUsed.text = getSlotsText();

		if (currentRecipe == null) {
			setResultDescriptionAsRecipesList();
		}
		else {
			setResultDescriptionAsRecipe(currentRecipe);
		}
		
		layout.pages.text = getPageText();
		layout.processCommands.text = getCommandsText();

		layout.processDescription.text = getProcessState();
		layout.recipeAnotation.text = getRecipeRealization();

		layout.recipeCommands.text = getRecipeCommandsText();

		Color t;
		switch (currentInstant) {
			case Instant.Before:
				if (ColorUtility.TryParseHtmlString("#" + disableColor_forPicture, out t)) {
					layout.picture.color = t;
					layout.time.color = t;
				}
				break;

			case Instant.While:
				if (ColorUtility.TryParseHtmlString("#" + ableColor_forPicture, out t)) {
					layout.picture.color = t;
					layout.time.color = t;
				}
				break;

			case Instant.Pause:
				if (ColorUtility.TryParseHtmlString("#" + disableColor_forPicture, out t)) {
					layout.picture.color = t;
				}
				break;

			case Instant.After:
				if (ColorUtility.TryParseHtmlString("#" + disableColor_forPicture, out t)) {
					layout.time.color = t;
					layout.picture.color = t;
				}
				break;
		}

		layout.ingredientCommands.text = getIngredientsCommandsText();
	}

	public IEnumerator cook() {

		while (currentInstant == Instant.While) {
			yield return new WaitForSecondsRealtime(1);
			currentTime++;
			layout.time.text = currentTime.ToString();
		}

	}

	public bool checkRealization(Recipe r) {
		foreach (Fragment f in r.fragments) {
			int count = 0;
			foreach (InteractableObject i in playerIngredients) {
				if (i == f.ingredient) {
					count++;
				}
				if (count >= f.volume) {
					break;	
				}
			}
			if (count < f.volume) {
				return false;
			}
		}
		return true;
	}

}

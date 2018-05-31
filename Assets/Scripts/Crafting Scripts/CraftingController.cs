using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingController : MonoBehaviour {

	public GameObject craftLayout;
	public RectTransform contentContainer;
	public TextMeshProUGUI display;
	private CraftingLayout layout;

	[Space(20)]
	public GameController controller;
	public InventoryManager inventory;
	public List<InteractableObject> playerIngredients;

	public float currentTime;
	public Recipe currentRecipe;
	public Recipient currentRecipient;

	public List<Process> process;
	private List<Process> beforeProcess;
	private List<Process> whileProcess;
	private List<Process> pauseProcess;

	public InteractableObject result;

	public enum Instant { Before, While, Pause, After }
	public enum AddingInstant { Before, While, Pause }

	public List<Recipe> recipes;                        
	[HideInInspector] public List<Recipe> recipesKnown; 
	Recipe potentialRecipe;

	public List<frag> ingredientsIn;

	public float craftingTime;                          
	float lastTime;
	public bool isCrafting;                             
	public Instant currentInstant;
	public AddingInstant currentAddingInstant;
	public bool alreadyStarted;

	public string inputStrings;

	public int totalPages;
	public int currentPage;
	public int slots;
	public int slotsUsed;

	public int resultState;

	#region 'can' bools
	public bool canPlay;
	public bool canPause;
	public bool canStop;
	public bool canExit;
	public bool canReclaim;
	#endregion

	#region colors
	private string color_forTitle = "F9A08BFF";
	private string color_forRecipeTitle = "F9AA98FF";

	private string ableColor_forText = "FFEBC6FF";
	private string disableColor_forText = "EEDAAD67";

	private string ableColor_forPicture = "F9A08B9D";
	private string disableColor_forPicture = "F9A08B3F";

	private string ableColor_forCommand = "F9CC88AA";
	private string disableColor_forCommand = "F9BE884F";

	private string disableColor_forLog = "FFEBC632";
	#endregion

	#region commands
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
	#endregion

	public enum InputType {	addCommand, removeCommand, backCommand, nextCommand, playCommand,
							pauseCommand, stopCommand, returnCommand, reclaimCommand, exitCommand,
							readCommand, unrecognized }
	public enum SubType	  {	mono, bi, natural,
							standard, unrecognized }
	//sintax
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

	public void initialize(Recipient r) {
		GameState.Instance.ChangeCurrentState(GameState.GameStates.crafting);
		fillPlayerIngredients();
		fillRecipes();
		alreadyStarted = false;
		currentInstant = Instant.Before;
		currentAddingInstant = AddingInstant.Before;
		isCrafting = false;
		craftingTime = 0;
		slotsUsed = 0;
		totalPages = getIngredientsPages();
		currentPage = 1;
		currentRecipient = r;
		slots = currentRecipient.slots;
		process = new List<Process>();
		beforeProcess = new List<Process>();
		pauseProcess = new List<Process>();
		whileProcess = new List<Process>();

		currentTime = 0;
		result = null;

		canPlay = true;
		canPause = false;
		canStop = false;
		canExit = true;

		ingredientsIn = new List<frag>();

		resultState = 0; //0-> Recetas, 1->Descripción, 2->Resultado, 3->Resultado Fallido.

		GameObject newCraft = Instantiate(craftLayout, contentContainer);
		layout = newCraft.GetComponent<CraftingLayout>();
		initLayout();
	}

	#region listFilling
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
		recipesKnown.Clear();
		foreach (InteractableObject b in inventory.nounsInInventory) {
			if (b.GetType() == typeof(RecipeBook)) {
				RecipeBook book = b as RecipeBook;
				foreach (Recipe r in book.recipes) {
					recipesKnown.Add(r);
				}
			}
		}
	}
	#endregion

	#region layoutJunk

	public void initLayout() {
		layout.title.text = getTitle();
		layout.ingredientsList.text = getTextIn(playerIngredients);
		layout.ingredientsUsed.text = getTextInProcess();
		setResultDescriptionAsRecipesList();
		layout.pages.text = getPageText();
		layout.processCommands.text = getCommandsText();
		layout.recipeAnotation.text = getRecipeRealization();
		layout.processDescription.text = getProcessState();
		layout.recipeCommands.text = getRecipeCommandsText();
		layout.ingredientCommands.text = getIngredientsCommandsText();

		StartCoroutine(adoptDisplay());
	}

	public void refresh() {
		if (inputStrings != null && display != null) {
			display.text = inputStrings.ToString();
		}
		layout.ingredientsList.text = getTextIn(playerIngredients);
		layout.ingredientsUsed.text = getTextInProcess();

		if (currentTime == 0.1f && canPlay) {
			layout.time.text = "0";
		}

		if (result != null) {
			setResultDescriptionAsResult();
		} else {
			if (result == null && resultState == 3) {
				setResultDescriptionAsResult();
			} else {
				if (currentRecipe == null) {
					setResultDescriptionAsRecipesList();
				}
				else {
					setResultDescriptionAsRecipe(currentRecipe);
				}
			}
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
		StartCoroutine(clearLog());
	}

	public void sendDisplayMessage(string s) {
		StopCoroutine(clearLog());
		if (display != null) {
			display.text = s;
		}
		StartCoroutine(clearLog());
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

	public string getRecipesList() {

		if (recipesKnown.Count == 0) {
			return "<color=#" + disableColor_forText + "><i>No hay recetas aprendidas.</i></color>";
		}


		string s = "<color=#" + ableColor_forText + ">";
		foreach (Recipe r in recipesKnown) {
			s += "+ " + r.recipeName + ".\n";
		}

		s += "</color>";

		if (s == "") {
			s = "<i>No hay recetas aprendidas.</i>";
		}
		return s;
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
		//Pending.
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
		}
		else {
			return "No realizable.";
		}
	}

	public string getRecipeCommandsText() {
		if (result != null) {
			return "<color=#" + ableColor_forCommand + "><b>[R]</b> Reclamar</color>";
		}

		if (result == null && resultState == 3) {
			return "<b>[V]</b> Volver";
		}

		if (currentRecipe == null) {
			return "";
		}
		else {
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
		}
		else {
			t += "<color=#" + ableColor_forText + "><b>[S]</b> Siguiente</color> \n";
		}

		return t;

	}

	public void setResultDescriptionAsRecipesList() {
		layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">Recetario</color>";
		layout.recipes.text = getRecipesList();
		currentRecipe = null;
		resultState = 0;
	}

	public void setResultDescriptionAsRecipe(Recipe r) {
		layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">" + r.recipeName + "</color>";
		layout.recipes.text = getRecipeDescription(r);
		currentRecipe = r;
		resultState = 1;
	}

	public void setResultDescriptionAsResult() {

		if (result != null) {
			layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">" + result.objectName + "</color>";
			layout.recipes.text = getResultDescription();
		} else {
			resultState = 3;
			layout.recipeTitle.text = "<color=#" + color_forRecipeTitle + ">Oops!</color>";
			layout.recipes.text = "No lograste producir nada.";
		}
	}

	public string getInputString(InputType t) {
		string s = "";
		s += "<color=#" + disableColor_forLog + ">";
		switch (t) {
			case InputType.addCommand:
				s += "Añadir ingrediente.";
				break;
			case InputType.backCommand:
				s += "Página anterior.";
				break;
			case InputType.exitCommand:
				s += "Salir.";
				break;
			case InputType.nextCommand:
				s += "Página siguiente.";
				break;
			case InputType.pauseCommand:
				s += "Pausar.";
				break;
			case InputType.playCommand:
				s += "Encender.";
				break;
			case InputType.readCommand:
				s += "Leer receta.";
				break;
			case InputType.reclaimCommand:
				s += "Reclamar.";
				break;
			case InputType.removeCommand:
				s += "Quitar ingrediente.";
				break;
			case InputType.returnCommand:
				s += "Volver al recetario.";
				break;
			case InputType.stopCommand:
				s += "Detener.";
				break;
		}

		s += "</color>";
		return s;
	}

	public string getResultDescription() {
		string s = "";
		s += result.descriptionAtAnalized;
		return s;
	}

	#endregion

	#region getVolumes
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

	public int getVolumeInProcess(InteractableObject ingredient, AddingInstant instant) {
		int count = 0;
		foreach (Process p in process) {
			if (p.frag.ingredient == ingredient && p.instantAdded == instant) {
				count++;
			}
		}
		return count;
	}

	#endregion

	#region checkFor
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
			foreach (string n in list[i].product.nouns) {
				if (s == n) {
					return true;
				}
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

	public bool checkFor(Process p, List<Process> list) {
		foreach (Process _p in list) {
			if (p == _p) {
				return true;
			}
		}
		return false;
	}

	public bool checkOneFor(Process p, List<Process> list) {
		foreach (Process _p in list) {
			if (p.frag == _p.frag && p.instantAdded == _p.instantAdded) {
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

	public bool checkFor(string s, string[] l) {
		foreach (string _s in l) {
			if (s == _s) {
				return true;
			}
		}
		return false;
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

	public bool checkFor(Recipe r, List<Recipe> l) {
		foreach (Recipe _r in l) {
			if (r == _r) {
				return true;
			}
		}
		return false;
	}
	#endregion

	#region gets

	public Recipe getRecipeWith(string s) {
		foreach (Recipe r in recipesKnown) {
			if (r.recipeName == s) {
				return r;
			}
			foreach (string n in r.product.nouns) {
				if (s == n) {
					return r;
				}
			}
		}
		return null;
	}

	public InteractableObject getIngredientFromPlayerWith(string s) {
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

	public string getTextInProcess() {

		string textConsumed = "</color><color=#" + ableColor_forText + ">";
		string textUnconsumed = "</color><color=#" + disableColor_forText + ">";

		List<frag> fragsConsumed = new List<frag>();
		List<frag> fragsUnconsumed = new List<frag>();

		foreach (frag f in ingredientsIn) {
			if (f.consumed) {
				fragsConsumed.Add(f);
			}

			if (!f.consumed) { 
				fragsUnconsumed.Add(f);
			}
		}

		List<frag> f_c = new List<frag>();
		List<frag> f_u = new List<frag>();

		foreach (frag f in fragsConsumed) {
			if (!checkFor(f.ingredient, f_c)) {
				f_c.Add(f);
				int volume = getVolumeOf(f.ingredient, fragsConsumed);
				textUnconsumed += "> " + f.ingredient.objectName + " x" + volume + "\n";
			}
		}

		foreach (frag f in fragsUnconsumed) {
			if (!checkFor(f.ingredient, f_u)) {
				f_u.Add(f);
				int volume = getVolumeOf(f.ingredient, fragsUnconsumed);
				textConsumed += "> " + f.ingredient.objectName + " x" + volume + "\n";
			}
		}

		if (f_c.Count + f_u.Count < slots) {
			for (int i = f_c.Count + f_u.Count; i < slots; i++) {
				textConsumed += "> \n";
			}
		}


		textUnconsumed += "</color>";
		textConsumed += "</color>";
		return textUnconsumed + textConsumed;
	}

	public RecipeBook getRecipebook() {
		foreach (InteractableObject i in inventory.nounsInInventory) {
			if (i.GetType() == typeof(RecipeBook)) {
				RecipeBook r = i as RecipeBook;
			}
		}
		return null;
	}

	#endregion

	#region ingredientsManagement
	public void addIngredient(InteractableObject ingredient) {
		if (checkFor(ingredient, playerIngredients)) {
			if (checkFor(ingredient, ingredientsIn)) {
				add(ingredient);
			} else {
				if (slotsUsed < slots) {
					slotsUsed++;
					add(ingredient);
				}
			}
		}
		refresh();
	}

	public void add(InteractableObject ingredient) {
		sendDisplayMessage("+ " + ingredient.objectName + ".");
		ingredientsIn.Add(new frag(ingredient));
		if (currentInstant == Instant.While) {
			ingredientsIn[ingredientsIn.Count - 1].consumed = true;
			destroyIngredient(ingredientsIn[ingredientsIn.Count - 1].ingredient, 1);
		}
		playerIngredients.Remove(ingredient);
		Debug.Log("+: " + ingredientsIn[ingredientsIn.Count - 1].ingredient.objectName + " in " + currentAddingInstant
			+ " at " + currentTime + " seconds.");
		process.Add(new Process(currentAddingInstant, ingredientsIn[ingredientsIn.Count - 1], currentTime));
	}

	public void addIngredient(InteractableObject ingredient, int volume) {
		if (checkFor(ingredient, volume, playerIngredients) && slotsUsed < slots) {
			for (int i = 0; i < volume; i++) {
				if (!checkFor(ingredient, ingredientsIn)) {
					slotsUsed++;
				}
				ingredientsIn.Add(new frag(ingredient));
				playerIngredients.Remove(ingredient);
			}
			slotsUsed++;
		}
		refresh();
	}

	public void removeIngredient(string s) {
		foreach (frag f in ingredientsIn) {
			if (s == f.ingredient.objectName && f.consumed == false) {
				playerIngredients.Add(f.ingredient);
				ingredientsIn.Remove(f);
			}
		}
		refresh();
	}

	public void consumeIngredients() {
		foreach (frag i in ingredientsIn) {
			i.consumed = true;
			destroyIngredient(i.ingredient, 1);
		}
		refresh();
	}

	public void destroyIngredient(InteractableObject ingredient, int times) {
		int t = 0;
		foreach (InteractableObject pocket in inventory.nounsInInventory) {
			if (pocket.GetType() == typeof(Pocket)) {
				Pocket p = pocket as Pocket;
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
									inventory.DisplayInventory();
									return;
								}
							}
						}
					}
				}
			}
		}

		if (t < times) {
			foreach (InteractableObject i in inventory.nounsInInventory) {
				if (i == ingredient) {
					inventory.nounsInInventory.Remove(i);
					t++;
				}
				if (t == times) {
					inventory.DisplayInventory();
					return;
				}
			}
		}

		inventory.DisplayInventory();
	}
	
	public void addResultToInventory() {
		inventory.nounsInInventory.Add(result);
		result = null;
		refresh();
		inventory.DisplayInventory();
	}
	#endregion

	#region result
	public InteractableObject getResult() {
		realignProcess();
		potentialRecipe = getRecipe();
		if (potentialRecipe != null) {
			if (!checkFor(potentialRecipe, recipesKnown)) {
				RecipeBook r = getRecipebook();
				if (r != null) {
					r.recipes.Add(potentialRecipe);
					recipesKnown.Add(potentialRecipe);

				}
				else {
					recipesKnown.Add(potentialRecipe);
				}

			}
			resultState = 2;
			return potentialRecipe.product;
		}

		resultState = 3;
		return null;
	}

	public Recipe getRecipe() {
		foreach (Recipe r in recipes) {
			int checkList = 3;

			if (r.beforeFragments.Count >= 1) {
				foreach (Fragment f in r.beforeFragments) {
					foreach (Process p in beforeProcess) {
						if (f.ingredient == p.frag.ingredient && f.volume != p.volume) {
							checkList--;
						}
					}
				}
			}

			if (r.whileFragments.Count >= 1) {
				foreach (Fragment f in r.whileFragments) {
					foreach (Process p in whileProcess) {
						if (f.ingredient == p.frag.ingredient && f.volume != p.volume) {
							checkList--;
						}
					}
				}
			}

			if (r.pauseFragments.Count >= 1) {
				foreach (Fragment f in r.pauseFragments) {
					foreach (Process p in pauseProcess) {
						if (f.ingredient == p.frag.ingredient && f.volume != p.volume) {
							checkList--;
						}
					}
				}
			}

			if (checkList == 3 && r.minTime <= currentTime && currentTime <= r.maxTime) {
				return r;
			}
		}
		return null;
	}

	public void realignProcess() {
		List<Process> temp = new List<Process>();

		foreach (Process p in process) {
			if (!checkOneFor(p, temp)) {
				temp.Add(new Process(p.instantAdded, p.frag, p.timeAdded, getVolumeInProcess(p.frag.ingredient, p.instantAdded)));
				Debug.Log(temp[temp.Count - 1].frag.ingredient.objectName + " x" +
					temp[temp.Count - 1].volume + " at" + temp[temp.Count - 1].instantAdded + ".");
			}
		}

		process = temp;
		setSubProcess();
	}

	public void setSubProcess() {
		foreach (Process p in process) {
			switch (p.instantAdded) {
				case AddingInstant.Before:
					beforeProcess.Add(p);
					break;
				case AddingInstant.Pause:
					pauseProcess.Add(p);
					break;
				case AddingInstant.While:
					whileProcess.Add(p);
					break;
			}
		}
	}
	#endregion

	#region machine

	public void play() {
		process = new List<Process>();
		currentInstant = Instant.While;
		currentAddingInstant = AddingInstant.While;
		alreadyStarted = true;
		consumeIngredients();
		canPlay = false;
		canPause = true;
		canStop = true;
		canExit = false;
		StartCoroutine(cook());
		refresh();
	}

	public void pause() {
		currentTime = lastTime;
		currentInstant = Instant.Pause;
		currentAddingInstant = AddingInstant.Pause;
		canPause = false;
		canPlay = true;
		canStop = true;
		canExit = false;
		StopCoroutine(cook());
		refresh();
	}

	public void stop() {
		result = getResult();
		currentTime = lastTime;
		currentTime = 0;
		slotsUsed = 0;
		layout.time.text = Mathf.Round(currentTime).ToString("F0");
		currentInstant = Instant.After;
		currentAddingInstant = AddingInstant.Before;
		alreadyStarted = false;
		canStop = false;
		canPlay = true;
		canPause = false;
		canExit = true;
		StopCoroutine(cook());
		ingredientsIn.Clear();
		refresh();
	}

	#endregion

	public void applyMono(InputType t, string i) {
		switch (t) {
			case InputType.addCommand:
				addIngredient(getIngredientFromPlayerWith(i));
				break;

			case InputType.playCommand:
				if (result != null) {
					addResultToInventory();
					sendDisplayMessage("Producto añadido al inventario.");
				}
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

			case InputType.readCommand:
				setResultDescriptionAsRecipe(getRecipeWith(i));
				break;

			case InputType.returnCommand:
				if (result == null) {
					setResultDescriptionAsRecipesList();
				} else {
					addResultToInventory();
					sendDisplayMessage("Producto añadido al inventario.");
					setResultDescriptionAsRecipesList();
				}
				break;

			case InputType.reclaimCommand:
				if (result != null) {
					addResultToInventory();
					sendDisplayMessage("Producto añadido al inventario.");
					setResultDescriptionAsRecipesList();
				}
				break;

			case InputType.exitCommand:
				if (canExit) {
					end();
					if (result != null) {
						addResultToInventory();
						sendDisplayMessage("Producto añadido al inventario.");
					}
					StartCoroutine(exit());
				}
				break;
		}

		refresh();
	}

	public void applyBi(InputType t, string i) {

	}

	public void receiveInput(string[] input) {
		InputType type;
		SubType subtype;
		
		type = getInputType(input[0], input.Length);
		inputStrings = getInputString(type);

		if (type == InputType.unrecognized) {
			return;
		}
		
		if (input.Length == 1) {
			subtype = SubType.mono;
			applyMono(type, input[0]);
			return;
		}

		subtype = getSubType(input);

		Debug.Log(type + " " + subtype);
		Debug.Log(getStringPredicate( getBiPredicate(input)) );

		switch (subtype) {
			case SubType.bi:
				switch (type) {
					case InputType.addCommand:
						if (checkFor(getBiPredicate(input).ToString(), playerIngredients)) {
							addIngredient(getIngredientFromPlayerWith(getBiPredicate(input).ToString()));
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

	public string getStringPredicate(string[] s) {
		string _s = "";
		foreach (string c in s) {
			_s += c + " ";
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

			if (checkFor(input, readCommands)) 
				return InputType.readCommand;
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

	public IEnumerator cook() {
		currentInstant = Instant.While;
		while (currentInstant == Instant.While) {
			lastTime = currentTime;
			yield return new WaitForSecondsRealtime(0.1f);
			currentTime += 0.1f;
			layout.time.text = currentTime.ToString("F1");
		}
	}

	public IEnumerator adoptDisplay() {
		yield return new WaitForSecondsRealtime(0.5f);
		display = contentContainer.GetChild(contentContainer.childCount - 1).gameObject.GetComponent<TextMeshProUGUI>();
		refresh();
	}

	public IEnumerator clearLog() {
		yield return new WaitForSeconds(2f);
		if (display != null) {
			display.text = " ";
		}
	}

	#region exit
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
	#endregion
}

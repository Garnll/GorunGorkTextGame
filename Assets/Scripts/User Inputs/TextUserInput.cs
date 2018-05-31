using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

/// <summary>
/// Recibe y envía los inputs del jugador según el estado del juego.
/// </summary>
public class TextUserInput : SerializedMonoBehaviour {

    public TMP_InputField inputField;
    public HabilitiesTextInput habilitiesTextInput;
    public Scrollbar principalScroll;
    [SerializeField] Dictionary<string, InputActions> inputDictionary = new Dictionary<string, InputActions>();

	[Space(15)]
	[SerializeField]
	string[] wrongDialogueInputs;

    GameController controller;

    //Enviado a CombatController cuando inicia una pelea.
    public delegate void WaitUntilFinished(GameController controller);
    public static event WaitUntilFinished OnFight;

    private void Awake()
    {
        controller = GetComponent<GameController>();

        //inputField.onEndEdit.AddListener(AcceptStringInput);
        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            InputActions inputAction = controller.inputActions[i];
            if (!inputDictionary.ContainsKey(inputAction.keyWord))
            {
                inputDictionary.Add(inputAction.keyWord, inputAction);
            }
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !NetworkManager.Instance.isConnecting)
        {
            AcceptStringInput(inputField.text);
            inputField.text = "";
        }
    }

    /// <summary>
    /// Accepta el input del jugador una vez este presiona "Enter"
    /// </summary>
    /// <param name="userInput"></param>
    public void AcceptStringInput(string userInput)
    {
        string originalInput = userInput;
        userInput = userInput.ToLower();

        char[] delimeterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimeterCharacters);
        string[] separatedCompleteWords = originalInput.Split(delimeterCharacters);

        principalScroll.value = 0;

        switch (GameState.Instance.CurrentState)
        {
            default:
                inputField.ActivateInputField();
                inputField.text = null;
                break;

            case GameState.GameStates.exploration:
                if (inputDictionary.ContainsKey(separatedInputWords[0]))
                {
                    if (inputDictionary[separatedInputWords[0]].GetType() != typeof(SayInput))
                    {
                        string userInputChanged = "<color=#9C9C9CC0>" + originalInput + "</color>";
                        controller.LogStringWithReturn(userInputChanged);

                        inputDictionary[separatedInputWords[0]].RespondToInput(controller, separatedInputWords, separatedCompleteWords);
                    }
                    else
                    {
                        string[] separatedOriginalInput = originalInput.Split(delimeterCharacters);

                        inputDictionary[separatedInputWords[0]].RespondToInput(controller, separatedOriginalInput, separatedCompleteWords);
                    }
                }
                else if (separatedInputWords[0] != "")
                {
                    string userInputChanged = "<color=#9C9C9CC0>" + originalInput + "</color>";
                    string answer = "<color=#9C9C9CC0>" + AnswerToWrongInput() + "</color>";
                    controller.LogStringWithReturn(userInputChanged + "\n" + answer);
                }
                DisplayInput();
                break;

            case GameState.GameStates.introduction:

                if (separatedInputWords[0] == "")
                {
                    DisplayInput();
                    return;
                }

                AnythingWorksResponse anythingWorks = controller.playerRoomNavigation.currentRoom.roomResponse as AnythingWorksResponse;

                controller.LogStringWithReturn("<color=#9C9C9CC0>" + originalInput + "</color>");
                anythingWorks.AcceptInput(controller);
                DisplayInput();

                break;

            case GameState.GameStates.creation:

                if (separatedInputWords[0] == "")
                {
                    DisplayInput();
                    return;
                }

                CharacterCreationResponse characterCreation = controller.playerRoomNavigation.currentRoom.roomResponse as CharacterCreationResponse;

                controller.LogStringWithReturn("<color=#9C9C9CC0>" + originalInput + "</color>");
                characterCreation.AcceptInput(separatedInputWords, originalInput);
                DisplayInput();
                break;


            case GameState.GameStates.levelUp:

                if (separatedInputWords[0] == "")
                {
                    DisplayInput();
                    return;
                }

                controller.LogStringWithReturn("<color=#9C9C9CC0>" + originalInput + "</color>");
                controller.playerManager.characteristicsChanger.AcceptInput(separatedInputWords, controller);
                DisplayInput();
                break;

            case GameState.GameStates.combatPreparation:
                DisplayInput();
                break;

            case GameState.GameStates.combat:
                char[] delimeterPoints = { '.', ' ' };
                string[] separatedInputs = userInput.Split(delimeterPoints);

                controller.combatController.ReceiveInput(separatedInputs, habilitiesTextInput);
                inputField.ActivateInputField();
                inputField.text = null;
                break;

			case GameState.GameStates.conversation:
				if (separatedInputWords[0] == "") {
					DisplayInput();
					return;
				}

				if (!controller.dialogueController.checkInput(separatedInputWords[0]) && controller.dialogueController.isTalking) {
					string userInputChanged = "<color=#9C9C9CC0>" + originalInput + "</color>";
					string answer = "<color=#9C9C9CC0>" + getWrongDialogue() + "</color>";
					controller.LogStringWithReturn(userInputChanged + "\n" + answer);
					controller.dialogueController.displayText();
				}

				controller.dialogueController.takeInput(userInput);
				DisplayInput();
				break;

			case GameState.GameStates.crafting:
				controller.craftController.receiveInput(separatedInputWords);
				inputField.ActivateInputField();
				inputField.text = null;
				break;

        }

    }

    /// <summary>
    /// Le muestra el input dado por el jugador y la respuesta obtenida al jugador en el display principal.
    /// </summary>
    void DisplayInput()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = null;

        if (GameState.Instance.CurrentState == GameState.GameStates.combatPreparation)
        {
            if (OnFight != null)
            {
                OnFight(controller);
            }
        }
    }

    /// <summary>
    /// Elje una respuesta aleatoria que le devuelve al jugador en caso de input erroneo.
    /// </summary>
    /// <returns></returns>
    string AnswerToWrongInput()
    {
        string[] randomAnswer = new string[11];
        randomAnswer[0] = "¿Eh? Repite eso.";
        randomAnswer[1] = "Piensas algo incoherente.";
        randomAnswer[2] = "Quieres hacer algo imposible.";
        randomAnswer[3] = "No entiendes tus propios pensamientos.";
        randomAnswer[4] = "¿¿¿¿????";
        randomAnswer[5] = "Me temo que no puedo hacer eso, Dave.";
        randomAnswer[6] = "Tu cuerpo no tienes esas habilidades.";
        randomAnswer[7] = "¿Disculpa?";
        randomAnswer[8] = "¿Qué dijiste?";
        randomAnswer[9] = "Nada, no entiendo.";
		randomAnswer[10] = "La tuya por si acaso.";

		string answer = randomAnswer[Random.Range(0, randomAnswer.Length)];

        return answer;
    }

	string getWrongDialogue() {
		int r = Random.Range(0, wrongDialogueInputs.Length - 1);
		return wrongDialogueInputs[r];
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class TextUserInput : SerializedMonoBehaviour {

    public TMP_InputField inputField;
    [SerializeField] Dictionary<string, InputActions> inputDictionary = new Dictionary<string, InputActions>();

    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        inputField.onEndEdit.AddListener(AcceptStringInput);

        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            InputActions inputAction = controller.inputActions[i];
            if (!inputDictionary.ContainsKey(inputAction.keyWord))
            {
                inputDictionary.Add(inputAction.keyWord, inputAction);
            }
        }

    }

    void AcceptStringInput(string userInput)
    {
        string originalInput = userInput;
        userInput = userInput.ToLower();

        char[] delimeterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimeterCharacters);

        switch (GameState.Instance.CurrentState)
        {
            default:
            case GameState.GameStates.exploration:
                if (inputDictionary.ContainsKey(separatedInputWords[0]))
                {
                    if (inputDictionary[separatedInputWords[0]].GetType() != typeof(SayInput))
                    {
                        string userInputChanged = "<color=#9C9C9CC0>" + userInput + "</color>";
                        controller.LogStringWithReturn(userInputChanged);

                        inputDictionary[separatedInputWords[0]].RespondToInput(controller, separatedInputWords);
                    }
                    else
                    {
                        string[] separatedOriginalInput = originalInput.Split(delimeterCharacters);

                        inputDictionary[separatedInputWords[0]].RespondToInput(controller, separatedOriginalInput);
                    }
                }
                else
                {
                    string userInputChanged = "<color=#9C9C9CC0>" + userInput + "</color>";
                    string answer = "<color=#9C9C9CC0>" + AnswerToWrongInput() + "</color>";
                    controller.LogStringWithReturn(userInputChanged + "\n" + answer);
                }

                DisplayInput();
                break;

            case GameState.GameStates.creation:
                CharacterCreationResponse characterCreation = controller.playerRoomNavigation.currentRoom.roomResponse as CharacterCreationResponse;

                controller.LogStringWithReturn("<color=#9C9C9CC0>" + userInput + "</color>");
                characterCreation.AcceptInput(separatedInputWords);
                break;
        }
    }

    void DisplayInput()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = null;
    }

    string AnswerToWrongInput()
    {
        string[] randomAnswer = new string[10];
        randomAnswer[0] = "¿Eh?";
        randomAnswer[1] = "Piensas algo incoherente.";
        randomAnswer[2] = "Quieres hacer algo imposible.";
        randomAnswer[3] = "No entiendes tus propios pensamientos.";
        randomAnswer[4] = "¿¿¿¿????";
        randomAnswer[5] = "Me temo que no puedo hacer eso, Dave.";
        randomAnswer[6] = "Tu cuerpo no tienes esas habilidades.";
        randomAnswer[7] = "¿Disculpa?";
        randomAnswer[8] = "¿Qué dijiste?";
        randomAnswer[9] = "Nada, no entiendo.";

        string answer = randomAnswer[Random.Range(0, randomAnswer.Length)];

        return answer;
    }
}

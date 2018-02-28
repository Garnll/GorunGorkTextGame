using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class TextInput : SerializedMonoBehaviour {

    //public InputField inputField;
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

        userInput = userInput.ToLower();
        controller.LogStringWithReturn(userInput);

        char[] delimeterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimeterCharacters);

        if (inputDictionary.ContainsKey(separatedInputWords[0]))
        {
            inputDictionary[separatedInputWords[0]].RespondToInput(controller, separatedInputWords);
        }

        InputComplete();
    }

    void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = null;
    }
}

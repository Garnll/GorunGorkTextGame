using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInput : MonoBehaviour {

    //public InputField inputField;
    public TMP_InputField inputField;

    GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        inputField.onEndEdit.AddListener(AcceptStringInput);
    }

    void AcceptStringInput(string userInput)
    {
        //Esto es para cambiar por dictionary

        userInput = userInput.ToLower();
        controller.LogStringWithReturn(userInput);

        char[] delimeterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimeterCharacters);

        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            InputActions inputAction = controller.inputActions[i];
            if (inputAction.keyWord == separatedInputWords[0])
            {
                inputAction.RespondToInput(controller, separatedInputWords);
            }
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

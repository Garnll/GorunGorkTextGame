using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextConverter {

	public static string MakeFirstLetterUpper(string original)
    {
        char[] separatedString = original.ToCharArray();

        separatedString[0] = char.ToUpper(separatedString[0]);

        return new string(separatedString);
    }

    public static string OutputObjectHimOrHer(InteractableObject interactable)
    {
        if (interactable.nounGender == InteractableObject.WordGender.male)
        {
            return "el " + interactable.objectName;
        }
        else
        {
            return "la " + interactable.objectName;
        }
    }
}

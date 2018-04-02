using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text/TextColorCoding")]
public class TextColorCoding : ScriptableObject {

    private static TextColorCoding instance = null;

    public static TextColorCoding Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.FindObjectsOfTypeAll<TextColorCoding>()[0];
            }
            return instance;
        }
    }

    public enum TextColors
    {
        white, grey, green, blue
    }

    [SerializeField] private Color white = new Color(1,1,1,1);
    [SerializeField] private Color grey = new Color(1, 1, 1, 1);
    [SerializeField] private Color green = new Color(1, 1, 1, 1);
    [SerializeField] private Color blue = new Color(1, 1, 1, 1);

    public Color[] GetColors()
    {
        Color[] colors = {white, grey, green, blue };
        return colors;
    }

    public string ChangeTextColor(string textToChange, TextColors newColor)
    {
        string fullSentence;
        string colorStart;
        string colorFinish = "</color>";

        switch (newColor)
        {
            default:
            case TextColors.white:
                colorStart = "<color=#" + ColorUtility.ToHtmlStringRGB(white) + ">";
                break;

            case TextColors.grey:
                colorStart = "<color=#" + ColorUtility.ToHtmlStringRGB(grey) + ">";
                break;

            case TextColors.green:
                colorStart = "<color=#" + ColorUtility.ToHtmlStringRGB(green) + ">";
                break;

            case TextColors.blue:
                colorStart = "<color=#" + ColorUtility.ToHtmlStringRGB(blue) + ">";
                break;
        }

        fullSentence = colorStart + textToChange + colorFinish;

        return fullSentence;
    }
}

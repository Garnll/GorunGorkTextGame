using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Font Change")]
public class FontChangeInput : InputActions
{
    public FontEqualizer fontEqualizer;

    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        if (fontEqualizer == null)
        {
            fontEqualizer = FindObjectOfType<FontEqualizer>();
        }

        if (separatedInputWords.Length > 1)
        {
            if (separatedInputWords[1] == "+")
            {
                fontEqualizer.ChangeSize(1);
                return;
            }

            if (separatedInputWords[1] == "-")
            {
                fontEqualizer.ChangeSize(-1);
                return;
            }

            controller.LogStringWithReturn("Indica si quieres aumentar (+) o disminuir (-) el tamaño de la letra.");
        }
        else
        {
            if (separatedInputWords[0] == "+")
            {
                fontEqualizer.ChangeSize(1);
                return;
            }

            if (separatedInputWords[0] == "-")
            {
                fontEqualizer.ChangeSize(-1);
                return;
            }
        }

    }
}

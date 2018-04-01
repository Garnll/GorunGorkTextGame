using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Recibe la font y la cambia
/// </summary>
[ExecuteInEditMode]
public class FontReceiver : MonoBehaviour {

    private TextMeshProUGUI myTextMesh;
    private TMP_InputField myInput;

    private float originalSize;

	void OnEnable ()
    {
        myTextMesh = GetComponent<TextMeshProUGUI>();
        myInput = GetComponent<TMP_InputField>();

        if (myTextMesh != null)
        {
            originalSize = myTextMesh.fontSize;
        }
        if (myInput != null)
        {
            originalSize = myInput.pointSize;
        }

        FontEqualizer.OnFontChange += ChangeMyFont;
	}

    void ChangeMyFont(TMP_FontAsset newFont, float sizeChange)
    {
        if (myTextMesh != null)
        {
            myTextMesh.font = newFont;
            myTextMesh.fontSize = originalSize + sizeChange;
        }
        if (myInput != null)
        {
            myInput.fontAsset = newFont;
            myInput.pointSize = originalSize + sizeChange;
        }
    }

    private void OnDisable()
    {
        if (myTextMesh != null)
        {
            myTextMesh.fontSize = originalSize;
        }
        if (myInput != null)
        {
            myInput.pointSize = myInput.pointSize;
        }

        FontEqualizer.OnFontChange -= ChangeMyFont;
    }
}

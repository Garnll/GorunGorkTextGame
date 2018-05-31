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
        FontEqualizer.OnFontSizeChange += ChangeFontSize;
	}

    void ChangeMyFont(TMP_FontAsset newFont)
    {
        if (myTextMesh != null)
        {
            myTextMesh.font = newFont;
        }
        if (myInput != null)
        {
            myInput.fontAsset = newFont;
        }
    }

    void ChangeFontSize(int sumer)
    {
        Debug.Log(sumer);
        if (myTextMesh != null)
        {
            myTextMesh.fontSize = originalSize + sumer;
        }
        if (myInput != null)
        {
            myInput.pointSize = originalSize + sumer;
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
            myInput.pointSize = originalSize;
        }

        FontEqualizer.OnFontChange -= ChangeMyFont;
    }
}

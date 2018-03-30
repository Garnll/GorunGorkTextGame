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

	void OnEnable ()
    {
        myTextMesh = GetComponent<TextMeshProUGUI>();
        myInput = GetComponent<TMP_InputField>();
        FontEqualizer.OnFontChange += ChangeMyFont;
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

    private void OnDisable()
    {
        FontEqualizer.OnFontChange -= ChangeMyFont;
    }
}

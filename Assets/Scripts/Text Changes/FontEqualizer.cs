using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script que solo cambia el font de los TextMeshes
/// </summary>
[ExecuteInEditMode]
public class FontEqualizer : MonoBehaviour {

    public TMP_FontAsset font;
    public int pluser = 0;

    //Enviado a FontReceiver
    public delegate void ChangeFont(TMP_FontAsset newFont);
    public static event ChangeFont OnFontChange;

    public delegate void ChangeFontSize(int sizeSume);
    public static event ChangeFontSize OnFontSizeChange;


#if UNITY_EDITOR
    private void LateUpdate()
    {

        if (OnFontChange != null)
        {
            OnFontChange(font);
        }
        

        if (OnFontSizeChange != null)
        {
           OnFontSizeChange(pluser);
        }
    }
#endif
}

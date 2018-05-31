using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Script que solo cambia el font de los TextMeshes
/// </summary>
[ExecuteInEditMode]
public class FontEqualizer : MonoBehaviour {

    public TMP_FontAsset font;
    [SerializeField] private int pluser = 0;

    //Enviado a FontReceiver
    public delegate void ChangeFont(TMP_FontAsset newFont);
    public static event ChangeFont OnFontChange;

    public delegate void ChangeFontSize(int sizeSume);
    public static event ChangeFontSize OnFontSizeChange;

    private void Awake()
    {
        pluser = 0;
        StartCoroutine(UpdateFont());
    }

    public void ChangeSize(int sizeUpOrDown)
    {
        pluser = pluser + sizeUpOrDown;

        if (OnFontSizeChange != null)
        {
            OnFontSizeChange(pluser);
            Debug.Log("yeah");
        }
    }

    private IEnumerator UpdateFont()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (OnFontChange != null)
            {
                OnFontChange(font);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Base de las UIs durante el combate.
/// </summary>
public abstract class UIDuringCombat : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI title;
    [HideInInspector] public Slider lifeSlider;
    [HideInInspector] public TextMeshProUGUI lifeText;
    [HideInInspector] public Slider turnSlider;
    [HideInInspector] public TextMeshProUGUI willText;
    [HideInInspector] public TextMeshProUGUI logText;

    public abstract void InstantiateMyStuff(RectTransform combatParent);
}

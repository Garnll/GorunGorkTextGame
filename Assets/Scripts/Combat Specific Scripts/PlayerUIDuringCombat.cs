using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Parametros en script del prefab del UI del jugador durante el combate.
/// </summary>
public class PlayerUIDuringCombat : MonoBehaviour {

    [HideInInspector] public TextMeshProUGUI title;
    [HideInInspector] public Slider lifeSlider;
    [HideInInspector] public TextMeshProUGUI lifeText;
    [HideInInspector] public Slider turnSlider;
    [HideInInspector] public TextMeshProUGUI willText;
    [HideInInspector] public TextMeshProUGUI habilitiesText;
    [HideInInspector] public TextMeshProUGUI optionsText;
    [HideInInspector] public TextMeshProUGUI logText;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerTitle;
    [SerializeField] private GameObject playerLife;
    [SerializeField] private GameObject playerTurnAndWill;
    [SerializeField] private GameObject playerHabilities;
    [SerializeField] private GameObject playerOptions;
    [SerializeField] private GameObject separator;
    [SerializeField] private GameObject playerLog;

    public void InstantiateMyStuff(RectTransform combatParent)
    {
        RectTransform enemyContainer = Instantiate(playerPrefab, combatParent).GetComponent<RectTransform>();

        GameObject temp = Instantiate(playerTitle, enemyContainer.transform);
        title = temp.GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerLife, enemyContainer);
        lifeSlider = temp.GetComponentInChildren<Slider>();
        lifeText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerTurnAndWill, enemyContainer);
        turnSlider = temp.GetComponentInChildren<Slider>();
        willText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerHabilities, enemyContainer);
        habilitiesText = temp.GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerOptions, enemyContainer);
        optionsText = temp.GetComponent<TextMeshProUGUI>();

        Instantiate(separator, enemyContainer.transform);

        temp = Instantiate(playerLog, enemyContainer);
        logText = temp.GetComponent<TextMeshProUGUI>();
    }
}

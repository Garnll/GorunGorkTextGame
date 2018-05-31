using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Parametros en script del prefab del UI del jugador durante el combate.
/// </summary>
public class PlayerUIDuringCombat : UIDuringCombat {
    [HideInInspector] public TextMeshProUGUI habilitiesText;
    [HideInInspector] public TextMeshProUGUI optionsText;
    [HideInInspector] public RectTransform playerContainer;

    public GameObject timerTextObject;
    [HideInInspector] public TextMeshProUGUI timerText;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerTitle;
    [SerializeField] private GameObject playerLife;
    [SerializeField] private GameObject playerTurnAndWill;
    [SerializeField] private GameObject playerHabilities;
    [SerializeField] private GameObject playerOptions;
    [SerializeField] private GameObject separator;
    [SerializeField] private GameObject playerLog;

    /// <summary>
    /// Instancia los gameObjects necesarios del jugador para el combate, pasandose a si mismo estas referencias.
    /// </summary>
    /// <param name="combatParent"></param>
    public override void InstantiateMyStuff(RectTransform combatParent)
    {
        playerContainer = Instantiate(playerPrefab, combatParent).GetComponent<RectTransform>();

        GameObject temp = Instantiate(playerTitle, playerContainer.transform);
        title = temp.GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerLife, playerContainer);
		//lifeLabel = temp.GetComponentInChildren<>
        lifeSlider = temp.GetComponentInChildren<Slider>();
        lifeText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerTurnAndWill, playerContainer);
        turnSlider = temp.GetComponentInChildren<Slider>();
        willText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        turnParticles.transform.SetParent(turnSlider.transform);
        turnParticles.transform.localPosition = new Vector3(50, 0, -10);
        turnParticles.transform.localScale = Vector3.one;

        temp = Instantiate(playerHabilities, playerContainer);
        habilitiesText = temp.GetComponent<TextMeshProUGUI>();

        temp = Instantiate(playerOptions, playerContainer);
        optionsText = temp.GetComponent<TextMeshProUGUI>();

        Instantiate(separator, playerContainer.transform);

        temp = Instantiate(playerLog, playerContainer);
        logText = temp.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTimerText(GameObject instance)
    {
        timerText = instance.GetComponent<TextMeshProUGUI>();
    }
}

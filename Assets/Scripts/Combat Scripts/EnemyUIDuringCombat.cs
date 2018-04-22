using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Parametros en script del prefab del UI del enemigo durante el combate.
/// </summary>
public class EnemyUIDuringCombat : UIDuringCombat {

    [HideInInspector] public TextMeshProUGUI descriptionText;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyTitle;
    [SerializeField] private GameObject enemyLife;
    [SerializeField] private GameObject enemyTurnAndWill;
    [SerializeField] private GameObject enemyDescription;
    [SerializeField] private GameObject separator;
    [SerializeField] private GameObject enemyLog;

    /// <summary>
    /// Instancia los gameObjects necesarios del enemigo para el combate, pasandose a si mismo estas referencias.
    /// </summary>
    /// <param name="combatParent"></param>
    public override void InstantiateMyStuff(RectTransform combatParent)
    {
        RectTransform enemyContainer = Instantiate(enemyPrefab, combatParent).GetComponent<RectTransform>();

        GameObject temp = Instantiate(enemyTitle, enemyContainer.transform);
        title = temp.GetComponent<TextMeshProUGUI>();

        temp = Instantiate(enemyLife, enemyContainer);
        lifeSlider = temp.GetComponentInChildren<Slider>();
        lifeText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        temp = Instantiate(enemyTurnAndWill, enemyContainer);
        turnSlider = temp.GetComponentInChildren<Slider>();
        willText = temp.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        temp = Instantiate(enemyDescription, enemyContainer);
        descriptionText = temp.GetComponent<TextMeshProUGUI>();

        Instantiate(separator, enemyContainer.transform);

        temp = Instantiate(enemyLog, enemyContainer);
        logText = temp.GetComponentInChildren<TextMeshProUGUI>();
    }
}

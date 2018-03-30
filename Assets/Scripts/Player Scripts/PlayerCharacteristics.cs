using UnityEngine;

/// <summary>
/// Las caracteristicas principales del jugador.
/// </summary>
public class PlayerCharacteristics : MonoBehaviour {

    public int defaultStrength = 1;
    [HideInInspector] public int currentStrength;
    public int defaultIntelligence = 1;
    [HideInInspector] public int currentIntelligence;
    public int defaultResistance = 1;
    [HideInInspector] public int currentResistance;
    public int defaultDexterity = 1;
    [HideInInspector] public int currentDexterity;

    /// <summary>
    /// x es infravisión.
    /// y es supravisión.
    /// </summary>
    [HideInInspector] public Vector2 vision;
    public Vector2 defaultVision = new Vector2(-4, 4);
    public Race playerRace;
    public Job playerJob;
    public PlayerOther other;


    public void InitializeCharacteristics()
    {
        ChangeStats();
        vision = defaultVision;
    }

    public void ChangeStats()
    {
        currentDexterity = defaultDexterity;
        currentIntelligence = defaultIntelligence;
        currentResistance = defaultResistance;
        currentStrength = defaultStrength;
    }

    /// <summary>
    /// Cambia los parametros de la visión. El primer factor aumenta o disminuye la infravisión,
    /// el segundo aumenta o disminuye la supravisión.
    /// </summary>
    /// <param name="InfraFactor"></param>
    /// <param name="SupraFactor"></param>
    public void ChangeVision(int InfraFactor, int SupraFactor)
    {
        vision.x = defaultVision.x - InfraFactor;
        vision.y = defaultVision.y + SupraFactor;
    }

}

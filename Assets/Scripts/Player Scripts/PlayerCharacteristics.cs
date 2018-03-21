using UnityEngine;

/// <summary>
/// Las caracteristicas principales del jugador.
/// </summary>
public class PlayerCharacteristics : MonoBehaviour {

    public int strength = 1;
    public int intelligence = 1;
    public int resistance = 1;
    public int dexterity = 1;

    /// <summary>
    /// x es infravisión.
    /// y es supravisión.
    /// </summary>
    [HideInInspector]public Vector2 vision;
    public Vector2 defaultVision = new Vector2(-4, 4);

    public Race playerRace;
    public Job playerJob;
    public PlayerOther other;


    public void InitializeCharacteristics()
    {
        vision = defaultVision;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void ChangeVision(int InfraFactor, int SupraFactor)
    {
        vision.x = defaultVision.x - InfraFactor;
        vision.y = defaultVision.y + SupraFactor;
    }

}

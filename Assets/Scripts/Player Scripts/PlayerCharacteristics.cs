using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour {

    public int strength = 0;
    public int intelligence = 0;
    public int resistance = 0;
    public int dexterity = 0;

    public Vector2 vision = new Vector2(-4, 4);

    public Race playerRace;
    public Job playerJob;
    public PlayerOther other;


}

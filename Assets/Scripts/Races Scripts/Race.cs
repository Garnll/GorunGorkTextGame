using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Race : ScriptableObject {

    public string keyword;
    public string raceDescription;

    public abstract void ChangePlayerStats(PlayerCharacteristics playerStats);
    public abstract void ActivatePassiveHability(PlayerManager player);
}

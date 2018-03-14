using UnityEngine;

public abstract class Race : ScriptableObject {

    public string keyword = "raza";
    [TextArea] public string raceDescription;

    public abstract void ChangePlayerStats(PlayerCharacteristics playerStats);

    public abstract void ActivatePassiveHability(PlayerManager player);
}

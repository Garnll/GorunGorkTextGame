using UnityEngine;

/// <summary>
/// Clase base de las razas. Esqueleto para crear una nueva raza.
/// </summary>
public abstract class Race : ScriptableObject {

    public string keyword = "raza";
    [TextArea] public string raceDescription;

    public abstract void ChangePlayerStats(PlayerCharacteristics playerStats);

    public abstract void ActivatePassiveHability(PlayerManager player);
}

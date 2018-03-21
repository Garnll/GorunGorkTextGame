using UnityEngine;

/// <summary>
/// Hijo de Raza. Controla como el búho cambia los stats, y cuál es su habilidad pasiva.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Races/Owl")]
public class OwlRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.intelligence += 2;
        playerStats.strength -= 1;
    }

    /// <summary>
    /// Aumenta la infravisión y la supravisión en 3.
    /// </summary>
    /// <param name="player"></param>
    public override void ActivatePassiveHability(PlayerManager player)
    {
        player.characteristics.ChangeVision(3, 3);
    }
}

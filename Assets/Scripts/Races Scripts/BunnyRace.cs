using UnityEngine;

/// <summary>
/// Hijo de Raza. Controla como el conejo cambia los stats, y cuál es su habilidad pasiva.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Races/Bunny")]
public class BunnyRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.defaultDexterity += 2;
        playerStats.defaultResistance -= 1;

        playerStats.ChangeStats();
    }

    /// <summary>
    /// Le da una evasión base al jugador.
    /// </summary>
    /// <param name="player"></param>
    public override void ActivatePassiveHability(PlayerManager player)
    {
        player.characteristics.other.currentEvasion = 10;
    }
}

using UnityEngine;

/// <summary>
/// Hijo de Raza. Controla como el oso cambia los stats, y cuál es su habilidad pasiva.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Races/Bear")]
public class BearRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.defaultResistance += 2;
        playerStats.defaultDexterity -= 1;

        playerStats.ChangeStats();
    }

    /// <summary>
    /// Multiplica la regeneración de salud.
    /// </summary>
    /// <param name="player"></param>
    public override void ActivatePassiveHability(PlayerManager player)
    {
        float multiplier = 1;

        if (player.currentHealth >= player.MaxHealth * 0.91f)
        {
            multiplier = 1.1f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.81f)
        {
            multiplier = 1.2f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.71f)
        {
            multiplier = 1.3f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.61f)
        {
            multiplier = 1.4f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.51f)
        {
            multiplier = 1.5f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.41f)
        {
            multiplier = 1.6f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.31f)
        {
            multiplier = 1.7f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.21f)
        {
            multiplier = 1.8f;
        }
        else if (player.currentHealth >= player.MaxHealth * 0.11f)
        {
            multiplier = 1.9f;
        }
        else if (player.currentHealth > player.MaxHealth * 0f)
        {
            multiplier = 2f;
        }

        player.characteristics.other.currentHealthRegenPerSecond =
            player.characteristics.other.DefaultHealthRegenPerSecond * multiplier;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Races/Bear")]
public class BearRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.resistance += 2;
        playerStats.dexterity -= 1;
    }

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

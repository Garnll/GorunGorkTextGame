using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Races/Bunny")]
public class BunnyRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.dexterity += 2;
        playerStats.resistance -= 1;
    }

    public override void ActivatePassiveHability(PlayerManager player)
    {
        player.characteristics.other.currentEvasion = 10;
    }
}

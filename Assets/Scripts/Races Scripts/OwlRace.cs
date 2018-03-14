using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Races/Owl")]
public class OwlRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.intelligence += 2;
        playerStats.strength -= 1;
    }

    public override void ActivatePassiveHability(PlayerManager player)
    {
        player.characteristics.ChangeVision(3, 3);
    }
}

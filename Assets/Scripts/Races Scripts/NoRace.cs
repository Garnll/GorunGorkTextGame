using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Races/No Race")]
public class NoRace : Race
{
    public override void ActivatePassiveHability(PlayerManager player)
    {
        //Nada
    }

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        //Nuh-uh
    }
}

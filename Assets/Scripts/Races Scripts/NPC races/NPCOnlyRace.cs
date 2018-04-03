using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCOnlyRace : Race {

    public override void ActivatePassiveHability(PlayerManager player)
    {
        //DoNothing
    }

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        //DoNothing
    }

    public abstract override void ActivatePassiveHability(EnemyNPC npc);

}

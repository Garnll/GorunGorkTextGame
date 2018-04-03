using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Jobs/NPC Job")]
public class NPCOnlyJob : Job {

    public override void TryToUseHability(string code, PlayerManager player)
    {
        //This really shouldn't do anything
    }
}

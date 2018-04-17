using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Jobs/NPC Job")]
public class NPCOnlyJob : Job {

    public override void CheckLevelPerks(int playerLevel, GameController controller)
    {
        //Really now
    }
}

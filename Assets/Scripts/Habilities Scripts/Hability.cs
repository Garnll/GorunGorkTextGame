using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hability : ScriptableObject {

    public int habilityID;
    public string habilityname;
    [TextArea]public string habilityDescription;

    public int turnConsuption;
    public int cooldownTime;

    public abstract void ImplementHability(PlayerManager player);

}

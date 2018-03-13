using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hability : ScriptableObject {

    public int habilityID = 0;
    public string habilityname = "schupiteiru";
    [TextArea]public string habilityDescription;

    public int turnConsuption = 50;
    public int cooldownTime = 8;

    public int habiltyLevel = 1;

    private bool isAvailable = true;

    public abstract void ImplementHability(PlayerManager player);

    public IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isAvailable = true;
    }

}

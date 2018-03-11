using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : ScriptableObject {

    public int identifier;
    public string jobName;
    [TextArea]public string jobDescription;

    public List<Hability> habilities = new List<Hability>();

    public abstract void TryToUseHability(string code, PlayerManager player);

}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase base de todos los trabajos posibles. Contiene el esqueleto de qué es un trabajo.
/// </summary>
public abstract class Job : ScriptableObject {

    public int identifier = 0;
    public string jobName = "job name";
    [TextArea]public string jobDescription;

    public List<Hability> habilities = new List<Hability>();

    public abstract void TryToUseHability(string code, PlayerManager player);

}

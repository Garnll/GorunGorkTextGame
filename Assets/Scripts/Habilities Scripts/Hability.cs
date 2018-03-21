using System.Collections;
using UnityEngine;

/// <summary>
/// Clase base para cnstruir todas las habilidades del jugador y/o NPCs.
/// </summary>
public abstract class Hability : ScriptableObject {

    public int habilityID = 0;
    public string habilityname = "schupiteiru";
    [TextArea]public string habilityDescription;

    public int turnConsuption = 50;
    public int cooldownTime = 8;

    public int habiltyLevel = 1;

    private bool isAvailable = true;

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe el manager del jugador.
    /// </summary>
    /// <param name="player"></param>
    public abstract void ImplementHability(PlayerManager player);

    protected IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isAvailable = true;
    }

}

using System.Collections;
using UnityEngine;

/// <summary>
/// Clase base para cnstruir todas las habilidades del jugador y/o NPCs.
/// </summary>
public abstract class Hability : ScriptableObject {

    public int habilityID = 0;
    public int jobIdentifier = 0;
    public string habilityName = "schupiteiru";
    [TextArea]public string habilityDescription;

    public int turnConsuption = 50;
    public int cooldownTime = 8;

    public int habiltyLevel = 1;

    protected bool isAvailable = true;

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe el manager del jugador.
    /// </summary>
    /// <param name="player"></param>
    public virtual void ImplementHability(PlayerManager player, NPCTemplate npc)
    {
        //Do something
    }

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe un objeto
    /// </summary>
    /// <param name="interactable"></param>
    public virtual void ImplementHability(InteractableObject interactable)
    {
        //Do something
    }

    protected virtual IEnumerator WaitForCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isAvailable = true;
    }

    public void MakeAvailable(bool isTrue)
    {
        isAvailable = isTrue;
    }
}

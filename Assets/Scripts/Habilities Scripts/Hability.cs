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

    [HideInInspector] public Timer timeWaiter;

    public int turnConsuption = 50;
    public int cooldownTime = 8;
    public CharacterState stateToChange;

    public int habiltyLevel = 1;

    public bool isAvailable = true;

    private void OnEnable()
    {
        habiltyLevel = 1;
        isAvailable = true;
    }

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe el manager del jugador.
    /// </summary>
    /// <param name="player"></param>
    public virtual void ImplementHability(PlayerManager player, EnemyNPC npc)
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

    public virtual void ImplementHability<T>(PlayerManager player, T thing, string[] separatedInputs)
    {
        //Do something
    }

    protected virtual void WaitForCooldown()
    {
        if (timeWaiter == null)
        {
            timeWaiter = GameObject.FindWithTag("Timer").GetComponent<Timer>();
        }

        timeWaiter.StopCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this));
        timeWaiter.StartCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this));
    }

    public void MakeAvailable(bool isTrue)
    {
        isAvailable = isTrue;
    }
}

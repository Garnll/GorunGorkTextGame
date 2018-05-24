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
    public CharacterEffectiveState stateToChange;

    public int habilityLevel = 1;

    public bool isAvailable = true;

    private void OnEnable()
    {
        habilityLevel = 1;
        isAvailable = true;
    }

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe el manager del jugador.
    /// </summary>
    /// <param name="player"></param>
    public virtual void ImplementHability(PlayerManager player, EnemyNPC npc)
    {
        AddExperience();
    }

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe el manager del jugador.
    /// </summary>
    /// <param name="player"></param>
    public virtual void ImplementHability(PlayerManager player, PlayerInstance npc)
    {
        AddExperience();
    }

    /// <summary>
    /// Activar la habilidad, que puede ser en combate o fuera de él. 
    /// Requiere que se le envíe un objeto
    /// </summary>
    /// <param name="interactable"></param>
    public virtual void ImplementHability(PlayerManager player, InteractableObject interactable)
    {
        AddExperience();
    }

    public virtual void ImplementHability<T>(PlayerManager player, T thing, string[] separatedInputs)
    {
        AddExperience();
    }

    protected void AddExperience()
    {

    }

    protected virtual void WaitForCooldown()
    {
        if (timeWaiter == null)
        {
            timeWaiter = Timer.Instance;
        }

        timeWaiter.StopCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this));
        timeWaiter.StartCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this));
    }

    public void MakeAvailable(bool isTrue)
    {
        isAvailable = isTrue;
    }
}

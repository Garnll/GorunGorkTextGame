using UnityEngine;

/// <summary>
/// Base de la creación de NPCs. Para poner todos sus valores y después crear un GameObject a partir de esto.
/// </summary>
public abstract class NPCTemplate : ScriptableObject {

    public string npcName = "Lukashenko";
    public string[] keyword;
    public int npcLevel = 0;
    public string npcGender = "macho";
    public Race npcRace;
    public Job npcJob;
    public int defaultVisibility = 0;

    [HideInInspector] public int currentVisibility;
    [TextArea] public string npcInRoomDescription;
    [TextArea] public string npcDetailedDescription;

    protected bool isAlive;

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }
}

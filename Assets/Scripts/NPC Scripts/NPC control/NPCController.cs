using UnityEngine;

/// <summary>
/// Controlador de todo lo que haría el gamecontroller durante el combate.
/// </summary>
public class NPCController : MonoBehaviour {

    public GameObject combatLayout;
    public RectTransform contentContainer;
    [Space(10)]
    public PlayerUIDuringCombat playerUI;
    public EnemyUIDuringCombat enemyUI;

    private NPCTemplate enemy;
    private PlayerManager player;

    public NPCTemplate TryToFight(string keywordGiven, Room currentRoom)
    {
        for (int i = 0; i < currentRoom.npcsInRoom.Count; i++)
        {
            NPCTemplate npc = currentRoom.npcsInRoom[i];

            if (npc.GetType() == typeof(EnemyNPC))
            {
                foreach (string keyword in npc.keyword)
                {
                    if (keyword == keywordGiven)
                    {
                        return npc;
                    }
                }
            }
        }

        return null;
    }


    public void PrepareFight(NPCTemplate npc, PlayerManager thisPlayer)
    {
        enemy = npc;
        player = thisPlayer;

        GameState.Instance.ChangeCurrentState(GameState.GameStates.combat);
    }

    public void StartFight()
    {
        ChangeLayout();
    }

    private void ChangeLayout()
    {
        GameObject newCombat = Instantiate(combatLayout, contentContainer);

        playerUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
        enemyUI.InstantiateMyStuff(newCombat.GetComponent<RectTransform>());
    }
}

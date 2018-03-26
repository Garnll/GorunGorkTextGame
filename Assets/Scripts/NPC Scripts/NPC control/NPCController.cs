using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public GameObject combatLayout;
    public RectTransform contentContainer;

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
        Instantiate(combatLayout, contentContainer);
    }
}

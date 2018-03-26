using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public GameController controller;
    public GameObject combatLayout;
    public GameObject textLayout;

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

    public void StartFight(NPCTemplate enemyToFight)
    {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.combat);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Races/Bull")]
public class BullRace : Race {

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.strength += 2;
        playerStats.intelligence -= 1;
    }

    public override void ActivatePassiveHability(PlayerManager player)
    {
        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            if (player.currentHealth < player.MaxHealth * 0.5f && player.currentTurn == player.MaxTurn)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                {
                    Debug.Log("Ahora estás en berserk. rawr");
                }
            }
        }
    }
}

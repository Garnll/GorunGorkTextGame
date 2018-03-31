using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Track")]
public class TrackHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Rastrear no disponible.");
                return;
            }
        }

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.controller.combatController.UpdatePlayerLog("¡Has usado Rastrear!");

            enemy.ChangeState(stateToChange);
        }

        WaitForCooldown();
    }
}

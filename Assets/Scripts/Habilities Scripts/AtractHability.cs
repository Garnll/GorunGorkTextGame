using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Atract")]
public class AtractHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Atraer no disponible.");
                return;
            }
        }

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Atraer!");

            enemy.ReceiveDamage(10);
            enemy.currentEvasion = 0;

            float r = Random.Range(0f, 1f);
            if (r <= 0.3f)
            {
                player.ChangeState(stateToChange);
            }
        }

        WaitForCooldown();
    }
}

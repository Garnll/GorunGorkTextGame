using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Emit")]
public class EmitHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Emitir no disponible.");
                return;
            }
        }

        base.ImplementHability(player, enemy);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Emitir!");

            int damage = Random.Range(10, 25);

            enemy.ReceiveDamage(damage);

            float r = Random.Range(0f, 1f);
            if (r <= (0.1f * habilityLevel))
            {
                enemy.ChangeState(stateToChange);
            }
        }

        WaitForCooldown();
    }

    public override void ImplementHability(PlayerManager player, PlayerInstance enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Emitir no disponible.");
                return;
            }
        }

        base.ImplementHability(player, enemy);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Emitir!");
            NetworkManager.Instance.UpdateEnemyLog(player.playerName + " está emitiendo.");

            int damage = Random.Range(10, 25);

            enemy.enemyStats.ReceiveDamage(damage, enemy);

            float r = Random.Range(0f, 1f);
            if (r <= (0.1f * habilityLevel))
            {
                NetworkManager.Instance.ChangeNewState(stateToChange.stateName, enemy);
            }
        }

        WaitForCooldown();
    }
}

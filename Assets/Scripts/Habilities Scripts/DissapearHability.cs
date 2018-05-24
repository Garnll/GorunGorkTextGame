using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Dissapear")]
public class DissapearHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Desaparecer no disponible.");
                return;
            }
        }

        base.ImplementHability(player, enemy);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Desaparecer!");

            player.ChangeState(stateToChange);
        }
        else if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
        {
            player.controller.LogStringWithReturn("Has desaparecido.");
            player.ChangeState(stateToChange);
        }

        WaitForCooldown();
    }

    public override void ImplementHability(PlayerManager player, PlayerInstance enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Desaparecer no disponible.");
                return;
            }
        }

        base.ImplementHability(player, enemy);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Desaparecer!");
            NetworkManager.Instance.UpdateEnemyLog(player.playerName + " acaba de desaparecer.");

            player.ChangeState(stateToChange);
        }
        else if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
        {
            player.controller.LogStringWithReturn("Has desaparecido.");
            player.ChangeState(stateToChange);
        }

        WaitForCooldown();
    }
}

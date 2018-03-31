﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Restart")]
public class RestartHabiity : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Reiniciar no disponible.");
                return;
            }
        }

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.controller.combatController.UpdatePlayerLog("¡Has usado Reiniciar!");
            enemy.ChangeState(stateToChange);
        }

        WaitForCooldown();
    }
}

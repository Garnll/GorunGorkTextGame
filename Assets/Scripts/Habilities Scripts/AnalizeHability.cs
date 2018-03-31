using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Habilidad de delegado. Analizar.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Habilities/AnalizeHability")]
public class AnalizeHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Analizar en cooldown.");
                return;
            }
        }

        string characteristicsEnemy;


        characteristicsEnemy = "Vida: " + enemy.currentHealth.ToString() + ".\n"
            + "Falta implementar cosas que no sabía que existían.";
        //Supondré que debería ser fuerza, destreza, etc. Falta por implementar eso.

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.controller.combatController.enemyUI.descriptionText.text = characteristicsEnemy;
            WaitForCooldown(player);
        }

        WaitForCooldown();
    }

    private IEnumerator WaitForCooldown(PlayerManager player)
    {
        yield return new WaitForSeconds(cooldownTime);
        player.controller.combatController.SetEnemyDescription();
    }

}

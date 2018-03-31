using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Habilidad de delegado. Analizar.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Analize")]
public class AnalizeHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Analizar no disponible.");
                return;
            }
        }

        string characteristicsEnemy;


        characteristicsEnemy = "Fuerza: " + enemy.currentStrength + "\n" +
            "Destreza: " + enemy.currentDexterity + "\n" +
            "Inteligencia: " + enemy.currentIntelligence + "\n" +
            "Resistencia: " + enemy.currentResistance;
        //Supondré que debería ser fuerza, destreza, etc. Falta por implementar eso.

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.controller.combatController.UpdatePlayerLog("¡Has usado Analizar!");
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

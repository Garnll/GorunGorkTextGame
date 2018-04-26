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

        base.ImplementHability(player, enemy);

        isAvailable = false;

        string characteristicsEnemy;


        characteristicsEnemy = "Fuerza: " + enemy.currentStrength + "\n" +
            "Destreza: " + enemy.currentDexterity + "\n" +
            "Inteligencia: " + enemy.currentIntelligence + "\n" +
            "Resistencia: " + enemy.currentResistance;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Analizar!");
            player.controller.combatController.enemyUI.descriptionText.text = characteristicsEnemy;
            WaitForCooldown(player);
        }
        else if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
        {
            player.controller.LogStringWithReturn(TextConverter.MakeFirstLetterUpper(enemy.myTemplate.npcName) +  ":\n" 
                + characteristicsEnemy);
        }

        WaitForCooldown();
    }

    public override void ImplementHability(PlayerManager player, InteractableObject interactable)
    {
        base.ImplementHability(player, interactable);

        player.controller.LogStringWithReturn(interactable.descriptionAtAnalized);
    }

    private void WaitForCooldown(PlayerManager player)
    {
        if (timeWaiter == null)
        {
            timeWaiter = Timer.Instance;
        }
        timeWaiter.StopCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this, player));
        timeWaiter.StartCoroutine(timeWaiter.WaitHabilityCooldown(cooldownTime, this, player));
    }

}

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

            float damage = player.characteristics.currentStrength + Random.Range(1, 5) + 10;

            enemy.ReceiveDamage(damage);
            enemy.currentEvasion = 0;

            float r = Random.Range(0f, 1f);
            if (r <= 0.3f)
            {
                player.ChangeState(stateToChange);
            }
        }

        WaitForCooldown();
    }

    public override void ImplementHability(PlayerManager player, InteractableObject interactable)
    {
        if (interactable.weight > habilityLevel * 10)
        {
            player.controller.LogStringWithReturn("Atraes " + TextConverter.OutputObjectHimOrHer(interactable) + " hacia ti.");
            //Generará carga en el mazo
        }
        else
        {
            player.controller.LogStringWithReturn(TextConverter.MakeFirstLetterUpper(
                TextConverter.OutputObjectHimOrHer(interactable)) 
                + " es muy pesado para ti.");
        }
    }
}

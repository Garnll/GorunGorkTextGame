using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Attack")]
public class AttackInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            NPCTemplate npcToAttack = 
                controller.combatController.TryToFight(separatedInputWords, controller.playerRoomNavigation.currentRoom);

            if (npcToAttack == null)
            {
                controller.LogStringWithReturn("No puedes atacar a " + separatedInputWords[1] + ".");
                return;
            }

            EnemyNPC enemy = controller.playerRoomNavigation.PickAnEnemy((EnemyNPCTemplate)npcToAttack);
            if (enemy == null)
            {
                controller.LogStringWithReturn("No hay un " + separatedInputWords[1] + " al que atacar.");
                return;
            }

            controller.LogStringWithReturn("¡Inicia el combate!");

            TextUserInput.OnFight += StartFight;

            controller.combatController.PrepareFight(enemy, controller.playerManager);
            controller.LogStringWithReturn(" ");
        }
        else
        {
            controller.LogStringWithReturn("Das un puño al aire.");
        }
    }

    private void StartFight(GameController controller)
    {
        controller.combatController.StartCoroutine(controller.combatController.StartFight());
        TextUserInput.OnFight -= StartFight;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Attack")]
public class AttackInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
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

            PlayerInstance player = controller.combatController.TryToFightPlayer(separatedCompleteInputWords,
                controller.playerRoomNavigation.currentRoom);

            if (player != null)
            {
                controller.LogStringWithReturn("Atacas a " + player.playerName);
                NetworkManager.Instance.TryToAttack(player.playerName);

                controller.LogStringWithReturn("¡Inicia el combate!");

                TextUserInput.OnFight += StartFight;

                controller.combatController.PrepareFight(player, controller.playerManager);
                controller.LogStringWithReturn(" ");
                return;
            }

            if (enemy == null && player == null)
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
        if (controller.combatController.vsPlayer)
        {
            NetworkManager.Instance.StartFight(controller.combatController.enemyPlayer.playerName);
        }

        controller.combatController.StartCoroutine(controller.combatController.StartFight());
        TextUserInput.OnFight -= StartFight;
    }
}

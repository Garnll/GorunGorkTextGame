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
                controller.npcController.TryToFight(separatedInputWords[1], controller.playerRoomNavigation.currentRoom);

            if (npcToAttack == null)
            {
                controller.LogStringWithReturn("No puedes atacar a " + separatedInputWords[1] + ".");
                return;
            }

            controller.npcController.StartFight(npcToAttack);
        }
        else
        {
            controller.LogStringWithReturn("Das un puño al aire.");
        }
    }
}

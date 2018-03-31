using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Vertigo")]
public class VertigoState : CharacterState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentTurnRegenPerSecond = 0;
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentTurnRegenPerSecond = 0;
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;


            player.characteristics.other.currentTurnRegenPerSecond = 
                player.characteristics.other.DefaultTurnRegenPerSecond;

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentTurnRegenPerSecond = enemy.myTemplate.DefaultTurnRegenPerSecond;

            enemy.ReturnToNormalState();
        }
    }
}

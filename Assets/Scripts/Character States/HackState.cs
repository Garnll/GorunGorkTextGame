using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Hack")]
public class HackState : CharacterState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentTurnRegenPerSecond = 
                player.characteristics.other.DefaultTurnRegenPerSecond * 0.5f;
            player.characteristics.other.currentHealthRegenPerSecond = 0;
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentTurnRegenPerSecond = enemy.DefaultTurnRegenPerSecond * 0.5f;
            enemy.currentHealthRegenPerSecond = 0;
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentTurnRegenPerSecond =
                player.characteristics.other.DefaultTurnRegenPerSecond;
            player.characteristics.other.currentHealthRegenPerSecond = 
                player.characteristics.other.DefaultHealthRegenPerSecond;

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentTurnRegenPerSecond = enemy.DefaultTurnRegenPerSecond;
            enemy.currentHealthRegenPerSecond = enemy.DefaultTurnRegenPerSecond;

            enemy.ReturnToNormalState();
        }
    }
}

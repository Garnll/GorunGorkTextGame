using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Trail")]
public class TrailState : CharacterEffectiveState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentEvasion = 0;
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentEvasion = 0;
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentEvasion = player.characteristics.other.DefaultEvasion;

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentEvasion = enemy.myTemplate.DefaultEvasion;

            enemy.ReturnToNormalState();
        }
    }
}

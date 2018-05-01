using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Supersonic")]
public class SupersonicState : CharacterEffectiveState
{

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.currentVisibility = 4 + (player.playerLevel/2);

            player.characteristics.other.currentEvasion = 50;
            player.characteristics.other.InvokeRepeating("ReduceEvasionBySecond", 1, 1);

        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentEvasion = 50;
            enemy.InvokeRepeating("ReduceEvasionBySecond", 1, 1);
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.currentVisibility = player.defaultVisibility;

            player.characteristics.other.currentEvasion = player.characteristics.other.DefaultEvasion;
            player.characteristics.other.CancelInvoke();

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;


            enemy.currentEvasion = enemy.myTemplate.DefaultEvasion;
            enemy.CancelInvoke();

            enemy.ReturnToNormalState();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Supersonic")]
public class SupersonicState : CharacterState
{

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentEvasion = 50;
            player.characteristics.other.InvokeRepeating("ReduceEvasionBySecond", 1, 1);

        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentEvasion = 50;
            //Falta qaui
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.other.currentEvasion = player.characteristics.other.DefaultEvasion;
            player.characteristics.other.CancelInvoke();

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;


            enemy.currentEvasion = enemy.DefaultEvasion;
            //Falta qaui

            enemy.ReturnToNormalState();
        }
    }
}

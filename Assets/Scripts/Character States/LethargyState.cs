using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Leathargy")]
public class LethargyState : CharacterState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            for (int i = 0; i < player.characteristics.playerJob.habilities.Count; i++)
            {
                player.characteristics.playerJob.habilities[i].MakeAvailable(false);
            } 

        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            for (int i = 0; i < enemy.habilities.Length; i++)
            {
                enemy.habilities[i].MakeAvailable(false);
            }
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            for (int i = 0; i < player.characteristics.playerJob.habilities.Count; i++)
            {
                player.characteristics.playerJob.habilities[i].MakeAvailable(true);
            }

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            for (int i = 0; i < enemy.habilities.Length; i++)
            {
                enemy.habilities[i].MakeAvailable(true);
            }

            enemy.ReturnToNormalState();
        }
    }
}

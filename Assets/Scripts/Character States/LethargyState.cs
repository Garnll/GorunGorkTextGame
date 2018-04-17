using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Lethargy")]
public class LethargyState : CharacterState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            for (int i = 0; i < player.characteristics.playerJob.jobHabilities.Count; i++)
            {
                player.characteristics.playerJob.jobHabilities[i].MakeAvailable(false);
            } 

        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            for (int i = 0; i < enemy.myTemplate.habilities.Length; i++)
            {
                enemy.myTemplate.habilities[i].MakeAvailable(false);
            }
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            for (int i = 0; i < player.characteristics.playerJob.jobHabilities.Count; i++)
            {
                player.characteristics.playerJob.jobHabilities[i].MakeAvailable(true);
            }

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            for (int i = 0; i < enemy.myTemplate.habilities.Length; i++)
            {
                enemy.myTemplate.habilities[i].MakeAvailable(true);
            }

            enemy.ReturnToNormalState();
        }
    }
}

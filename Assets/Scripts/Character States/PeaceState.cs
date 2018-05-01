using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Pacifier")]
public class PeaceState : CharacterEffectiveState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.pacifier = 0;
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.pacifier = 0;
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.pacifier = 1;

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.pacifier = 1;

            enemy.ReturnToNormalState();
        }
    }
}

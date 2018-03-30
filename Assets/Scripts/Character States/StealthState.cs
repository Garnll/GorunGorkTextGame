using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// No implementada porque los personajes no tiene rango de vision aun 
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Character States/Stealth")]
public class StealthState : CharacterState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;


        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;


        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;


            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;


            enemy.ReturnToNormalState();
        }
    }
}

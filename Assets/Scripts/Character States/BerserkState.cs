using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Character States/Berserk")]
public class BerserkState : CharacterEffectiveState {

    public override void ApplyStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.currentStrength += (int)(player.characteristics.defaultStrength * 0.25f);
            player.characteristics.currentDexterity += (int)(player.characteristics.defaultDexterity * 0.25f);
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentStrength += (int)(enemy.myTemplate.defaultStrength * 0.25f);
            enemy.currentDexterity += (int)(enemy.myTemplate.defaultDexterity * 0.25f);
        }
    }

    public override void DissableStateEffect<T>(T character)
    {
        if (character.GetType() == typeof(PlayerManager))
        {
            PlayerManager player = character as PlayerManager;

            player.characteristics.currentStrength = (player.characteristics.defaultStrength);
            player.characteristics.currentDexterity = (player.characteristics.defaultDexterity);

            player.ReturnToNormalState();
        }
        else if (character.GetType() == typeof(EnemyNPC))
        {
            EnemyNPC enemy = character as EnemyNPC;

            enemy.currentStrength = (enemy.myTemplate.defaultStrength);
            enemy.currentDexterity = (enemy.myTemplate.defaultDexterity);

            enemy.ReturnToNormalState();
        }
    }
}

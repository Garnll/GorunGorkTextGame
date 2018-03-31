using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public IEnumerator WaitHabilityCooldown(int time, Hability wichOne)
    {
        yield return new WaitForSeconds(time);
        wichOne.isAvailable = true;
    }

    public IEnumerator WaitHabilityCooldown(int time, Hability wichOne, PlayerManager player)
    {
        yield return new WaitForSeconds(time);
        player.controller.combatController.SetEnemyDescription();
        wichOne.isAvailable = true;
    }

}

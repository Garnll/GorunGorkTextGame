using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private static Timer instance = null;

    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject newTimer = new GameObject();
                instance = newTimer.AddComponent<Timer>();
            }
            return instance;
        }
    }

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

    public IEnumerator RepositionTime(int time, PlayerManager player)
    {
        yield return new WaitForSeconds(time);
        player.characteristics.other.currentEvasion = player.characteristics.other.DefaultEvasion;
    }
    public IEnumerator RepositionTime(int time, EnemyNPC enemy)
    {
        yield return new WaitForSeconds(time);
        enemy.currentEvasion = enemy.myTemplate.DefaultEvasion;
    }

}

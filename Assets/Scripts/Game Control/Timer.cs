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
                newTimer.name = "Timer";
                instance = newTimer.AddComponent<Timer>();
            }
            return instance;
        }
    }

    public IEnumerator WaitHabilityCooldown(float time, Hability wichOne)
    {
        yield return new WaitForSecondsRealtime(time);
        wichOne.isAvailable = true;
    }

    public IEnumerator WaitHabilityCooldown(float time, Hability wichOne, PlayerManager player)
    {
        yield return new WaitForSecondsRealtime(time);
        player.controller.combatController.SetEnemyDescription();
    }

    public IEnumerator RepositionTime(float time, PlayerManager player)
    {
        yield return new WaitForSecondsRealtime(time);
        player.characteristics.other.currentEvasion = player.characteristics.other.DefaultEvasion;
    }
    public IEnumerator RepositionTime(float time, EnemyNPC enemy)
    {
        yield return new WaitForSecondsRealtime(time);
        enemy.currentEvasion = enemy.myTemplate.DefaultEvasion;
    }

}

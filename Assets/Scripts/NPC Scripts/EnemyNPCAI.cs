using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNPCAI : MonoBehaviour {

    public enum EnemyMode
    {
        Defensive,
        Aggresive,
        Balanced,
        Afraid
    }

    public enum EnemyStates
    {
        Wait,
        Attack,
        Reposition,
        UseHability,
        Escape
    }

    public EnemyNPC myNPC;
    public EnemyMode defaultMode;
    [Space(10)]
    [Range(10, 100)] public float lifeThreshold1 = 50;
    [Range(0, 90)] public float lifeThreshold2 = 25;
    [Range(0, 30)] public float lifeThreshold3 = 0;
    [Space(5)]
    [Range(0, 5)] public float timeBetweenIterations = 0.5f;
    [Space(5)]
    [Range(10, 100)] public float turnThreshold = 100;
    [Range(0, 90)] public float turnThreshold2 = 50;
    [Range(0, 30)] public float turnThreshold3 = 0;

    protected bool isActive = false;
    protected EnemyMode currentMode;
    protected EnemyStates currentState;

    protected List<bool> habilitiesReady = new List<bool>();

    public void StartAI()
    {
        isActive = true;
        currentMode = defaultMode;

        StopCoroutine(WaitBetweenIterations(timeBetweenIterations));
        StartCoroutine(WaitBetweenIterations(timeBetweenIterations));
    }

    public IEnumerator WaitBetweenIterations(float howMuchTime)
    {
        while (isActive)
        {
            yield return new WaitForSeconds(howMuchTime);
            AnalizeSituation();
        }
    }

    public void AnalizeSituation()
    {
        if (myNPC.currentTurn < myNPC.myTemplate.MaxTurn)
        {
            currentState = EnemyStates.Wait;
            return;
        }
    }

    public void CheckHabilities()
    {
        habilitiesReady.Clear();
        for (int i = 0; i < myNPC.myTemplate.habilities.Length; i++)
        {
            habilitiesReady.Add(myNPC.myTemplate.habilities[i].isAvailable);
        }
    }
}

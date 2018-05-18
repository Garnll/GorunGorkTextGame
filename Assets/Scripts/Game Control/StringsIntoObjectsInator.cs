using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringsIntoObjectsInator : MonoBehaviour {

    [SerializeField]
    private Job[] jobs;

    [SerializeField]
    private Race[] races;

    [SerializeField]
    private CharacterEffectiveState[] states;

    public static StringsIntoObjectsInator Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Job JobFromString (string jobName)
    {
        for (int i = 0; i < jobs.Length; i++)
        {
            if (jobName == jobs[i].jobName)
            {
                return jobs[i];
            }
        }

        return null;
    }

    public Race RaceFromString(string raceName)
    {
        for (int i = 0; i < races.Length; i++)
        {
            if (raceName == races[i].raceName)
            {
                return races[i];
            }
        }

        return null;
    }

    public CharacterEffectiveState StateFromString(string stateName)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (stateName == states[i].stateName)
            {
                return states[i];
            }
        }

        return null;
    }
}

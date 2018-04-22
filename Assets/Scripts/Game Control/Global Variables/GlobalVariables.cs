using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables {

    static Dictionary<string, int> globalVariablesDictionary = new Dictionary<string, int>();

    public static void AddNewAs(string variableName, int start)
    {
        if (!globalVariablesDictionary.ContainsKey(variableName))
        {
            globalVariablesDictionary.Add(variableName, start);
        }
        else
        {
            Debug.LogError(variableName + " ya existe en el diccionario global.");
        }
    }

    public static bool ContainsVariable(string variableName)
    {
        if (globalVariablesDictionary.ContainsKey(variableName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int GetValueOf(string variableName)
    {
        return globalVariablesDictionary[variableName];
    }

    public static void SetValue(string variableName, int newvalue)
    {
        if (globalVariablesDictionary.ContainsKey(variableName))
        {
            globalVariablesDictionary[variableName] = newvalue;
        }
    }

}

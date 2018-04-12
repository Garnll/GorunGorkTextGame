using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables {

    static Dictionary<string, int> globalVariablesDictionary = new Dictionary<string, int>();

    public static void AddVariable(string variableName, int start)
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

    public static int GetVariableCurrentValue(string variableName)
    {
        return globalVariablesDictionary[variableName];
    }

    public static void NewVariableValue(string variableName, int newvalue)
    {
        if (globalVariablesDictionary.ContainsKey(variableName))
        {
            globalVariablesDictionary[variableName] = newvalue;
        }
    }

}

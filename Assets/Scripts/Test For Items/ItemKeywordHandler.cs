using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKeywordHandler : SerializedMonoBehaviour {



    public Dictionary<string, InteractableObject[]> itemKeywordDictionary = new Dictionary<string, InteractableObject[]>();

    public InteractableObject[] GetObjectWithNoun(string noun)
    {
        if (itemKeywordDictionary.ContainsKey(noun))
        {
            return itemKeywordDictionary[noun];
        }

        return null;
    }

}

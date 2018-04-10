using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las keywords de cada objeto que se le agregue desde el inspector.
/// Si no se le agrega desde alli, el objeto no existe.
/// </summary>
[ExecuteInEditMode]
public class ItemKeywordHandler : SerializedMonoBehaviour {

    public List<InteractableObject> interactableObjects = new List<InteractableObject>();

    public Dictionary<string, List<InteractableObject>> itemKeywordDictionary = 
        new Dictionary<string, List<InteractableObject>>();

    /// <summary>
    /// Recibe un sustantivo, y detecta si este corresponde a un keyword de objeto.
    /// Si asi es, envía la lista de los objetos con esa misma keyword.
    /// </summary>
    /// <param name="noun"></param>
    /// <returns></returns>
    public InteractableObject[] GetObjectWithNoun(string noun)
    {
        if (itemKeywordDictionary.ContainsKey(noun))
        {
            return itemKeywordDictionary[noun].ToArray();
        }

        return null;
    }

    private void Awake()
    {
#if UNITY_EDITOR
        itemKeywordDictionary.Clear();
#endif

        GetObjectsFromList();
    }

    public void GetObjectsFromList()
    {
        for (int i = 0; i < interactableObjects.Count; i++)
        {
            for (int f = 0; f < interactableObjects[i].nouns.Length; f++)
            {
                if (itemKeywordDictionary.ContainsKey(interactableObjects[i].nouns[f]))
                {
                    bool existsInDictionary = false;
                    for (int g = 0; g < itemKeywordDictionary[interactableObjects[i].nouns[f]].Count; g++)
                    {
                        if (itemKeywordDictionary[interactableObjects[i].nouns[f]][g] == interactableObjects[i])
                        {
                            existsInDictionary = true;
                            break;
                        }
                    }
                    if (!existsInDictionary)
                    {
                        itemKeywordDictionary[interactableObjects[i].nouns[f]].Add(interactableObjects[i]);
                    }
                }
                else
                {
                    itemKeywordDictionary.Add((interactableObjects[i].nouns[f]), 
                        new List<InteractableObject> { interactableObjects[i] });
                }
            }
        }
    }

}

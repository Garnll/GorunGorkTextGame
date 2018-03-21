using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// Contiene las keywords de cada objeto que se le agregue desde el inspector.
/// Si no se le agrega desde alli, el objeto no existe.
/// </summary>
public class ItemKeywordHandler : SerializedMonoBehaviour {

    public Dictionary<string, InteractableObject[]> itemKeywordDictionary = 
        new Dictionary<string, InteractableObject[]>();

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
            return itemKeywordDictionary[noun];
        }

        return null;
    }

}

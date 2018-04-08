using UnityEngine;

/// <summary>
/// Clase base de todos los objetos con los que se pueden interactuar.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Objects/Interactable Object")]
public class InteractableObject : ScriptableObject {

    public enum WordGender
    {
        male,
        female
    }

    public WordGender nounGender = WordGender.male;
    public string[] nouns = { "nombre" };
    [TextArea]public string description = "Descripción en la habitación";
    public Interaction[] interactions;

}

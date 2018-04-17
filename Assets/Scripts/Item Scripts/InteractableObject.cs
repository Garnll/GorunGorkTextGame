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
    public string objectName = "nombre";
    public string[] nouns = { "nombre" };
    public float weight = 10;
    [TextArea] public string description = "Descripción en la habitación";
    [TextArea] public string descriptionAtAnalized = "Descripción al ser analizado";
    [Space(10)]
    public Interaction[] interactions;

}

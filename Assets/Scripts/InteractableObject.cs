using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Interactable Object")]
public class InteractableObject : ScriptableObject {

    public enum WordGender
    {
        male,
        female
    }

    public WordGender nounGender = WordGender.male;
    public string noun = "nombre";
    [TextArea]public string description = "Descripción en la habitación";
    public Interaction[] interactions;

}

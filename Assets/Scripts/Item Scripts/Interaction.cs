using UnityEngine;

/// <summary>
/// Parte de los objetos. Define la respuesta textuales y jugabilisticas de los inputs sobre un objeto en particular.
/// </summary>
[System.Serializable]
public class Interaction  {

    public InputActions inputAction;
    public bool isInverseInteraction; //Cuando se quiere que ocurra una acción especial si no se puede interactuar
    [TextArea] public string textResponse;
    public ActionResponse actionResponse;

}

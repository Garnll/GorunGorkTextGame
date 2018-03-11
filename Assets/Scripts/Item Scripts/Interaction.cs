using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction  {

    public InputActions inputAction;
    public bool isInverseInteraction; //Cuando se quiere que ocurra una acción especial cuando no se pueda interactuar
    [TextArea] public string textResponse;
    public ActionResponse actionResponse;

}

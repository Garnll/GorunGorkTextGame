using UnityEngine;

/// <summary>
/// Parte de la habitación. Es el esqueleto de una salida posible de una habitación.
/// </summary>
[System.Serializable]
public class Exit {

    //public string myKeyword;
    public DirectionKeyword myKeyword;
    public Room conectedRoom;
    [TextArea] public string exitDescription;
}

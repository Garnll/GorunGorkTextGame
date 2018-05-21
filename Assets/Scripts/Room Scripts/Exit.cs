using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Parte de la habitación. Es el esqueleto de una salida posible de una habitación.
/// </summary>
[System.Serializable]
public class Exit {

    [Space(10)]
    public DirectionKeyword myKeyword;
    public RoomObject conectedRoom;
	public bool isExplicit;
	public bool isAble = true;
	[TextArea] public string exitDescription;
    [Space(5)]
    [TextArea] public string exitActionDescription;


}

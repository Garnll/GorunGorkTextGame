using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room Response 
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/RoomObject Responses/Connect To Server")]
public class ConnectToServer : RoomResponse {

	public override void TriggerResponse(GameController controller)
    {
        base.TriggerResponse(controller);

        NetworkManager.Instance.ConnectToServer();
    }
}

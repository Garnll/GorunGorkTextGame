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
        if (NetworkManager.Instance.connected || NetworkManager.Instance.isConnecting)
        {
            return;
        }

        controller.StartCoroutine(controller.StopTextWhileConnecting());

        base.TriggerResponse(controller);

        NetworkManager.Instance.ConnectToServer();
    }
}

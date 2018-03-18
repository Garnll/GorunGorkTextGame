using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Room Responses/Job Selection")]
public class JobSelectionResponse : RoomResponse {

    public Job jobToGive;

    public override void TriggerResponse(GameController controller)
    {
        base.TriggerResponse(controller);

        controller.playerManager.characteristics.playerJob = jobToGive;
    }

}

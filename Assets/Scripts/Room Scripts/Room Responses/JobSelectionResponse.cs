using UnityEngine;

/// <summary>
/// Hijo de Respuesta de la Habitación. Hace que el jugador que pase por la habitación obtenga el trabajo dado.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Room Responses/Job Selection")]
public class JobSelectionResponse : RoomResponse {

    public Job jobToGive;

    public override void TriggerResponse(GameController controller)
    {
        base.TriggerResponse(controller);

        controller.playerManager.characteristics.playerJob = jobToGive;
    }

}

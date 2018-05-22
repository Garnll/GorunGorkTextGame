using UnityEngine;

/// <summary>
/// Hijo de Respuesta de la Habitación. Hace que el jugador que pase por la habitación obtenga el trabajo dado.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/RoomObject Responses/Job Selection")]
public class JobSelectionResponse : RoomResponse {

    public Job jobToGive;

    public override void TriggerResponse(GameController controller)
    {
        base.TriggerResponse(controller);

        controller.playerManager.characteristics.playerJob = jobToGive;
		if (!GlobalVariables.ContainsVariable(jobToGive.jobName)) {
			GlobalVariables.AddNewAs(jobToGive.jobName, 1);
		}
		else {
			GlobalVariables.SetValue(jobToGive.jobName, 1);
		}

		if (!GlobalVariables.ContainsVariable("hasjob")) {
			GlobalVariables.AddNewAs("hasjob", 1);
		}
		else {
			GlobalVariables.SetValue("hasjob", 1);
		}

		Debug.Log("hasjob: " + GlobalVariables.GetValueOf("hasjob"));
		controller.questManager.updateQuests();

	}

}

using System.Text;
using UnityEngine;

/// <summary>
/// Input que el usuario utiliza para examinar objetos y/o personajes.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Examine")]
public class ExamineInput : InputActions {

    /// <summary>
    /// Revisa el input del usuario, y determina si se puede examinar o no lo que se está diciendo.
    /// En ambos casos envía una respuesta.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="separatedInputWords"></param>
    public override void RespondToInput(GameController controller, string[] separatedInputWords, string[] separatedCompleteInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            string noun = separatedInputWords[1];
            string nounWithCapitals = separatedCompleteInputWords[1];

            if (NetworkManager.Instance.playerInstanceManager.playerInstancesOnScene.ContainsKey(nounWithCapitals))
            {
                NetworkManager.OnExamine += ExaminePlayer;
                NetworkManager.Instance.AskForCurrentStats(nounWithCapitals);
                return;
            }

            if (noun == "habitacion" || noun == "" || noun == "lugar")
            {
                controller.LogStringWithReturn(controller.RefreshCurrentRoomDescription());
                return;
            }
        }
        else
        {
            controller.LogStringWithReturn(controller.RefreshCurrentRoomDescription());
            return;
        }

		InteractableObject o = controller.playerManager.inventoryManager.tryOpen(separatedInputWords);

		if (o != null) {
			controller.LogStringWithReturn(o.Open());
		}
		else {
			InteractableObject objectToExamine =
				controller.itemHandler.SearchObjectInRoomOrInventory(separatedInputWords, true, true);

			if (objectToExamine != null) {
				controller.itemHandler.ExamineObject(objectToExamine);
			}
		}
    }

    void ExaminePlayer(PlayerInstance otherPlayer, GameController controller)
    {
        AnalizePlayerInfo(otherPlayer);
        controller.LogStringWithoutReturn(AnalizePlayerInfo(otherPlayer));
    }

    string AnalizePlayerInfo(PlayerInstance player)
    {
        StringBuilder info = new StringBuilder();

        info.Append(player.playerName);
        info.Append("\n");
        info.Append(player.playerRace.raceName);
        info.Append("\n");
        info.Append(player.playerJob.jobName);
        info.Append("\n");
        info.Append(player.playerState.stateName);

        return info.ToString();
    }
}

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
        NetworkManager.OnExamine -= ExaminePlayer;

        controller.LogStringWithoutReturn(AnalizePlayerInfo(otherPlayer, controller));
    }

    string AnalizePlayerInfo(PlayerInstance player, GameController controller)
    {
        StringBuilder info = new StringBuilder();

        info.Append(player.playerName + " es ");

        if (player.playerGender == "hembra")
        {
            info.Append("una " + player.playerRace.raceName + " ");
            if (player.playerJob.GetType() != typeof(NoJob))
            {
                info.Append("que se nota es una " + player.playerJob.jobName + ".");
            }
            else
            {
                info.Append("que se nota recién llegó a este lugar.");
            }
        }
        else
        {
            info.Append("un " + player.playerRace.raceName + " ");
            if (player.playerJob.GetType() != typeof(NoJob))
            {
                info.Append("que se nota es un " + player.playerJob.jobName + ".");
            }
            else
            {
                info.Append("que se nota recién llegó a este lugar.");
            }
        }

        info.Append("\n");

        if (player.playerState.GetType() != typeof(NormalState))
        {
            info.Append("Puedes ver que está en estado de " + player.playerState.stateName + " en este momento.");
        }
        else
        {
            info.Append("Puedes ver que se encuentra relajado ahora mismo.");
        }

        if (player.strength > controller.playerManager.characteristics.currentStrength)
        {
            info.Append("\n");
            info.Append("Notas que es bastante fuerte.");
        }
        if (player.dexterity > controller.playerManager.characteristics.currentDexterity)
        {
            info.Append("\n");
            info.Append("Parece ser muy ágil.");
        }
        if (player.resistance > controller.playerManager.characteristics.currentResistance)
        {
            info.Append("\n");
            info.Append("Ves que podría resistir cualquier golpe.");
        }
        if (player.intelligence > controller.playerManager.characteristics.currentIntelligence)
        {
            info.Append("\n");
            info.Append("Es aparente su gran intelecto.");
        }

        return info.ToString();
    }
}

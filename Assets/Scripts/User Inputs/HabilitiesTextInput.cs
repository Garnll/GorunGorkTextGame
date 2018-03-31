using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitiesTextInput : MonoBehaviour {

	public void CheckHabilitiesInputDuringCombat(string[] separatedInputWords, GameController controller, EnemyNPC enemy)
    {
        Job playerJob = controller.playerManager.characteristics.playerJob;

        if (separatedInputWords.Length < 3)
        {
            TryToSendResponse(controller,
                "Parametros Insuficientes.");
            return;
        }

        if (separatedInputWords[0] != "0")
        {
            TryToSendResponse(controller,
                "Valores invalidos recibidos.");
            return;
        }
        if (separatedInputWords[1] != playerJob.identifier.ToString())
        {
            TryToSendResponse(controller,
                "No se detectó habilidad de " + TextConverter.MakeFirstLetterUpper(playerJob.jobName) + 
                ".");
            return;
        }
        for (int i = 0; i < playerJob.habilities.Count; i++)
        {
            if (separatedInputWords[2] == playerJob.habilities[i].habilityID.ToString())
            {
                if (separatedInputWords.Length == 3)
                {
                    playerJob.habilities[i].ImplementHability(controller.playerManager, enemy);
                }
                else if (separatedInputWords.Length == 4)
                {
                    string[] separatedString = { separatedInputWords[3]};

                    playerJob.habilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
                }
                else if (separatedInputWords.Length == 5)
                {
                    string[] separatedString = { separatedInputWords[3], separatedInputWords[4] };

                    playerJob.habilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
                }

                return;
            }
        }

        TryToSendResponse(controller,
            "Habilidad no detectada.");
    }

    public void CheckHabilitiesInput(string[] separatedInputWords, GameController controller)
    {
        Job playerJob = controller.playerManager.characteristics.playerJob;

        if (separatedInputWords.Length < 3)
        {
            TryToSendResponse(controller,
                "Parametros Insuficientes.");
            return;
        }

        if (separatedInputWords[0] != "0")
        {
            TryToSendResponse(controller,
                "Valores invalidos recibidos.");
            return;
        }
        if (separatedInputWords[1] != playerJob.identifier.ToString())
        {
            TryToSendResponse(controller,
                "No se detectó habilidad de " + TextConverter.MakeFirstLetterUpper(playerJob.jobName) +
                ".");
            return;
        }
        for (int i = 0; i < playerJob.habilities.Count; i++)
        {
            if (separatedInputWords[2] == playerJob.habilities[i].habilityID.ToString())
            {

                return;
            }
        }

        TryToSendResponse(controller,
            "Habilidad no detectada.");
    }

    public void TryToSendResponse(GameController controller, string response)
    {
        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            controller.combatController.UpdatePlayerLog(response);
        }
        else if (GameState.Instance.CurrentState == GameState.GameStates.exploration)
        {
            controller.LogStringWithReturn(response);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitiesTextInput : MonoBehaviour {

	public void CheckHabilitiesInputDuringCombat(string[] separatedInputWords, GameController controller, EnemyNPC enemy)
    {
        Job playerJob = controller.playerManager.characteristics.playerJob;
        char[] habilitiesChars = separatedInputWords[0].ToCharArray();

        if (habilitiesChars.Length == 3)
        {
            string[] tempSeparatedInputWords = new string[separatedInputWords.Length + 3];
            for (int i = 0; i < habilitiesChars.Length; i++)
            {
                tempSeparatedInputWords[i] = habilitiesChars[i].ToString();
            }
            for (int i = 1; i < separatedInputWords.Length; i++)
            {
                tempSeparatedInputWords[i + 2] = separatedInputWords[i];
            }
            separatedInputWords = tempSeparatedInputWords;
        }

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

        for (int i = 0; i < playerJob.unlockedHabilities.Count; i++)
        {
            if (separatedInputWords[2] == playerJob.unlockedHabilities[i].habilityID.ToString())
            {
                if (separatedInputWords.Length == 3)
                {
                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy);
                }
                else if (separatedInputWords.Length == 4)
                {
                    string[] separatedString = { separatedInputWords[3]};

                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
                }
                else if (separatedInputWords.Length == 5)
                {
                    string[] separatedString = { separatedInputWords[3], separatedInputWords[4] };

                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
                }

                return;
            }
        }

        TryToSendResponse(controller,
            "Habilidad no detectada.");
    }


    public void CheckHabilitiesInputDuringCombat(string[] separatedInputWords, GameController controller, PlayerInstance enemy)
    {
        Job playerJob = controller.playerManager.characteristics.playerJob;
        char[] habilitiesChars = separatedInputWords[0].ToCharArray();

        if (habilitiesChars.Length == 3)
        {
            string[] tempSeparatedInputWords = new string[separatedInputWords.Length + 3];
            for (int i = 0; i < habilitiesChars.Length; i++)
            {
                tempSeparatedInputWords[i] = habilitiesChars[i].ToString();
            }
            for (int i = 1; i < separatedInputWords.Length; i++)
            {
                tempSeparatedInputWords[i + 2] = separatedInputWords[i];
            }
            separatedInputWords = tempSeparatedInputWords;
        }

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

        for (int i = 0; i < playerJob.unlockedHabilities.Count; i++)
        {
            if (separatedInputWords[2] == playerJob.unlockedHabilities[i].habilityID.ToString())
            {
                if (separatedInputWords.Length == 3)
                {
                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy);
                }
                else if (separatedInputWords.Length == 4)
                {
                    string[] separatedString = { separatedInputWords[3] };

                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
                }
                else if (separatedInputWords.Length == 5)
                {
                    string[] separatedString = { separatedInputWords[3], separatedInputWords[4] };

                    playerJob.unlockedHabilities[i].ImplementHability(controller.playerManager, enemy, separatedString);
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
        for (int i = 0; i < playerJob.jobHabilities.Count; i++)
        {
            if (separatedInputWords[2] == playerJob.jobHabilities[i].habilityID.ToString())
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

using UnityEngine;

/// <summary>
/// Hijo de Respuesta de la Habitación. Hace que la habitación se convierta en un lugar para crear
/// el personaje y controla casi todos los aspectos de esto.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/RoomObject Responses/Character Creation")]
public class CharacterCreationResponse : RoomResponse {

    public Race[] possibleRaces;

    [TextArea] [SerializeField] private string selectRaceText;
    [TextArea] [SerializeField] private string selectGenderText;
    [TextArea] [SerializeField] private string selectNameText;
    [TextArea] [SerializeField] private string endCreationText;

    private GameController gameController;

    private bool raceSelected = false;
    private bool genderSelected = false;
    private bool nameGiven = false;
    private bool isAskingForConfirmation = false;

    private int tempRaceCode = -1;
    private string tempInput = "";

    public override void TriggerResponse(GameController controller)
    {
        raceSelected = false;
        genderSelected = false;
        nameGiven = false;
        isAskingForConfirmation = false;

        base.TriggerResponse(controller);
        gameController = controller;
        gameController.LogStringAfterRoom(selectRaceText);
    }

    /// <summary>
    /// Acepta el input dado por el usuario y lo redirige ssegún si está eligiendo raza, género o nombre 
    /// (o está dando su confirmación).
    /// </summary>
    /// <param name="input"></param>
    public void AcceptInput(string[] input, string originalInput)
    {
        if (input[0] == "")
        {
            return;
        }

        if (!raceSelected)
        {
            if (!isAskingForConfirmation)
            {
                if (input[0] == "preguntar")
                {
                    gameController.LogStringWithReturn(AskForRaceDescription(input[1]));
                }
                if (input.Length > 1)
                {
                    if (input[1] == "preguntar" && input[0] != "preguntar")
                    {
                        gameController.LogStringWithReturn(AskForRaceDescription(input[0]));
                    }
                }
                else
                {
                    InputPlayerRace(input[0]);
                }
            }
            else
            {
                if (AskForConfirmation(input[0]))
                {
                    Race newRace = possibleRaces[tempRaceCode];
                    gameController.playerManager.SelectRace(newRace);
                    gameController.LogStringWithReturn("Ahora eres un " + newRace.raceName + ".");

                    raceSelected = true;

                    gameController.LogStringWithReturn(selectGenderText);
                }
                else
                {
                    gameController.LogStringWithReturn("Indica nuevamente tu raza: <b><color=#FBEBB5>[Toro, Conejo, Oso, Búho]</color></b>.");
                }
            }
        }
        else if (!genderSelected)
        {
            if (!isAskingForConfirmation)
            {
                InputPlayerGender(input[0]);
            }
            else
            {
                if (AskForConfirmation(input[0]))
                {
                    gameController.playerManager.gender = tempInput;
                    gameController.LogStringWithReturn("Renacerás como " + tempInput + ".");

                    genderSelected = true;

                    gameController.LogStringWithReturn(selectNameText);
                }
                else
                {
                    gameController.LogStringWithReturn("Indica nuevamente tu género: <b><color=#FBEBB5>[Macho, Hembra]</color></b>");
                }
            }
        }
        else if (!nameGiven)
        {
            if (!isAskingForConfirmation)
            {
                InputPlayerName(originalInput);
            }
            else
            {
                if (AskForConfirmation(input[0]))
                {
                    gameController.playerManager.playerName = tempInput;


					gameController.LogStringWithReturn("Ahora te llamas <b><color=#F9EEC1>" + tempInput + "</color></b>.");

                    nameGiven = true;

                    gameController.LogStringWithReturn(endCreationText);
                }
                else
                {
                    gameController.LogStringWithReturn("Indica nuevamente tu nombre:");
                }
            }
        }
        else
        {
            EndResponse();
        }

    }



    private void InputPlayerRace(string raceName)
    {
        for (int i = 0; i < possibleRaces.Length; i++)
        {
            for (int f = 0; f < possibleRaces[i].keyword.Length; f++)
            {
                if (raceName == possibleRaces[i].keyword[f])
                {
                    tempInput = raceName;
                    tempRaceCode = i;
                    gameController.LogStringWithReturn("¿Estás seguro de querer ser " + possibleRaces[i].raceName 
						+ "? <b><color=#FBEBB5>[Sí, No]</color></b>");
                    isAskingForConfirmation = true;
                    return;
                }
            }
        }

        gameController.LogStringWithReturn("Ahora mismo es imposible nacer como \"" + raceName 
            + "\". Intenta de nuevo: <b><color=#FBEBB5>[Toro, Conejo, Oso, Búho]</color></b>.");
    }

    private string AskForRaceDescription(string raceName)
    {
        for (int i = 0; i < possibleRaces.Length; i++)
        {
            for (int f = 0; f < possibleRaces[i].keyword.Length; f++)
            {
                if (raceName == possibleRaces[i].keyword[f])
                {
                    return possibleRaces[i].raceDescription;
                }
            }

        }

        return "No existe nadie tan perfecto como para ser \"" + raceName + "\". " +
            "Por favor, pregunta por una de las razas disponibles.";

    }


    private void InputPlayerGender(string playerGender)
    {
        if (playerGender == "macho" || playerGender == "m")
        {
            isAskingForConfirmation = true;
            tempInput = "macho";
            gameController.LogStringWithReturn("Has elegido renacer como macho. ¿Estás seguro? <b><color=#FBEBB5>[Sí, No]</color></b>");
            return;
        }
        if (playerGender == "hembra" || playerGender == "h")
        {
            isAskingForConfirmation = true;
            tempInput = "hembra";
            gameController.LogStringWithReturn("Has elegido renacer como hembra. ¿Estás segura? <b><color=#FBEBB5>[Sí, No]</color></b>");
            return;
        }

        gameController.LogStringWithReturn("Es comprensivo que quieras ser \"" + playerGender + "\", pero eso solo existe en Tumblr. " +
			"Elige un género entre <b><color=#FBEBB5>[Macho]</color></b> y <b><color=#FBEBB5>[Hembra]</color></b>.");

    }

    private void InputPlayerName(string playerName)
    {
        tempInput = playerName;
        gameController.LogStringWithReturn("Tu nombre a partir de ahora será <b><color=#F9EEC1>\""
			+ playerName
            + "\"</color></b> ¿De acuerdo?. <b><color=#FBEBB5>[Sí, No]</b></color>"
			);
        isAskingForConfirmation = true;
    }



    private bool AskForConfirmation(string playerResponse)
    {
        if (playerResponse == "si" || playerResponse == "s" || 
			playerResponse == "sí" || playerResponse == "Sí" || 
			playerResponse == "Si" || playerResponse == "S" ||
			playerResponse == "SI" || playerResponse == "SÍ" ||
			playerResponse == "sip" || playerResponse == "bueno" || playerResponse == "ok"
			|| playerResponse == "weno")
        {
            isAskingForConfirmation = false;
            return true;
        }
        else if (playerResponse == "no" || playerResponse == "n" ||
			playerResponse == "NO" || playerResponse == "No" || playerResponse == "N" || playerResponse == "nope"
			|| playerResponse == "nop" || playerResponse == "neh" || playerResponse == "nah" || playerResponse == "ño") 
        {
            isAskingForConfirmation = false;
            return false;
        }
        else
        {
            isAskingForConfirmation = false;
            gameController.LogStringWithReturn("Anda, que es una pregunta muy sencilla, solo tienes que decir <b><color=#FBEBB5>[Sí]</color></b>" +
				" o <b><color=#FBEBB5>[No]</color></b>. Volvamos a intentarlo.");
            return false;
        }
    }

    /// <summary>
    /// Termina el momento de creación de personaje.
    /// </summary>
    private void EndResponse()
    {
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
        gameController.playerRoomNavigation.AttemptToChangeRooms(
        gameController.playerRoomNavigation.currentRoom.exits[0].myKeyword);

		if (!GlobalVariables.ContainsVariable("om")) {
			GlobalVariables.AddNewAs("om", 1);
		}
	}
}

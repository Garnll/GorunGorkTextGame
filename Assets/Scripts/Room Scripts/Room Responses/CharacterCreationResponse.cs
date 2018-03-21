﻿using UnityEngine;

/// <summary>
/// Hijo de Respuesta de la Habitación. Hace que la habitación se convierta en un lugar para crear
/// el personaje y controla casi todos los aspectos de esto.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Room Responses/Character Creation")]
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
    public void AcceptInput(string[] input)
    {
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
                    gameController.LogStringWithReturn("Ahora eres un " + newRace.keyword);

                    raceSelected = true;

                    gameController.LogStringWithReturn(selectGenderText);
                }
                else
                {
                    gameController.LogStringWithReturn("Trata de volver a indicar la raza que deseas ser:");
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
                    gameController.LogStringWithReturn("Renaceras como " + tempInput);

                    genderSelected = true;

                    gameController.LogStringWithReturn(selectNameText);
                }
                else
                {
                    gameController.LogStringWithReturn("Trata de volver a indicar el género que deseas ser:");
                }
            }
        }
        else if (!nameGiven)
        {
            if (!isAskingForConfirmation)
            {
                InputPlayerName(input[0]);
            }
            else
            {
                if (AskForConfirmation(input[0]))
                {
                    gameController.playerManager.playerName = tempInput;
                    gameController.LogStringWithReturn("Tu nombre es " + tempInput);

                    nameGiven = true;

                    gameController.LogStringWithReturn(endCreationText);
                }
                else
                {
                    gameController.LogStringWithReturn("Trata de volver a indicar el nombre que deseas tener:");
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
            if (raceName == possibleRaces[i].keyword)
            {
                tempInput = raceName;
                tempRaceCode = i;
                gameController.LogStringWithReturn("¿Estás seguro de querer ser un " + raceName + "?");
                isAskingForConfirmation = true;
                return;
            }
        }

        gameController.LogStringWithReturn("La raza " + raceName 
            + " no está disponible en el sistema. Por favor, elige una de las razas disponibles");
    }

    private string AskForRaceDescription(string raceName)
    {
        for (int i = 0; i < possibleRaces.Length; i++)
        {
            if (raceName == possibleRaces[i].keyword)
            {
                return possibleRaces[i].raceDescription;
            }
        }

        return "La raza " + raceName + " no está disponible en el sistema. " +
            "Por favor, pregunta por una de las razas disponibles";

    }


    private void InputPlayerGender(string playerGender)
    {
        if (playerGender == "macho" || playerGender == "m")
        {
            isAskingForConfirmation = true;
            tempInput = "macho";
            gameController.LogStringWithReturn("Has elegido renacer como macho. ¿Estás seguro?");
            return;
        }
        if (playerGender == "hembra" || playerGender == "h")
        {
            isAskingForConfirmation = true;
            tempInput = "hembra";
            gameController.LogStringWithReturn("Has elegido renacer como hembra. ¿Estás seguro?");
            return;
        }

        gameController.LogStringWithReturn("No se ha detectado el género " + playerGender + ". " +
            "En el sistema existen Macho(m) y Hembra(h).");

    }

    private void InputPlayerName(string playerName)
    {
        tempInput = playerName;
        gameController.LogStringWithReturn("Tu nombre a partir de ahora será \"" 
            + playerName
            + "\". ¿Quieres vivir con este nombre el resto de tu existencia?"
            );
        isAskingForConfirmation = true;
    }



    private bool AskForConfirmation(string playerResponse)
    {
        if (playerResponse == "si" || playerResponse == "s")
        {
            isAskingForConfirmation = false;
            return true;
        }
        else if (playerResponse == "no" || playerResponse == "n")
        {
            isAskingForConfirmation = false;
            return false;
        }
        else
        {
            isAskingForConfirmation = false;
            gameController.LogStringWithReturn("No se ha detectado respuesta SI/NO. Reiniciando.");
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
    }
}

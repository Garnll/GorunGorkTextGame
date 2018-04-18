using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsChanger : MonoBehaviour {

    [SerializeField] [TextArea] string textToDisplay = "Elije entre FUERZA, DESTREZA, INTELIGENCIA o RESISTENCIA.";
    [SerializeField] [TextArea] string endText = "No tienes más puntos para gastar.";

    private int availablePoints = 0;

    public void AcceptInput(string[] characteristic, GameController controller)
    {
        switch (characteristic[0])
        {
            case "fuerza":
            case "fuer":
            case "f":
                if (controller.playerManager.characteristics.AddPointsToDefaultStrength(1))
                {
                    controller.LogStringWithReturn(controller.playerManager.characteristics.defaultStrength.ToString());
                    availablePoints--;
                }
                break;

            case "destreza":
            case "des":
            case "d":
                if (controller.playerManager.characteristics.AddPointsToDefaultDexterity(1))
                {
                    controller.LogStringWithReturn(controller.playerManager.characteristics.defaultDexterity.ToString());
                    availablePoints--;
                }
                break;

            case "inteligencia":
            case "int":
            case "i":
                if (controller.playerManager.characteristics.AddPointsToDefaultIntelligence(1))
                {
                    controller.LogStringWithReturn(controller.playerManager.characteristics.defaultIntelligence.ToString());
                    availablePoints--;
                }
                break;

            case "resistencia":
            case "res":
            case "r":
                if (controller.playerManager.characteristics.AddPointsToDefaultResistance(1))
                {
                    controller.LogStringWithReturn(controller.playerManager.characteristics.defaultResistance.ToString());
                    availablePoints--;
                }
                break;

            default:
                controller.LogStringWithReturn("Elije una caracteristica a mejorar.");
                break;
        }

        if (availablePoints <= 0)
        {
            availablePoints = 0;
            EndCharacteristicLevelUp(controller);
        }
    }

    public void StartCharacteristicLevelUp(int newPoints, GameController controller)
    {
        availablePoints = newPoints;
        controller.LogStringWithoutReturn(textToDisplay);
    }

    private void EndCharacteristicLevelUp(GameController controller)
    {
        controller.LogStringWithReturn(endText);
        GameState.Instance.ChangeCurrentState(GameState.GameStates.exploration);
    }
}

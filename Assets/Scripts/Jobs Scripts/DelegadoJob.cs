using System;
using UnityEngine;

/// <summary>
/// Hijo de Trabajo. Maneja al delegado.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Jobs/Delegado")]
public class DelegadoJob : Job
{

    public override void CheckLevelPerks(int playerLevel, GameController controller)
    {
        switch (playerLevel)
        {
            default:
            case 0:
                unlockedHabilities.Add(jobHabilities[0]);
                controller.LogStringWithoutReturn("Desbloqueaste la habilidad " + jobHabilities[0].habilityName);
                break;

            case 1:
                controller.LogStringWithoutReturn("Puedes mejorar 1 caracteristica... ");
                controller.playerManager.characteristicsChanger.StartCharacteristicLevelUp(1, controller);
                break;

            case 2:
                unlockedHabilities.Add(jobHabilities[1]);
                controller.LogStringWithoutReturn("Desbloqueaste la habilidad " + jobHabilities[1].habilityName);
                break;

            case 3:
                controller.LogStringWithoutReturn("Puedes mejorar 1 caracteristica... ");
                controller.playerManager.characteristicsChanger.StartCharacteristicLevelUp(1, controller);
                break;

            case 4:
                unlockedHabilities.Add(jobHabilities[2]);
                controller.LogStringWithoutReturn("Desbloqueaste la habilidad " + jobHabilities[2].habilityName);
                break;

            case 5:
                controller.LogStringWithoutReturn("Puedes mejorar 2 caracteristicas... ");
                controller.playerManager.characteristicsChanger.StartCharacteristicLevelUp(2, controller);
                break;
        }
    }
}

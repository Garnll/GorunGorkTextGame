using System;
using UnityEngine;

/// <summary>
/// Hijo de Trabajo. Maneja a los operarios.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Jobs/Operario")]
public class OperarioJob : Job
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
                //Hacer algo para que esto se pueda hacer de verdad
                break;

            case 2:
                unlockedHabilities.Add(jobHabilities[1]);
                controller.LogStringWithoutReturn("Desbloqueaste la habilidad " + jobHabilities[1].habilityName);
                break;

            case 3:
                controller.LogStringWithoutReturn("Puedes mejorar 2 caracteristicas... ");
                //Hacer algo para que esto se pueda hacer de verdad
                break;

            case 4:
                unlockedHabilities.Add(jobHabilities[2]);
                controller.LogStringWithoutReturn("Desbloqueaste la habilidad " + jobHabilities[2].habilityName);
                break;

            case 5:
                controller.LogStringWithoutReturn("Puedes mejorar 1 caracteristicas... ");
                //Hacer algo para que esto se pueda hacer de verdad
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Habilities/Modify")]
public class ModifyHability : Hability {

    public override void ImplementHability(PlayerManager player, EnemyNPC enemy)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Reiniciar no disponible.");
                return;
            }
        }

        base.ImplementHability(player, enemy);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            player.controller.combatController.UpdatePlayerLog("¡Has usado Modificar!");

            int pickOne = Random.Range(1, 5);
            int pickTwo = pickOne;

            while (pickTwo == pickOne)
            {
                pickTwo = Random.Range(1, 5);
            }

            float temp;

            switch(pickOne)
            {
                case 1:
                    temp = enemy.currentStrength;

                    switch (pickTwo)
                    {
                        case 1:
                            enemy.currentStrength = 1;
                            break;

                        case 2:
                            enemy.currentStrength = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case 3:
                            enemy.currentStrength = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case 4:
                            enemy.currentStrength = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case 2:
                    temp = enemy.currentDexterity;

                    switch (pickTwo)
                    {
                        case 1:
                            enemy.currentDexterity = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case 2:
                            enemy.currentDexterity = 1;
                            break;

                        case 3:
                            enemy.currentDexterity = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case 4:
                            enemy.currentDexterity = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case 3:
                    temp = enemy.currentIntelligence;

                    switch (pickTwo)
                    {
                        case 1:
                            enemy.currentIntelligence = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case 2:
                            enemy.currentIntelligence = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case 3:
                            enemy.currentIntelligence = 1;
                            break;

                        case 4:
                            enemy.currentIntelligence = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case 4:
                    temp = enemy.currentIntelligence;

                    switch (pickTwo)
                    {
                        case 1:
                            enemy.currentResistance = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case 2:
                            enemy.currentResistance = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case 3:
                            enemy.currentResistance = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case 4:
                            enemy.currentResistance = 1;
                            break;
                    }
                    break;
            }

        }

        WaitForCooldown();
    }

    public override void ImplementHability<T>(PlayerManager player, T thing, string[] separatedInputs)
    {
        if (!isAvailable)
        {
            if (GameState.Instance.CurrentState == GameState.GameStates.combat)
            {
                player.controller.combatController.UpdatePlayerLog("Reiniciar no disponible.");
                return;
            }
        }

        base.ImplementHability(player, thing, separatedInputs);

        isAvailable = false;

        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            player.currentTurn -= turnConsuption;
            EnemyNPC enemy = thing as EnemyNPC;

            player.controller.combatController.UpdatePlayerLog("¡Has usado Modificar!");

            if (separatedInputs.Length < 2)
            {
                player.controller.combatController.UpdatePlayerLog("Parámetros insuficientes.");
                ImplementHability(player, enemy);
                return;
            }

            string statOne = separatedInputs[0];
            string statTwo = separatedInputs[1];

            float temp;

            switch (statOne)
            {
                default:
                    player.controller.combatController.UpdatePlayerLog("Parámetro 1 no aceptado.");
                    ImplementHability(player, enemy);
                    return;

                case "fuerza":
                    temp = enemy.currentStrength;

                    switch (statTwo)
                    {
                        default:
                            player.controller.combatController.UpdatePlayerLog("Parámetro 2 no aceptado.");
                            ImplementHability(player, enemy);
                            return;

                        case "fuerza":
                            enemy.currentStrength = 1;
                            break;

                        case "destreza":
                            enemy.currentStrength = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case "inteligencia":
                            enemy.currentStrength = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case "resistencia":
                            enemy.currentStrength = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case "destreza":
                    temp = enemy.currentDexterity;

                    switch (statTwo)
                    {
                        default:
                            player.controller.combatController.UpdatePlayerLog("Parámetro 2 no aceptado.");
                            ImplementHability(player, enemy);
                            return;

                        case "fuerza":
                            enemy.currentDexterity = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case "destreza":
                            enemy.currentDexterity = 1;
                            break;

                        case "inteligencia":
                            enemy.currentDexterity = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case "resistencia":
                            enemy.currentDexterity = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case "inteligencia":
                    temp = enemy.currentIntelligence;

                    switch (statTwo)
                    {
                        default:
                            player.controller.combatController.UpdatePlayerLog("Parámetro 2 no aceptado.");
                            ImplementHability(player, enemy);
                            return;

                        case "fuerza":
                            enemy.currentIntelligence = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case "destreza":
                            enemy.currentIntelligence = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case "inteligencia":
                            enemy.currentIntelligence = 1;
                            break;

                        case "resistencia":
                            enemy.currentIntelligence = enemy.currentResistance;
                            enemy.currentResistance = temp;
                            break;
                    }
                    break;

                case "resistencia":
                    temp = enemy.currentResistance;

                    switch (statTwo)
                    {
                        default:
                            player.controller.combatController.UpdatePlayerLog("Parámetro 2 no aceptado.");
                            ImplementHability(player, enemy);
                            return;

                        case "fuerza":
                            enemy.currentResistance = enemy.currentStrength;
                            enemy.currentStrength = temp;
                            break;

                        case "destreza":
                            enemy.currentResistance = enemy.currentDexterity;
                            enemy.currentDexterity = temp;
                            break;

                        case "inteligencia":
                            enemy.currentResistance = enemy.currentIntelligence;
                            enemy.currentIntelligence = temp;
                            break;

                        case "resistencia":
                            enemy.currentResistance = 1;
                            break;
                    }
                    break;
            }
        }

        WaitForCooldown();
    }
}

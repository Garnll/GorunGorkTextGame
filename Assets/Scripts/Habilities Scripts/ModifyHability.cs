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

    public override void ImplementHability(PlayerManager player, PlayerInstance enemy)
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
            NetworkManager.Instance.UpdateEnemyLog(player.playerName + " ha modificado algo.");

            int pickOne = Random.Range(1, 5);
            int pickTwo = pickOne;

            while (pickTwo == pickOne)
            {
                pickTwo = Random.Range(1, 5);
            }

            float temp;

            switch (pickOne)
            {
                case 1:
                    temp = enemy.strength;

                    switch (pickTwo)
                    {
                        case 1:
                            NetworkManager.Instance.ChangeStrength(1, enemy);
                            break;

                        case 2:
                            NetworkManager.Instance.ChangeStrength(enemy.dexterity, enemy);
                            NetworkManager.Instance.ChangeDexterity(temp, enemy);
                            break;

                        case 3:
                            NetworkManager.Instance.ChangeStrength(enemy.intelligence, enemy);
                            NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                            break;

                        case 4:
                            NetworkManager.Instance.ChangeStrength(enemy.resistance, enemy);
                            NetworkManager.Instance.ChangeResistance(temp, enemy);
                            break;
                    }
                    break;

                case 2:
                    temp = enemy.dexterity;

                    switch (pickTwo)
                    {
                        case 1:
                            NetworkManager.Instance.ChangeDexterity(enemy.strength, enemy);
                            NetworkManager.Instance.ChangeStrength(temp, enemy);
                            break;

                        case 2:
                            NetworkManager.Instance.ChangeDexterity(1, enemy);
                            break;

                        case 3:
                            NetworkManager.Instance.ChangeDexterity(enemy.intelligence, enemy);
                            NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                            break;

                        case 4:
                            NetworkManager.Instance.ChangeDexterity(enemy.resistance, enemy);
                            NetworkManager.Instance.ChangeResistance(temp, enemy);
                            break;
                    }
                    break;

                case 3:
                    temp = enemy.intelligence;

                    switch (pickTwo)
                    {
                        case 1:
                            NetworkManager.Instance.ChangeIntelligence(enemy.strength, enemy);
                            NetworkManager.Instance.ChangeStrength(temp, enemy);
                            break;

                        case 2:
                            NetworkManager.Instance.ChangeIntelligence(enemy.dexterity, enemy);
                            NetworkManager.Instance.ChangeDexterity(temp, enemy);
                            break;

                        case 3:
                            NetworkManager.Instance.ChangeIntelligence(1, enemy);
                            break;

                        case 4:
                            NetworkManager.Instance.ChangeIntelligence(enemy.resistance, enemy);
                            NetworkManager.Instance.ChangeResistance(temp, enemy);
                            break;
                    }
                    break;

                case 4:
                    temp = enemy.resistance;

                    switch (pickTwo)
                    {
                        case 1:
                            NetworkManager.Instance.ChangeResistance(enemy.strength, enemy);
                            NetworkManager.Instance.ChangeStrength(temp, enemy);
                            break;

                        case 2:
                            NetworkManager.Instance.ChangeResistance(enemy.dexterity, enemy);
                            NetworkManager.Instance.ChangeDexterity(temp, enemy);
                            break;

                        case 3:
                            NetworkManager.Instance.ChangeResistance(enemy.intelligence, enemy);
                            NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                            break;

                        case 4:
                            NetworkManager.Instance.ChangeResistance(1, enemy);
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
            if (thing.GetType() == typeof(PlayerInstance))
            {
                player.currentTurn -= turnConsuption;
                PlayerInstance enemy = thing as PlayerInstance;

                player.controller.combatController.UpdatePlayerLog("¡Has usado Modificar!");
                NetworkManager.Instance.UpdateEnemyLog(player.playerName + " ha modificado algo.");

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
                    case "fuerza":
                        temp = enemy.strength;

                        switch (statTwo)
                        {
                            case "fuerza":
                                NetworkManager.Instance.ChangeStrength(1, enemy);
                                break;

                            case "destreza":
                                NetworkManager.Instance.ChangeStrength(enemy.dexterity, enemy);
                                NetworkManager.Instance.ChangeDexterity(temp, enemy);
                                break;

                            case "inteligencia":
                                NetworkManager.Instance.ChangeStrength(enemy.intelligence, enemy);
                                NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                                break;

                            case "resistencia":
                                NetworkManager.Instance.ChangeStrength(enemy.resistance, enemy);
                                NetworkManager.Instance.ChangeResistance(temp, enemy);
                                break;
                        }
                        break;

                    case "destreza":
                        temp = enemy.dexterity;

                        switch (statTwo)
                        {
                            case "fuerza":
                                NetworkManager.Instance.ChangeDexterity(enemy.strength, enemy);
                                NetworkManager.Instance.ChangeStrength(temp, enemy);
                                break;

                            case "destreza":
                                NetworkManager.Instance.ChangeDexterity(1, enemy);
                                break;

                            case "inteligencia":
                                NetworkManager.Instance.ChangeDexterity(enemy.intelligence, enemy);
                                NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                                break;

                            case "resistencia":
                                NetworkManager.Instance.ChangeDexterity(enemy.resistance, enemy);
                                NetworkManager.Instance.ChangeResistance(temp, enemy);
                                break;
                        }
                        break;

                    case "inteligencia":
                        temp = enemy.intelligence;

                        switch (statTwo)
                        {
                            case "fuerza":
                                NetworkManager.Instance.ChangeIntelligence(enemy.strength, enemy);
                                NetworkManager.Instance.ChangeStrength(temp, enemy);
                                break;

                            case "destreza":
                                NetworkManager.Instance.ChangeIntelligence(enemy.dexterity, enemy);
                                NetworkManager.Instance.ChangeDexterity(temp, enemy);
                                break;

                            case "inteligencia":
                                NetworkManager.Instance.ChangeIntelligence(1, enemy);
                                break;

                            case "resistencia":
                                NetworkManager.Instance.ChangeIntelligence(enemy.resistance, enemy);
                                NetworkManager.Instance.ChangeResistance(temp, enemy);
                                break;
                        }
                        break;

                    case "resistencia":
                        temp = enemy.resistance;

                        switch (statTwo)
                        {
                            case "fuerza":
                                NetworkManager.Instance.ChangeResistance(enemy.strength, enemy);
                                NetworkManager.Instance.ChangeStrength(temp, enemy);
                                break;

                            case "destreza":
                                NetworkManager.Instance.ChangeResistance(enemy.dexterity, enemy);
                                NetworkManager.Instance.ChangeDexterity(temp, enemy);
                                break;

                            case "inteligencia":
                                NetworkManager.Instance.ChangeResistance(enemy.intelligence, enemy);
                                NetworkManager.Instance.ChangeIntelligence(temp, enemy);
                                break;

                            case "resistencia":
                                NetworkManager.Instance.ChangeResistance(1, enemy);
                                break;
                        }
                        break;
                }            
            }

            if (thing.GetType() == typeof(EnemyNPC))
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
        }

        WaitForCooldown();
    }
}

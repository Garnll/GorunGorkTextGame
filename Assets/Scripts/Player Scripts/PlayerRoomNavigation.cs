using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El movimiento del jugador. Este se mueve unicamente entre habitaciones.
/// </summary>
public class PlayerRoomNavigation : MonoBehaviour {

    public GameController controller;
    public Room currentRoom;
    public MiniMapper miniMapper;

    KeywordToStringConverter converter;

    [HideInInspector]public Vector3 currentPosition;

    Dictionary<DirectionKeyword, Exit> exitDictionary = new Dictionary<DirectionKeyword, Exit>();

    Dictionary<string, int> enemyCounter = new Dictionary<string, int>();

    /// <summary>
    /// Le envía al GameController las salidas de la habitación actual.
    /// </summary>
    public void AddExitsToController()
    {
        for (int i = 0; i < currentRoom.exits.Count; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].myKeyword, currentRoom.exits[i]);

            if (currentRoom.exits[i].exitDescription != "")
            {
                controller.exitDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
            }
        }
    }

    public void CheckForNPCs()
    {
        for (int i = 0; i < currentRoom.npcTemplatesInRoom.Count; i++)
        {
            if (currentRoom.npcTemplatesInRoom[i].GetType() != typeof(EnemyNPCTemplate))
            {
                controller.npcDescriptionsInRoom.Add(currentRoom.npcTemplatesInRoom[i].npcInRoomDescription);
            }
        }
        CheckEnemiesInRoom();
    }

    private void CheckEnemiesInRoom()
    {
        if (currentRoom)

        if (currentRoom.enemiesInRoom.Count == 0)
        {
            CreateEnemies();
        }


        for (int i = 0; i < currentRoom.enemiesInRoom.Count; i++)
        {
            if (currentRoom.enemiesInRoom[i].isAlive)
            {
                currentRoom.enemiesInRoom[i].gameObject.SetActive(true);
                if (!enemyCounter.ContainsKey(currentRoom.enemiesInRoom[i].myTemplate.npcName))
                {
                    enemyCounter.Add(currentRoom.enemiesInRoom[i].myTemplate.npcName, 1);
                }
                else
                {
                    enemyCounter[currentRoom.enemiesInRoom[i].myTemplate.npcName]++;
                }
            }
            else
            {
                Destroy(currentRoom.enemiesInRoom[i].gameObject);
                currentRoom.enemiesInRoom.RemoveAt(i);
                i--;
            }
        }

        DisplayEnemies();
    }


    private void DisplayEnemies()
    {
        foreach (string enemy in enemyCounter.Keys)
        {
            switch (enemyCounter[enemy])
            {
                default:
                case 1:
                    controller.npcDescriptionsInRoom.Add("Hay un " + 
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 2:
                    controller.npcDescriptionsInRoom.Add("Hay dos " + 
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 3:
                    controller.npcDescriptionsInRoom.Add("Hay tres " +
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 4:
                    controller.npcDescriptionsInRoom.Add("Hay cuatro " +
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 5:
                    controller.npcDescriptionsInRoom.Add("Hay cinco " +
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 6:
                    controller.npcDescriptionsInRoom.Add("Hay seis " +
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
            }
        }
    }

    private void CreateEnemies()
    {
        int howManyEnemies = 0;

        for (int i = 0; i < currentRoom.npcTemplatesInRoom.Count; i++)
        {
            if (currentRoom.npcTemplatesInRoom[i].GetType() == typeof(EnemyNPCTemplate))
            {
                howManyEnemies++;
            }
        }

        if (howManyEnemies < 1)
        {
            return;
        }

        int exhaust = 0;

        int maxEnemies = Random.Range(0, 5);

        int randomCheck = Random.Range(0, (currentRoom.npcTemplatesInRoom.Count - 1));

        while (currentRoom.enemiesInRoom.Count < maxEnemies && exhaust < 50)
        {
            if (currentRoom.npcTemplatesInRoom[randomCheck].GetType() == typeof(EnemyNPCTemplate))
            {
                EnemyNPCTemplate enemy = currentRoom.npcTemplatesInRoom[randomCheck] as EnemyNPCTemplate;
                GameObject newEnemy = Instantiate(enemy.enemyGameObject, currentRoom.roomPosition, Quaternion.identity);

                newEnemy.name = enemy.npcName;
                newEnemy.GetComponent<EnemyNPC>().myTemplate = enemy;
                currentRoom.enemiesInRoom.Add(newEnemy.GetComponent<EnemyNPC>());
            }
            randomCheck = Random.Range(0, (currentRoom.npcTemplatesInRoom.Count - 1));
            exhaust++;
        }
    }

    public EnemyNPC PickAnEnemy(EnemyNPCTemplate template)
    {
        for (int i = 0; i < currentRoom.enemiesInRoom.Count; i++)
        {
            if (currentRoom.enemiesInRoom[i].myTemplate.npcName == template.npcName)
            {
                return currentRoom.enemiesInRoom[i];
            }
        }
        return null;
    }

    public void HideEnemies()
    {
        for (int i = 0; i < currentRoom.enemiesInRoom.Count; i++)
        {
            if (currentRoom.enemiesInRoom[i].isAlive)
            {
                currentRoom.enemiesInRoom[i].gameObject.SetActive(false);
            }
        }
    }


    public void TriggerRoomResponse()
    {
        if (currentRoom.roomResponse == null)
            return;

        currentRoom.roomResponse.TriggerResponse(controller);
    }

    /// <summary>
    /// Intenta moverse en la dirección dada. Si esta dirección no pertenece a una salida, no se mueve.
    /// </summary>
    /// <param name="directionNoun"></param>
    public void AttemptToChangeRooms(DirectionKeyword directionNoun)
    {
        if (converter == null)
            converter = KeywordToStringConverter.Instance;

        if (exitDictionary.ContainsKey(directionNoun))
        {
            Exit exitToGo = exitDictionary[directionNoun];

            if (exitToGo.exitActionDescription == "")
            {
                controller.LogStringWithReturn("Te dirijes hacia el " + converter.ConvertFromKeyword(directionNoun));
            }
            else
            {
                controller.LogStringWithReturn(exitToGo.exitActionDescription);
            }

            currentRoom = exitDictionary[directionNoun].conectedRoom;

            miniMapper.MovePlayerInMap(currentPosition);

            controller.DisplayRoomText();
        }
        else if (directionNoun != DirectionKeyword.unrecognized)
        {
            controller.LogStringWithReturn("No hay caminos hacia el " + converter.ConvertFromKeyword(directionNoun));
        }
        else
        {
            controller.LogStringWithReturn("Pensaste en una dirección dificil de alcanzar fisicamente...");
        }
    }

    /// <summary>
    /// Da una respuesta erronea por no haber entrado correctamente a dirección
    /// </summary>
    /// <param name="directionNoun"></param>
    public void AttemptToChangeRooms(string directionNoun)
    {
        controller.LogStringWithReturn("Pensaste en una dirección imposible...");
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }

}

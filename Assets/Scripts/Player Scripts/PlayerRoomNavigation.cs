﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El movimiento del jugador. Este se mueve unicamente entre habitaciones.
/// </summary>
public class PlayerRoomNavigation : MonoBehaviour {

    public GameController controller;
    public RoomObject currentRoom;
    public MiniMapper miniMapper;
	public EquipManager equipManager;
	public InventoryManager inventoryManager;

    KeywordToStringConverter converter;
    [HideInInspector]public Vector3 currentPosition;
    Dictionary<DirectionKeyword, Exit> exitDictionary = new Dictionary<DirectionKeyword, Exit>();

    Dictionary<string, int> enemyCounter = new Dictionary<string, int>();
    Dictionary<string, int> enemyVisiblity = new Dictionary<string, int>();

    [Sirenix.OdinInspector.ShowInInspector]
    private Vector3 CurrentPosition
    {
        get
        {
            return currentPosition;
        }
    }

    /// <summary>
    /// Le envía al GameController las salidas de la habitación actual.
    /// </summary>
    public void AddExitsToController()
    {
        for (int i = 0; i < currentRoom.exits.Count; i++)
        {
            exitDictionary.Add(currentRoom.exits[i].myKeyword, currentRoom.exits[i]);
			string temp = "";
			if (currentRoom.exits[i].exitDescription != "")
            {
				if (currentRoom.exits[i].isExplicit && currentRoom.exits[i].isAble) {
					switch (currentRoom.exits[i].myKeyword) {
						case DirectionKeyword.north:
							temp = "<b><color=#FBEBB5>[N]:</color></b> ";
							break;

						case DirectionKeyword.south:
							temp = "<b><color=#FBEBB5>[S]:</color></b> ";
							break;

						case DirectionKeyword.east:
							temp = "<b><color=#FBEBB5>[E]:</color></b> ";
							break;

						case DirectionKeyword.west:
							temp = "<b><color=#FBEBB5>[O]:</color></b> ";
							break;

						case DirectionKeyword.northEast:
							temp = "<b><color=#FBEBB5>[NE]:</color></b> ";
							break;

						case DirectionKeyword.southEast:
							temp = "<b><color=#FBEBB5>[SE]:</color></b> ";
							break;

						case DirectionKeyword.northWest:
							temp = "<b><color=#FBEBB5>[NO]:</color></b> ";
							break;

						case DirectionKeyword.southWest:
							temp = "<b><color=#FBEBB5>[SO]:</color></b> ";
							break;
					}
				}

				if (currentRoom.exits[i].isAble) {
					controller.exitDescriptionsInRoom.Add(temp + currentRoom.exits[i].exitDescription);
				} else {
					if (currentRoom.exits[i].disabledDescription != null && currentRoom.exits[i].disabledDescription != "") {
						controller.exitDescriptionsInRoom.Add(temp + currentRoom.exits[i].disabledDescription);
					}
				}
            }
        }
    }

    public void CheckForNPCs()
    {
        for (int i = 0; i < currentRoom.npcTemplatesInRoom.Count; i++)
        {
            if (currentRoom.npcTemplatesInRoom[i].GetType() != typeof(EnemyNPCTemplate))
            {
                if (currentRoom.npcTemplatesInRoom[i].currentVisibility >= controller.playerManager.characteristics.vision.x &&
                    currentRoom.npcTemplatesInRoom[i].currentVisibility <= controller.playerManager.characteristics.vision.y)
                {
                    controller.npcDescriptionsInRoom.Add(currentRoom.npcTemplatesInRoom[i].npcInRoomDescription);
                }
            }
        }
        CheckEnemiesInRoom();

		foreach (DialogueNPC npc in currentRoom.charactersInRoom) {
			controller.npcDescriptionsInRoom.Add(npc.npcInRoomDescription);
		}
    }

    private void CheckEnemiesInRoom()
    {
        if (currentRoom.enemiesInRoom.Count == 0)
        {
            CreateEnemies();
        }

        enemyCounter.Clear();
        enemyVisiblity.Clear();

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

                if (!enemyVisiblity.ContainsKey(currentRoom.enemiesInRoom[i].myTemplate.npcName))
                {
                    enemyVisiblity.Add(currentRoom.enemiesInRoom[i].myTemplate.npcName, 
                        currentRoom.enemiesInRoom[i].myTemplate.currentVisibility);
                }
                else
                {
                    enemyVisiblity[currentRoom.enemiesInRoom[i].myTemplate.npcName] = 
                        currentRoom.enemiesInRoom[i].myTemplate.currentVisibility;
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

            if (enemyVisiblity[enemy] < controller.playerManager.characteristics.vision.x ||
                enemyVisiblity[enemy] > controller.playerManager.characteristics.vision.y)
            {
                continue;
            }

            switch (enemyCounter[enemy])
            {
                default:
                case 1:
                    controller.npcDescriptionsInRoom.Add("Hay un " + 
                        TextConverter.MakeFirstLetterUpper(enemy) + ".");
                    break;
                case 2:
                    controller.npcDescriptionsInRoom.Add("Hay dos " + 
                        TextConverter.MakeFirstLetterUpper(enemy) + "s.");
                    break;
                case 3:
                    controller.npcDescriptionsInRoom.Add("Hay tres " +
                        TextConverter.MakeFirstLetterUpper(enemy) + "s.");
                    break;
                case 4:
                    controller.npcDescriptionsInRoom.Add("Hay cuatro " +
                        TextConverter.MakeFirstLetterUpper(enemy) + "s.");
                    break;
                case 5:
                    controller.npcDescriptionsInRoom.Add("Hay cinco " +
                        TextConverter.MakeFirstLetterUpper(enemy) + "s.");
                    break;
                case 6:
                    controller.npcDescriptionsInRoom.Add("Hay seis " +
                        TextConverter.MakeFirstLetterUpper(enemy) + "s.");
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

        int maxEnemies = Random.Range(1, 2);

        int randomCheck = Random.Range(0, (currentRoom.npcTemplatesInRoom.Count - 1));

        while (currentRoom.enemiesInRoom.Count < maxEnemies && exhaust < 50)
        {
            if (currentRoom.npcTemplatesInRoom[randomCheck].GetType() == typeof(EnemyNPCTemplate))
            {
                EnemyNPCTemplate enemy = currentRoom.npcTemplatesInRoom[randomCheck] as EnemyNPCTemplate;
                enemy.currentVisibility = enemy.defaultVisibility;

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
		controller.questManager.updateQuests();
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
			foreach (Exit e in currentRoom.exits) {
				if (e.myKeyword == directionNoun && !e.isAble) {
					controller.LogStringWithReturn("No puedes ir en esa dirección.");
					return;
				}
			}

			Exit exitToGo = exitDictionary[directionNoun];

            if (exitToGo.exitActionDescription == "")
            {
                controller.LogStringWithReturn("Te diriges hacia el " + converter.ConvertFromKeyword(directionNoun) + ".");
            }
            else
            {
                controller.LogStringWithReturn(exitToGo.exitActionDescription);
            }

            HideEnemies();

            currentRoom = exitDictionary[directionNoun].conectedRoom;
            currentPosition = currentRoom.roomPosition;

            miniMapper.MovePlayerInMap(currentPosition);
			equipManager.updateText();
			inventoryManager.DisplayInventory();

			if (GlobalVariables.ContainsVariable("diario")) {
				if (GlobalVariables.GetValueOf("diario") == 1) {
					controller.questManager.updateQuests();
				}
			}

			PlayerChangedRooms(); //Añadido para network

            controller.DisplayRoomText();
        }
        else if (directionNoun != DirectionKeyword.unrecognized)
        {
            controller.LogStringWithReturn("No hay caminos hacia el " + converter.ConvertFromKeyword(directionNoun) + ".");
        }
        else
        {
            controller.LogStringWithReturn("Eso sería bastante difícil.");
        }
    }


    /// <summary>
    /// Da una respuesta erronea por no haber entrado correctamente a dirección
    /// </summary>
    /// <param name="directionNoun"></param>
    public void AttemptToChangeRooms(string directionNoun)
    {
        controller.LogStringWithReturn("Eso no es posible.");
    }

    public void MovePlayerToRoom(RoomObject room)
    {
        currentRoom = room;
        currentPosition = currentRoom.roomPosition;

        miniMapper.MovePlayerInMap(currentPosition);
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }


    public void PlayerChangedRooms()
    {
        if (NetworkManager.Instance.connected)
        {
            NetworkManager.Instance.MyPlayerChangedRooms(controller.playerManager.playerName, currentPosition);
        }
    }


    public void CheckPlayersInRoom()
    {
        for (int i = 0; i < currentRoom.playersInRoom.Count; i++)
        {
            if (currentRoom.playersInRoom[i].currentVisibility >= controller.playerManager.characteristics.vision.x &&
                currentRoom.playersInRoom[i].currentVisibility <= controller.playerManager.characteristics.vision.y)
            {
                //Temporal, supongo
                controller.playerDescriptionssInRoom.Add("<b><color=#FFD790>" + currentRoom.playersInRoom[i].playerName + "</color></b> está aqui.");
            }          
        }
    }

    public void ShowPlayersInRoom()
    {
        CheckPlayersInRoom();
        controller.LogStringWithoutReturn(string.Join("\n",controller.playerDescriptionssInRoom.ToArray()));
    }
}

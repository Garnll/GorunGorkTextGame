using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : MonoBehaviour {

    public string playerName = "";
    public int playerUserID;

    public string playerGender = "";
    public int playerLevel = 0;

    public Job playerJob;
    public Race playerRace;
    public CharacterEffectiveState playerState;

    public float strength = 1;
    public float intelligence = 1;
    public float resistance = 1;
    public float dexterity = 1;

    public int currentHealth = 100;

    public int currentVisibility = 0;

    public RoomObject currentRoom;
}

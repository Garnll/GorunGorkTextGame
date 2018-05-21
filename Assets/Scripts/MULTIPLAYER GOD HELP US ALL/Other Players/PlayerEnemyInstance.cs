using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyInstance : MonoBehaviour
{

    public int maxHealth = 100;

    public float currentTurn;
    public int maxTurn;

    public void ReceiveDamage(float damage, PlayerInstance myself)
    {
        NetworkManager.Instance.OtherPlayerReceivedDamage(myself, damage);
    }
}

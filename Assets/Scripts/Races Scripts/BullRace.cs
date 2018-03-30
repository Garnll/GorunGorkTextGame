using UnityEngine;

/// <summary>
/// Hijo de Raza. Controla como el toro cambia los stats, y cuál es su habilidad pasiva.
/// </summary>
[CreateAssetMenu(menuName = "Gorun Gork/Races/Bull")]
public class BullRace : Race {

    public CharacterState berserk;

    public override void ChangePlayerStats(PlayerCharacteristics playerStats)
    {
        playerStats.defaultStrength += 2;
        playerStats.defaultIntelligence -= 1;

        playerStats.ChangeStats();
    }

    /// <summary>
    /// Da una posibilidad de entrar en estado berserk.
    /// </summary>
    /// <param name="player"></param>
    public override void ActivatePassiveHability(PlayerManager player)
    {
        if (GameState.Instance.CurrentState == GameState.GameStates.combat)
        {
            if (player.currentHealth < player.MaxHealth * 0.5f && player.currentTurn == player.MaxTurn)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.5f)
                {
                    player.ChangeState(berserk);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Objects/Action Responses/Stat Change")]
public class StatChangeResponse : ActionResponse {

    public enum Stats
    {
        Strength,
        Dexterity,
        Resistance,
        Intelligence
    }

    [System.Serializable]
    public class StatToChange
    {
        public Stats stat;
        public int points;
    }

    public StatToChange[] statsToChange;

    public override bool DoActionResponse(GameController controller)
    {
        for (int i = 0; i < statsToChange.Length; i++)
        {
            switch (statsToChange[i].stat)
            {
                case Stats.Dexterity:
                    controller.playerManager.characteristics.defaultDexterity += statsToChange[i].points;
                    break;
                case Stats.Intelligence:
                    controller.playerManager.characteristics.defaultIntelligence += statsToChange[i].points;
                    break;
                case Stats.Resistance:
                    controller.playerManager.characteristics.defaultResistance += statsToChange[i].points;
                    break;
                case Stats.Strength:
                    controller.playerManager.characteristics.defaultStrength += statsToChange[i].points;
                    break;
            }   
        }

        controller.playerManager.characteristics.ChangeStats();

        return true;
    }
}

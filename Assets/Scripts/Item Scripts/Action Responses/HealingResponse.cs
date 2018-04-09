using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Objects/Action Responses/Healing")]
public class HealingResponse : ActionResponse {

    public float lifeToHeal = 5;

    public override bool DoActionResponse(GameController controller)
    {
        controller.playerManager.currentHealth += lifeToHeal;
        if (controller.playerManager.currentHealth > controller.playerManager.MaxHealth)
        {
            controller.playerManager.currentHealth = controller.playerManager.MaxHealth;
        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/Objects/Interactable Consumable Object")]
public class InteractableConsumableObject : InteractableObject {

    [SerializeField] [TextArea] private string stopWorkingDescription;
    [SerializeField] private int maxUses = 3;
    private int uses = -5;
    private bool useful;

    public int CurrentUses
    {
        get
        {
            InitializeObject();
            return uses;
        }
    }

    public bool IsUseful
    {
        get
        {
            InitializeObject();
            return useful;
        }
    }


    private void InitializeObject()
    {
        if (uses < 0)
        {
            uses = maxUses;
            useful = true;
        }
    }

    public void UseObject()
    {
        InitializeObject();

        if (!useful)
        {
            return;
        }
        uses--;

        if (uses == 0)
        {
            StopUsefulness();
        }
    }

    private void StopUsefulness()
    {
        uses = 0;
        useful = false;
    }

    public void StopWorking(GameController controller)
    {
        controller.LogStringWithReturn(stopWorkingDescription);
    }


}

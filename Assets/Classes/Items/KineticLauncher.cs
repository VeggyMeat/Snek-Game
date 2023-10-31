using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KineticLauncher : Item
{
    private float damageModifier;

    private bool buffing = false;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/KineticLauncher.json";

        base.Setup();

        if (ItemManager.headController.Turning)
        {
            StartBuffing();
        }

        TriggerManager.StopTurningTrigger.AddTrigger(StopBuffing);
        TriggerManager.StartTurningTrigger.AddTrigger(StartBuffing);
    }

    private int StartBuffing(int value = 0)
    {
        Debug.Log("buffing");

        if (buffing)
        {
            return value;
        }

        BuffAllBodies(false);

        buffing = true;

        return value;
    }

    private int StopBuffing(int value = 0)
    {
        Debug.Log("not buffing");

        if (!buffing)
        {
            return value;
        }

        BuffAllBodies(true);

        buffing = false;

        return value;
    }

    private void BuffAllBodies(bool unbuff)
    {
        BodyController bodyController = ItemManager.headController.head;
        while (bodyController is not null)
        {
            if (unbuff)
            {
                UnBuffBody(bodyController);
            }
            else
            {
                BuffBody(bodyController);
            }

            bodyController = bodyController.next;
        }
    }

    private BodyController BuffBody(BodyController bodyController)
    {
        bodyController.damageBuff.AddBuff(damageModifier, true, null);

        return bodyController;
    }

    private void UnBuffBody(BodyController bodyController)
    {
        bodyController.damageBuff.AddBuff(1 / damageModifier, true, null);
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(damageModifier)))
        {
            if (jsonLoaded)
            {
                StopBuffing();
            }

            damageModifier = float.Parse(jsonVariables[nameof(damageModifier)].ToString());

            if (jsonLoaded)
            {
                if (ItemManager.headController.Turning)
                {
                    StartBuffing();
                }
            }
        }
    }
}

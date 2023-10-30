using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstoneHelmet : Item
{
    private float healthPercentagePerBody;

    private int deadBodies = 0;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/HeadstoneHelmet.json";

        base.Setup();

        // counts the number of dead bodies
        BodyController currentBody = ItemManager.headController.head;
        while (currentBody is not null)
        {
            if (currentBody.IsDead)
            {
                deadBodies++;
            }

            currentBody = currentBody.next;
        }

        AddBuff();

        TriggerManager.BodyDeadTrigger.AddTrigger(Increase);
        TriggerManager.BodyRevivedTrigger.AddTrigger(Decrease);
    }

    private GameObject Increase(GameObject bodyObj)
    {
        RemoveBuff();

        deadBodies++;

        AddBuff();

        return bodyObj;
    }

    private BodyController Decrease(BodyController bodyController)
    {
        RemoveBuff();

        deadBodies--;

        AddBuff();

        return bodyController;
    }

    private void AddBuff()
    {
        ItemManager.headController.head.healthBuff.AddBuff(1 + healthPercentagePerBody * deadBodies, true, null);
    }

    private void RemoveBuff()
    {
        ItemManager.headController.head.healthBuff.AddBuff(1 / (1 + healthPercentagePerBody * deadBodies), true, null);
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(healthPercentagePerBody)))
        {
            RemoveBuff();

            healthPercentagePerBody = float.Parse(jsonVariables[nameof(healthPercentagePerBody)].ToString());

            AddBuff();
        }
    }
}

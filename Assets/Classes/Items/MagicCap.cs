using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCap : Item
{
    private float damageMultiplier;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/MagicCap.json";

        base.Setup();

        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);

        BuffAllBodies();
    }

    private BodyController BuffBody(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Mage)))
        {
            bodyController.damageBuff.AddBuff(damageMultiplier, true, null);
        }

        return bodyController;
    }

    private void BuffAllBodies()
    {
        BodyController bodyController = ItemManager.headController.head;

        while (bodyController is not null)
        {
            BuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref damageMultiplier, nameof(damageMultiplier));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCap : Item
{
    private float damageMultiplier;

    private int mageKillsLevelUp;
    private int mageKills = 0;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/MagicCap.json";

        base.Setup();

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKilled);

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

    private GameObject OnBodyKilled(GameObject body)
    {
        // gets the bodyController from the body
        BodyController bodyClass = body.GetComponent<BodyController>();

        if (bodyClass.classNames.Contains(nameof(Archer)))
        {
            // its an archer, so add to the archer kills
            mageKills++;
        }

        // if the archer kills is greater than the level up amount
        if (mageKills >= mageKillsLevelUp && Levelable)
        {
            // level up
            LevelUp();
        }

        return body;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref damageMultiplier, nameof(damageMultiplier));
        jsonVariables.Setup(ref mageKillsLevelUp, nameof(mageKillsLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            mageKills -= mageKillsLevelUp;
        }

        base.LevelUp();
    }
}

using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class EnchantersTome : Item
{
    private float enchanterHPMultiplier;
    private float enchanterDamageMultiplier;

    private float HPBuff
    {
        get
        {
            return 1 + enchanterHPMultiplier * enchanters;
        }
    }

    private float DamageBuff
    {
        get
        {
            return 1 + enchanterDamageMultiplier * enchanters;
        }
    }

    private int enchanters = 0;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/EnchantersTome.json";

        base.Setup();

        // counts the number of enchanters
        BodyController bodyController = ItemManager.headController.head;
        while (bodyController is not null)
        {
            IncreaseEnchanters(bodyController);

            bodyController = bodyController.next;
        }

        // buffs all the bodies
        BuffAllBodies();

        // sets up all the triggers
        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseEnchanters);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseEnchanters);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseEnchanters);
    }

    private BodyController BuffBody(BodyController bodyController)
    {
        // adds the buffs to the body
        bodyController.healthBuff.AddBuff(HPBuff, true, null);
        bodyController.damageBuff.AddBuff(DamageBuff, true, null);

        return bodyController;
    }

    private void UnBuffBody(BodyController bodyController)
    {
        // removes the buffs from the body
        bodyController.healthBuff.AddBuff(1/HPBuff, true, null);
        bodyController.damageBuff.AddBuff(1/DamageBuff, true, null);
    }

    private BodyController IncreaseEnchanters(BodyController bodyController)
    {
        // checks if the body is an enchanter
        if (bodyController.classNames.Contains(nameof(Enchanter)))
        {
            // if so remove all buffs, increase the number of enchanters, then buff everything
            UnBuffAllBodies();

            enchanters++;

            BuffAllBodies();
        }

        return bodyController;
    }

    private BodyController DecreaseEnchanters(BodyController bodyController)
    {
        // checks if the body is an enchanter
        if (bodyController.classNames.Contains(nameof(Enchanter)))
        {
            // if so remove all buffs, decrease the number of enchanters, then buff everything
            UnBuffAllBodies();

            enchanters--;

            BuffAllBodies();
        }

        return bodyController;
    }

    private void BuffAllBodies()
    {
        // gets the head of the snake
        BodyController bodyController = ItemManager.headController.head;

        // goes through each body in the snake buffing it
        while (bodyController is not null)
        {
            BuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    private void UnBuffAllBodies()
    {
        // gets the head of the snake
        BodyController bodyController = ItemManager.headController.head;

        // goes through each body in the snake, removing the buff from it
        while (bodyController is not null)
        {
            UnBuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(enchanterHPMultiplier)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            enchanterHPMultiplier = float.Parse(jsonVariables[nameof(enchanterHPMultiplier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }

        if (jsonVariables.ContainsKey(nameof(enchanterDamageMultiplier)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            enchanterDamageMultiplier = float.Parse(jsonVariables[nameof(enchanterDamageMultiplier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }
    }
}

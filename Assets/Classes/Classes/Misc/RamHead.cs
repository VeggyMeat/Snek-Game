using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamHead : Class
{
    private float contactForceMultiplier;
    private float contactDamageMultiplier;
    private float defenceAdded;
    private float healthAdded;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/RamHead.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        body.classNames.Add("Misc");

        base.Setup();

        BuffHead(0);

        TriggerManager.PostBodyMoveTrigger.AddTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.AddTrigger(UnBuffHead);
    }

    /// <summary>
    /// Buffs the head
    /// </summary>
    private int BuffHead(int _)
    {
        // adds a health buff to the head
        body.snake.head.healthBuff.AddBuff(healthAdded, false, null);

        // adds a defence buff to the head
        body.snake.head.defenceBuff.AddBuff(defenceAdded, false, null);

        // increases the contactDamage
        body.snake.head.ContactDamage = (int)(body.snake.head.ContactDamage * contactDamageMultiplier);

        // increases the contactForce
        body.snake.head.ContactForce = (int)(body.snake.head.ContactForce * contactForceMultiplier);

        return _;
    }

    /// <summary>
    /// Removes the buffs from the head
    /// </summary>
    private int UnBuffHead(int _)
    {
        // adds a negative health buff to the head
        body.snake.head.healthBuff.AddBuff(-healthAdded, false, null);

        // adds a negative defence buff to the head
        body.snake.head.defenceBuff.AddBuff(-defenceAdded, false, null);

        // decreases the contactDamage
        body.snake.head.ContactDamage = (int)(body.snake.head.ContactDamage / contactDamageMultiplier);

        // decreases the contactForce
        body.snake.head.ContactForce = (int)(body.snake.head.ContactForce / contactForceMultiplier);

        return _;
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        UnBuffHead(0);

        TriggerManager.PostBodyMoveTrigger.RemoveTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.RemoveTrigger(UnBuffHead);
    }

    internal override void Revived()
    {
        base.Revived();

        BuffHead(0);

        TriggerManager.PostBodyMoveTrigger.AddTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.AddTrigger(UnBuffHead);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref contactDamageMultiplier, "contactDamageMultiplier");
        jsonData.Setup(ref contactForceMultiplier, "contactForceMultiplier");
        jsonData.Setup(ref healthAdded, "healthAdded");
        jsonData.Setup(ref defenceAdded, "defenceAdded");

        if (jsonLoaded)
        {
            UnBuffHead(0);
            BuffHead(0);
        }
    }
}

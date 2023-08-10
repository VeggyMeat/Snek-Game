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
        body.classNames.Add("misc");

        base.Setup();

        BuffHead();
    }

    /// <summary>
    /// Buffs the head
    /// </summary>
    private void BuffHead()
    {
        // adds a health buff to the head
        body.snake.head.healthBuff.AddBuff(healthAdded, false, null);

        // adds a defence buff to the head
        body.snake.head.defenceBuff.AddBuff(defenceAdded, false, null);

        // increases the contactDamage
        body.snake.head.ContactDamage = (int)(body.snake.head.ContactDamage * contactDamageMultiplier);

        // increases the contactForce
        body.snake.head.ContactForce = (int)(body.snake.head.ContactForce * contactForceMultiplier);
    }

    /// <summary>
    /// Removes the buffs from the head
    /// </summary>
    private void UnBuffHead()
    {
        // adds a negative health buff to the head
        body.snake.head.healthBuff.AddBuff(-healthAdded, false, null);

        // adds a negative defence buff to the head
        body.snake.head.defenceBuff.AddBuff(-defenceAdded, false, null);

        // decreases the contactDamage
        body.snake.head.ContactDamage = (int)(body.snake.head.ContactDamage / contactDamageMultiplier);

        // decreases the contactForce
        body.snake.head.ContactForce = (int)(body.snake.head.ContactForce / contactForceMultiplier);
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        UnBuffHead();
    }

    internal override void Revived()
    {
        base.Revived();

        BuffHead();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref contactDamageMultiplier, "contactDamageMultiplier");
        jsonData.Setup(ref contactForceMultiplier, "contactForceMultiplier");
        jsonData.Setup(ref healthAdded, "healthAdded");
        jsonData.Setup(ref defenceAdded, "defenceAdded");

        if (body.Level != 1)
        {
            UnBuffHead();
            BuffHead();
        }
    }
}

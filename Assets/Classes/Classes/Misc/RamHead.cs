using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The ram head class, a miscellanious class
/// </summary>
internal class RamHead : Class
{
    /// <summary>
    /// The contact force multiplier for the head
    /// </summary>
    private float contactForceMultiplier;

    /// <summary>
    /// The contact damage multiplier for the head
    /// </summary>
    private float contactDamageMultiplier;

    /// <summary>
    /// The defence added onto the head
    /// </summary>
    private float defenceAdded;

    /// <summary>
    /// The health added onto the head
    /// </summary>
    private float healthAdded;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/RamHead.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // indicates that this is a misc class
        body.classNames.Add("Misc");

        base.Setup();

        BuffHead(0);

        // adds the triggers to buff and debuff the head before and after reorganising the body
        TriggerManager.PostBodyMoveTrigger.AddTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.AddTrigger(UnBuffHead);
    }

    /// <summary>
    /// Buffs the head
    /// </summary>
    private int BuffHead(int _)
    {
        body.snake.Head.healthBuff.AddBuff(healthAdded, false, null);
        body.snake.Head.defenceBuff.AddBuff(defenceAdded, false, null);
        body.snake.Head.ContactDamage = (int)(body.snake.Head.ContactDamage * contactDamageMultiplier);
        body.snake.Head.ContactForce = (int)(body.snake.Head.ContactForce * contactForceMultiplier);

        return _;
    }

    /// <summary>
    /// Removes the buffs from the head
    /// </summary>
    private int UnBuffHead(int _)
    {
        body.snake.Head.healthBuff.AddBuff(-healthAdded, false, null);
        body.snake.Head.defenceBuff.AddBuff(-defenceAdded, false, null);
        body.snake.Head.ContactDamage = (int)(body.snake.Head.ContactDamage / contactDamageMultiplier);
        body.snake.Head.ContactForce = (int)(body.snake.Head.ContactForce / contactForceMultiplier);

        return _;
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        UnBuffHead(0);

        // removes the triggers
        TriggerManager.PostBodyMoveTrigger.RemoveTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.RemoveTrigger(UnBuffHead);
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        BuffHead(0);

        // adds the triggers back
        TriggerManager.PostBodyMoveTrigger.AddTrigger(BuffHead);
        TriggerManager.PreBodyMoveTrigger.AddTrigger(UnBuffHead);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref contactDamageMultiplier, nameof(contactDamageMultiplier));
        jsonData.Setup(ref contactForceMultiplier, nameof(contactForceMultiplier));
        jsonData.Setup(ref healthAdded, nameof(healthAdded));
        jsonData.Setup(ref defenceAdded, nameof(defenceAdded));

        if (jsonLoaded)
        {
            UnBuffHead(0);
            BuffHead(0);
        }
    }
}

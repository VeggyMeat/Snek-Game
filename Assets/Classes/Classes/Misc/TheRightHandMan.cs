using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The right hand man class, a miscellanious class
/// </summary>
internal class TheRightHandMan : Class
{
    /// <summary>
    /// The damage multiplier for the head
    /// </summary>
    private float headDamageMultiplier;

    /// <summary>
    /// The speed multiplier for the head
    /// </summary>
    private float headSpeedMultiplier;

    /// <summary>
    /// The attack speed multiplier for the head
    /// </summary>
    private float headAttackSpeedMultiplier;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/TheRightHandMan.json";

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

        // buffs the head
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
        // adds the buffs
        body.snake.Head.damageBuff.AddBuff(headDamageMultiplier, true, null);
        body.snake.Head.speedBuff.AddBuff(headSpeedMultiplier, true, null);
        body.snake.Head.attackSpeedBuff.AddBuff(headAttackSpeedMultiplier, true, null);

        return _;
    }

    /// <summary>
    /// Removes the buffs from the head
    /// </summary>
    private int UnBuffHead(int _)
    {
        // removes the buffs
        body.snake.Head.damageBuff.AddBuff(1 / headDamageMultiplier, true, null);
        body.snake.Head.speedBuff.AddBuff(1 / headSpeedMultiplier, true, null);
        body.snake.Head.attackSpeedBuff.AddBuff(1 / headAttackSpeedMultiplier, true, null);

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

        jsonData.Setup(ref headDamageMultiplier, nameof(headDamageMultiplier));
        jsonData.Setup(ref headSpeedMultiplier, nameof(headSpeedMultiplier));
        jsonData.Setup(ref headAttackSpeedMultiplier, nameof(headAttackSpeedMultiplier));

        if (jsonLoaded)
        {
            UnBuffHead(0);
            BuffHead(0);
        }
    }
}

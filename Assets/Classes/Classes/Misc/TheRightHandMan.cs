using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheRightHandMan : Class
{
    private float headDamageMultiplier;
    private float headSpeedMultiplier;
    private float headAttackSpeedMultiplier;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/TheRightHandMan.json";

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
        // adds the buffs
        body.snake.head.damageBuff.AddBuff(headDamageMultiplier, true, null);
        body.snake.head.speedBuff.AddBuff(headSpeedMultiplier, true, null);
        body.snake.head.attackSpeedBuff.AddBuff(headAttackSpeedMultiplier, true, null);

        return _;
    }

    /// <summary>
    /// Removes the buffs from the head
    /// </summary>
    private int UnBuffHead(int _)
    {
        // removes the buffs
        body.snake.head.damageBuff.AddBuff(1 / headDamageMultiplier, true, null);
        body.snake.head.speedBuff.AddBuff(1 / headSpeedMultiplier, true, null);
        body.snake.head.attackSpeedBuff.AddBuff(1 / headAttackSpeedMultiplier, true, null);

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

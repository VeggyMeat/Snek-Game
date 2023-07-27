using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyFlask : Item
{
    // currently dodges no matter what, rather than just on a death hit

    private float dodgeChance;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/LuckyFlask.json";

        base.Setup();

        TriggerManager.BodyLostHealthTrigger.AddTrigger(OnBodyHit);
    }

    private int OnBodyHit(int damage)
    {
        if (Random.Range(0f, 1f) < dodgeChance)
        {
            return 0;
        }

        return damage;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref dodgeChance, nameof(dodgeChance));
    }
}

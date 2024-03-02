using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyFlask : Item
{
    // currently dodges no matter what, rather than just on a death hit

    private float dodgeChance;

    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/LuckyFlask.json";

        base.Setup(gameSetup);

        TriggerManager.BodyLostHealthTrigger.AddTrigger(OnBodyHit);
    }

    private (BodyController, int) OnBodyHit((BodyController, int) info)
    {
        if (Random.Range(0f, 1f) < dodgeChance)
        {
            return (info.Item1, 0);
        }

        return info;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref dodgeChance, nameof(dodgeChance));
    }
}

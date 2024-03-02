using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstoneHelmet : Item
{
    private float healthPercentagePerBody;

    private int deadBodies = 0;

    protected int bodiesDeadCount = 0;
    protected int bodiesDeadLevelUp;

    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/HeadstoneHelmet.json";

        base.Setup(gameSetup);

        // counts the number of dead bodies
        BodyController currentBody = gameSetup.HeadController.Head;
        while (currentBody is not null)
        {
            if (currentBody.IsDead)
            {
                deadBodies++;
            }

            currentBody = currentBody.next;
        }

        AddBuff();

        TriggerManager.BodyDeadTrigger.AddTrigger(Increase);
        TriggerManager.BodyRevivedTrigger.AddTrigger(Decrease);
    }

    private BodyController Increase(BodyController bodyController)
    {
        RemoveBuff();

        deadBodies++;
        bodiesDeadCount++;

        AddBuff();

        if (bodiesDeadCount >= bodiesDeadLevelUp && Levelable)
        {
            LevelUp();
        }

        return bodyController;
    }

    private BodyController Decrease(BodyController bodyController)
    {
        RemoveBuff();

        deadBodies--;

        AddBuff();

        return bodyController;
    }

    private void AddBuff()
    {
        gameSetup.HeadController.Head.healthBuff.AddBuff(1 + healthPercentagePerBody * deadBodies, true, null);
    }

    private void RemoveBuff()
    {
        gameSetup.HeadController.Head.healthBuff.AddBuff(1 / (1 + healthPercentagePerBody * deadBodies), true, null);
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(healthPercentagePerBody)))
        {
            RemoveBuff();

            healthPercentagePerBody = float.Parse(jsonVariables[nameof(healthPercentagePerBody)].ToString());

            AddBuff();
        }

        jsonVariables.Setup(ref bodiesDeadLevelUp, nameof(bodiesDeadLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            base.LevelUp();
        }

        bodiesDeadCount = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerBlood : Item
{
    private float healthThreshold;
    private float damageMultiplier;
    private float attackSpeedMultiplier;

    private List<BodyController> buffedBodies = new List<BodyController>();

    private int buffedBodiesKills = 0;
    private int buffedBodiesKillsLevelUp;

    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/BerserkerBlood.json";

        base.Setup(gameSetup);

        // intially buffs the bodies
        BuffAllBodies();

        // sets up all the triggers
        TriggerManager.BodyDeadTrigger.AddTrigger(BodyDead);
        TriggerManager.BodyRevivedTrigger.AddTrigger(BodyRevived);
        TriggerManager.BodyGainedHealthTrigger.AddTrigger(BodyGainedHealth);
        TriggerManager.BodyLostHealthTrigger.AddTrigger(BodyLostHealth);

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKill);
    }

    private bool CheckBodyCondition(BodyController bodyController)
    {
        // if the body is alive
        if (!bodyController.IsDead)
        {
            // checks if the body is a frontline
            if (bodyController.classNames.Contains(nameof(Frontline)))
            {
                // if the health is below the threshold
                if (bodyController.PercentageHealth <= healthThreshold)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private BodyController BodyDead(BodyController bodyController)
    {
        // if its been buffed
        if (buffedBodies.Contains(bodyController)) 
        {
            // unbuff it
            UnbuffBody(bodyController);
        }

        return bodyController;
    }

    private BodyController BodyRevived(BodyController bodyController)
    {
        // if it fits the conditions
        if (CheckBodyCondition(bodyController))
        {
            // buff it
            BuffBody(bodyController);
        }

        return bodyController;
    }

    private (BodyController, int) BodyGainedHealth((BodyController, int) info)
    {
        BodyController bodyController = info.Item1;

        // if the body is currently buffed
        if (buffedBodies.Contains(bodyController))
        {
            // if it no longer qualifies
            if (!CheckBodyCondition(bodyController))
            {
                // unbuff the body
                UnbuffBody(bodyController);
            }
        }

        return info;
    }

    private (BodyController, int) BodyLostHealth((BodyController, int) info)
    {
        BodyController bodyController = info.Item1;

        // if the body is not yet buffed
        if (!buffedBodies.Contains(bodyController))
        {
            // if it now qualifies for the conditions
            if (CheckBodyCondition(bodyController))
            {
                // buff the body
                BuffBody(bodyController);
            }
        }

        return info;
    }

    private void BuffBody(BodyController bodyController)
    {
        // buffs the body
        bodyController.attackSpeedBuff.AddBuff(attackSpeedMultiplier, true, null);
        bodyController.damageBuff.AddBuff(damageMultiplier, true, null);

        buffedBodies.Add(bodyController);
    }

    private void UnbuffBody(BodyController bodyController)
    {
        // buffs the body
        bodyController.attackSpeedBuff.AddBuff(1 / attackSpeedMultiplier, true, null);
        bodyController.damageBuff.AddBuff(1 / damageMultiplier, true, null);

        buffedBodies.Remove(bodyController);
    }

    private void BuffAllBodies()
    {
        // gets the head
        BodyController bodyController = gameSetup.HeadController.Head;

        // goes through each body in the snake
        while (bodyController is not null)
        {
            // if it fulfills the conditions
            if (CheckBodyCondition(bodyController))
            {
                // buff the body
                BuffBody(bodyController);
            }

            // get the next body
            bodyController = bodyController.next;
        }
    }

    private void UnBuffAllBodies()
    {
        // go through each body in the list
        foreach (BodyController bodyController in buffedBodies)
        {
            // unbuff the body
            UnbuffBody(bodyController);
        }
    }

    private GameObject OnBodyKill(GameObject body)
    {
        if (buffedBodies.Contains(body.GetComponent<BodyController>()))
        {
            buffedBodiesKills++;

            if (buffedBodiesKills >= buffedBodiesKillsLevelUp)
            {
                LevelUp();
            }
        }

        return body;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(healthThreshold)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            healthThreshold = float.Parse(jsonVariables[nameof(healthThreshold)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }

        if (jsonVariables.ContainsKey(nameof(damageMultiplier)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            damageMultiplier = float.Parse(jsonVariables[nameof(damageMultiplier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }

        if (jsonVariables.ContainsKey(nameof(attackSpeedMultiplier)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            attackSpeedMultiplier = float.Parse(jsonVariables[nameof(attackSpeedMultiplier)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }

        jsonVariables.Setup(ref buffedBodiesKillsLevelUp, nameof(buffedBodiesKillsLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            buffedBodiesKills -= buffedBodiesKillsLevelUp;
        }

        base.LevelUp();
    }
}

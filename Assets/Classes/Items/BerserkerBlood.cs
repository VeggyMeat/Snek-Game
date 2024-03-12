using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The berserker blood item
/// </summary>
internal class BerserkerBlood : Item
{
    /// <summary>
    /// The health threshold for the bodies to be buffed
    /// </summary>
    private float healthThreshold;

    /// <summary>
    /// The damage multiplier for the bodies that are being buffed
    /// </summary>
    private float damageMultiplier;
    
    /// <summary>
    /// The attack speed multiplier for the bodies that are being buffed
    /// </summary>
    private float attackSpeedMultiplier;

    /// <summary>
    /// The bodies that are currently buffed
    /// </summary>
    private List<BodyController> buffedBodies = new List<BodyController>();

    /// <summary>
    /// The number of kills that the buffed bodies have gotten
    /// </summary>
    private int buffedBodiesKills = 0;

    /// <summary>
    /// The number of buffed bodies kills needed to level up
    /// </summary>
    private int buffedBodiesKillsLevelUp;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
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

    /// <summary>
    /// Checks whether a particular body should be buffed or not
    /// </summary>
    /// <param name="bodyController">The body controller of the body to be checked</param>
    /// <returns>Whether to buff the body or not</returns>
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

    /// <summary>
    /// Called when a body dies and unbuffs it if it was buffed
    /// </summary>
    /// <param name="bodyController">The body that died</param>
    /// <returns>The body that died</returns>
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

    /// <summary>
    /// Called when a body is revived, and buffs it if it fits the conditions
    /// </summary>
    /// <param name="bodyController">The body that was revived</param>
    /// <returns></returns>
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

    /// <summary>
    /// Called on a body when it gained health, to check whether it should no longer be buffed
    /// </summary>
    /// <param name="info">The body controller, and the amount it was healed</param>
    /// <returns>The body controller, and the amount it was healed</returns>
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

    /// <summary>
    /// Called on a body when it lost health, to check whether it should be now buffed
    /// </summary>
    /// <param name="info">The body controller, and the amount it was healed</param>
    /// <returns>The body controller, and the amount it was healed</returns>
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

    /// <summary>
    /// Buffs a body
    /// </summary>
    /// <param name="bodyController">The body to be buffed</param>
    private void BuffBody(BodyController bodyController)
    {
        // buffs the body
        bodyController.attackSpeedBuff.AddBuff(attackSpeedMultiplier, true, null);
        bodyController.damageBuff.AddBuff(damageMultiplier, true, null);

        buffedBodies.Add(bodyController);
    }

    /// <summary>
    /// Removes the buff from a buffed body
    /// </summary>
    /// <param name="bodyController">The body to have the buff removed</param>
    private void UnbuffBody(BodyController bodyController)
    {
        // buffs the body
        bodyController.attackSpeedBuff.AddBuff(1 / attackSpeedMultiplier, true, null);
        bodyController.damageBuff.AddBuff(1 / damageMultiplier, true, null);

        buffedBodies.Remove(bodyController);
    }

    /// <summary>
    /// Buffs all the bodies in the snake that fit the conditions
    /// </summary>
    private void BuffAllBodies()
    {
        // gets the head
        BodyController bodyController = gameSetup.HeadController.Head;

        // goes through each body in the snake
        while (bodyController != null)
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

    /// <summary>
    /// Removes the buffs from all the bodies in the snake that are buffed
    /// </summary>
    private void UnBuffAllBodies()
    {
        // go through each body in the list
        foreach (BodyController bodyController in buffedBodies)
        {
            // unbuff the body
            UnbuffBody(bodyController);
        }
    }

    /// <summary>
    /// Called when a body kills another body, to increase the number of kills this item has, if that body was buffed
    /// </summary>
    /// <param name="body">The body that killed an enemy</param>
    /// <returns>The body that killed an enemy</returns>
    private GameObject OnBodyKill(GameObject body)
    {
        // if the body is buffed
        if (buffedBodies.Contains(body.GetComponent<BodyController>()))
        {
            // increase the number of kills
            buffedBodiesKills++;

            // if the number of kills is enough to level up
            if (buffedBodiesKills >= buffedBodiesKillsLevelUp)
            {
                LevelUp();
            }
        }

        return body;
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.SetupAction(ref healthThreshold, nameof(healthThreshold), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonVariables.SetupAction(ref damageMultiplier, nameof(damageMultiplier), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonVariables.SetupAction(ref attackSpeedMultiplier, nameof(attackSpeedMultiplier), UnBuffAllBodies, BuffAllBodies, jsonLoaded);

        jsonVariables.Setup(ref buffedBodiesKillsLevelUp, nameof(buffedBodiesKillsLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the old count of kills
        if (jsonLoaded)
        {
            buffedBodiesKills -= buffedBodiesKillsLevelUp;
        }

        base.LevelUp();
    }
}

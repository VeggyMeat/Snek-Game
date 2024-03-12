using UnityEngine;

// COMPLETE

/// <summary>
/// The magic cap item
/// </summary>
internal class MagicCap : Item
{
    /// <summary>
    /// The damage multiplier for the mage bodies
    /// </summary>
    private float damageMultiplier;

    /// <summary>
    /// The number of mage kills needed to level up
    /// </summary>
    private int mageKillsLevelUp;

    /// <summary>
    /// The current number of mage kills
    /// </summary>
    private int mageKills = 0;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/MagicCap.json";

        base.Setup(gameSetup);

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKilled);
        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);

        BuffAllBodies();
    }

    /// <summary>
    /// Buffs a particular body if it is a mage
    /// </summary>
    /// <param name="bodyController">The body to buff</param>
    /// <returns>The body to buff</returns>
    private BodyController BuffBody(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Mage)))
        {
            bodyController.damageBuff.AddBuff(damageMultiplier, true, null);
        }

        return bodyController;
    }

    /// <summary>
    /// Removes the buff from a body if it is a mage
    /// </summary>
    /// <param name="bodyController">The body to remove the buff from</param>
    /// <returns>The body to remove the buff from</returns>
    private BodyController RemoveBuff(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Mage)))
        {
            bodyController.damageBuff.AddBuff(1 / damageMultiplier, true, null);
        }

        return bodyController;
    }

    /// <summary>
    /// Buffs all bodies
    /// </summary>
    private void BuffAllBodies()
    {
        BodyController bodyController = gameSetup.HeadController.Head;

        while (bodyController != null)
        {
            BuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    /// <summary>
    /// Removes the buff from all bodies
    /// </summary>
    private void UnBuffAllBodies()
    {
        BodyController bodyController = gameSetup.HeadController.Head;

        while (bodyController != null)
        {
            RemoveBuff(bodyController);

            bodyController = bodyController.next;
        }
    }

    /// <summary>
    /// Called when an enemy is killed to increase the mage kills
    /// </summary>
    /// <param name="body">The body that killed an enemy</param>
    /// <returns>The body that killed an enemy</returns>
    private GameObject OnBodyKilled(GameObject body)
    {
        BodyController bodyClass = body.GetComponent<BodyController>();

        if (bodyClass.classNames.Contains(nameof(Mage)))
        {
            // its an mage, so add to the mage kills
            mageKills++;
        }

        // if the mage kills is greater than the level up amount
        if (mageKills >= mageKillsLevelUp && Levelable)
        {
            LevelUp();
        }

        return body;
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.SetupAction(ref damageMultiplier, nameof(damageMultiplier), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonVariables.Setup(ref mageKillsLevelUp, nameof(mageKillsLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            mageKills -= mageKillsLevelUp;
        }

        base.LevelUp();
    }
}

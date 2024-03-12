using UnityEngine;

// COMPLETE

/// <summary>
/// The enchanter's tome item
/// </summary>
internal class EnchantersTome : Item
{
    /// <summary>
    /// The HP multiplier for the snakes' bodies, per enchanter
    /// </summary>
    private float enchanterHPMultiplier;

    /// <summary>
    /// The damage multiplier for the snakes' bodies, per enchanter
    /// </summary>
    private float enchanterDamageMultiplier;

    /// <summary>
    /// The number of enemies killed
    /// </summary>
    private float enemiesKilled = 0;

    /// <summary>
    /// The number of enemies killed needed to level up
    /// </summary>
    private float enemiesKilledLevelUp;

    /// <summary>
    /// The multiplicative HP buff for the bodies
    /// </summary>
    private float HPBuff
    {
        get
        {
            return 1 + enchanterHPMultiplier * enchanters;
        }
    }

    /// <summary>
    /// The multiplicative damage buff for the bodies
    /// </summary>
    private float DamageBuff
    {
        get
        {
            return 1 + enchanterDamageMultiplier * enchanters;
        }
    }

    /// <summary>
    /// The number of enchanters in the body
    /// </summary>
    private int enchanters = 0;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/EnchantersTome.json";

        base.Setup(gameSetup);

        // counts the number of enchanters
        BodyController bodyController = gameSetup.HeadController.Head;
        while (bodyController != null)
        {
            IncreaseEnchanters(bodyController);

            bodyController = bodyController.next;
        }

        // buffs all the bodies
        BuffAllBodies();

        // sets up all the triggers
        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseEnchanters);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseEnchanters);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseEnchanters);
        TriggerManager.BodyKilledTrigger.AddTrigger(OnEnemyKilled);
    }

    /// <summary>
    /// Buffs a body
    /// </summary>
    /// <param name="bodyController">The body to be buffed</param>
    /// <returns>The body to be buffed</returns>
    private BodyController BuffBody(BodyController bodyController)
    {
        bodyController.healthBuff.AddBuff(HPBuff, true, null);
        bodyController.damageBuff.AddBuff(DamageBuff, true, null);

        return bodyController;
    }

    /// <summary>
    /// Removes the buff from a body
    /// </summary>
    /// <param name="bodyController">The body that the buff should be removed from</param>
    private void UnBuffBody(BodyController bodyController)
    {
        bodyController.healthBuff.AddBuff(1/HPBuff, true, null);
        bodyController.damageBuff.AddBuff(1/DamageBuff, true, null);
    }

    /// <summary>
    /// Checks if a body is an enchanter, and if so, increases the number of enchanters in the body
    /// </summary>
    /// <param name="bodyController">The body to check</param>
    /// <returns>The body to check</returns>
    private BodyController IncreaseEnchanters(BodyController bodyController)
    {
        // checks if the body is an enchanter
        if (bodyController.classNames.Contains(nameof(Enchanter)))
        {
            // if so remove all buffs, increase the number of enchanters, then buff everything
            UnBuffAllBodies();

            enchanters++;

            BuffAllBodies();
        }

        return bodyController;
    }

    /// <summary>
    /// Checks if a body is an enchanter, and if so, decreases the number of enchanters in the body
    /// </summary>
    /// <param name="bodyController">The body to check</param>
    /// <returns>The body to check</returns>
    private BodyController DecreaseEnchanters(BodyController bodyController)
    {
        // checks if the body is an enchanter
        if (bodyController.classNames.Contains(nameof(Enchanter)))
        {
            // if so remove all buffs, decrease the number of enchanters, then buff everything
            UnBuffAllBodies();

            enchanters--;

            BuffAllBodies();
        }

        return bodyController;
    }

    /// <summary>
    /// Buffs all the bodies in the snake
    /// </summary>
    private void BuffAllBodies()
    {
        // gets the head of the snake
        BodyController bodyController = gameSetup.HeadController.Head;

        // goes through each body in the snake buffing it
        while (bodyController != null)
        {
            BuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    /// <summary>
    /// Removes the buffs from all the bodies in the snake
    /// </summary>
    private void UnBuffAllBodies()
    {
        // gets the head of the snake
        BodyController bodyController = gameSetup.HeadController.Head;

        // goes through each body in the snake, removing the buff from it
        while (bodyController != null)
        {
            UnBuffBody(bodyController);

            bodyController = bodyController.next;
        }
    }

    /// <summary>
    /// Called when an enemy is killed, to count the number of enemies killed
    /// </summary>
    /// <param name="body">The body that killed an enemy</param>
    /// <returns>The body that killed an enemy</returns>
    private GameObject OnEnemyKilled(GameObject body)
    {
        // if the item is buffing
        if (enchanters > 0)
        {
            enemiesKilled++;
            
            // if the number of enemies killed is enough to level up
            if (enemiesKilled >= enemiesKilledLevelUp)
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

        jsonVariables.SetupAction(ref enchanterHPMultiplier, nameof(enchanterHPMultiplier), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonVariables.SetupAction(ref enchanterDamageMultiplier, nameof(enchanterDamageMultiplier), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonVariables.Setup(ref enemiesKilledLevelUp, nameof(enemiesKilledLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the old count of enemies killed
        if (jsonLoaded)
        {
            enemiesKilled -= enemiesKilledLevelUp;
        }

        base.LevelUp();
    }
}

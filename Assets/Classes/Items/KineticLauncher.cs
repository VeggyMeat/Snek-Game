using System;

// COMPLETE

/// <summary>
/// The kinetic launcher item
/// </summary>
internal class KineticLauncher : Item
{
    /// <summary>
    /// The damage modifier for bodies when the snake is turning
    /// </summary>
    private float damageModifier;

    /// <summary>
    /// Whether the snake is currently turning
    /// </summary>
    private bool buffing = false;

    /// <summary>
    /// The total time spent turning
    /// </summary>
    private float timeTurning = 0;

    /// <summary>
    /// The time turning needed to level up
    /// </summary>
    private float timeTurningLevelUp;

    /// <summary>
    /// The time the snake started turning
    /// </summary>
    private DateTime startedTurning = DateTime.Now;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/KineticLauncher.json";

        base.Setup(gameSetup);

        // if the snake is turning, start buffing
        if (gameSetup.HeadController.Turning)
        {
            StartBuffing();
        }

        TriggerManager.StopTurningTrigger.AddTrigger(StopBuffing);
        TriggerManager.StartTurningTrigger.AddTrigger(StartBuffing);
    }

    /// <summary>
    /// Called to start buffing the snake when it starts turning
    /// </summary>
    /// <param name="value">Redundant value</param>
    /// <returns>Redundant value</returns>
    private int StartBuffing(int value = 0)
    {
        // if it is already buffing, return
        if (buffing)
        {
            return value;
        }

        // record the time
        startedTurning = DateTime.Now;

        BuffAllBodies(false);
        buffing = true;

        return value;
    }

    /// <summary>
    /// Called to stop buffing the snake when it stops turning
    /// </summary>
    /// <param name="value">Redundant value</param>
    /// <returns>Redundant value</returns>
    private int StopBuffing(int value = 0)
    {
        // if it is not buffing, return
        if (!buffing)
        {
            return value;
        }

        // adds the unpaused time spent turning to the total time
        timeTurning += (float)TimeManager.GetElapsedTimeSince(startedTurning).TotalSeconds;

        // if the time turning is greater than the level up amount, level up
        if (timeTurning >= timeTurningLevelUp && Levelable)
        {
            LevelUp();
        }

        BuffAllBodies(true);
        buffing = false;

        return value;
    }

    /// <summary>
    /// Buffs or unbuffs all the body
    /// </summary>
    /// <param name="unbuff">Whether to buff (true) or remove the buffs (false)</param>
    private void BuffAllBodies(bool unbuff)
    {
        BodyController bodyController = gameSetup.HeadController.Head;
        while (bodyController != null)
        {
            if (unbuff)
            {
                UnBuffBody(bodyController);
            }
            else
            {
                BuffBody(bodyController);
            }

            bodyController = bodyController.next;
        }
    }

    /// <summary>
    /// Buffs a particular body
    /// </summary>
    /// <param name="bodyController">The body to buff</param>
    /// <returns>The body to buff</returns>
    private BodyController BuffBody(BodyController bodyController)
    {
        bodyController.damageBuff.AddBuff(damageModifier, true, null);

        return bodyController;
    }

    /// <summary>
    /// Removes the buff from a body
    /// </summary>
    /// <param name="bodyController">The body to remove the buff from</param>
    private void UnBuffBody(BodyController bodyController)
    {
        bodyController.damageBuff.AddBuff(1 / damageModifier, true, null);
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary> 
    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(damageModifier)))
        {
            if (jsonLoaded)
            {
                StopBuffing();
            }

            damageModifier = float.Parse(jsonVariables[nameof(damageModifier)].ToString());

            if (jsonLoaded)
            {
                if (gameSetup.HeadController.Turning)
                {
                    StartBuffing();
                }
            }
        }

        jsonVariables.Setup(ref timeTurningLevelUp, nameof(timeTurningLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            timeTurning -= timeTurningLevelUp;
        }

        base.LevelUp();
    }
}

using System;

// COMPLETE

/// <summary>
/// The steering wheel item
/// </summary>
internal class SteeringWheel : Item
{
    /// <summary>
    /// The turning rate multiplier
    /// </summary>
    private float turningRateMultiplier;

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
    private DateTime startedTurning;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/SteeringWheel.json";

        startedTurning = DateTime.Now;

        base.Setup(gameSetup);

        TriggerManager.StopTurningTrigger.AddTrigger(OnTurnStop);
        TriggerManager.StartTurningTrigger.AddTrigger(OnTurnStart);
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        if (jsonVariables.ContainsKey(nameof(turningRateMultiplier)))
        {
            if (jsonLoaded)
            {
                // remove the current multiplier from the snake
                gameSetup.HeadController.TurningRate *= 1 / turningRateMultiplier;
            }

            // load the new multiplier in
            jsonVariables.Setup(ref turningRateMultiplier, nameof(turningRateMultiplier));

            // add the new multiplier to the snake
            gameSetup.HeadController.TurningRate *= turningRateMultiplier;
        }

        jsonVariables.Setup(ref timeTurningLevelUp, nameof(timeTurningLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the time turning
        if (jsonLoaded)
        {
            timeTurning -= timeTurningLevelUp;
        }

        base.LevelUp();
    }

    /// <summary>
    /// Called when the snake starts turning
    /// </summary>
    /// <param name="value">Redundant value</param>
    /// <returns>Redundant value</returns>
    private int OnTurnStart(int value = 0)
    {
        // record the time the snake started turning
        startedTurning = DateTime.Now;

        return value;
    }

    /// <summary>
    /// Called when the snake stops turning
    /// </summary>
    /// <param name="value">Redundant value</param>
    /// <returns>Redundant value</returns>
    private int OnTurnStop(int value = 0)
    {
        // adds the unpaused time spent turning to the total time
        timeTurning += (float)TimeManager.GetElapsedTimeSince(startedTurning).TotalSeconds;

        // if the time turning is greater than the level up amount, level up
        if (timeTurning >= timeTurningLevelUp && Levelable)
        {
            LevelUp();
        }

        return value;
    }
}

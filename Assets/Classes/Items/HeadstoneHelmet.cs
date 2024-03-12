// COMPLETE

/// <summary>
/// The headstone helmet item
/// </summary>
internal class HeadstoneHelmet : Item
{
    /// <summary>
    /// The health buff to give the head, per dead body
    /// </summary>
    private float healthPercentagePerBody;

    /// <summary>
    /// The multiplicative health buff for the head
    /// </summary>
    private float HealthBuff
    {
        get
        {
            return 1 + healthPercentagePerBody * DeadBodies;
        }
    }

    /// <summary>
    /// The number of dead bodies
    /// </summary>
    private int DeadBodies
    {
        get
        {
            return gameSetup.HeadController.Bodies - gameSetup.HeadController.AliveBodies;
        }
    }

    /// <summary>
    /// The number of bodies that have died or been revived
    /// </summary>
    protected int bodiesDiedOrRevivedCount = 0;

    /// <summary>
    /// The number of bodies that have died or been revived needed to level up
    /// </summary>
    protected int bodiesDeadLevelUp;

    /// <summary>
    /// The previous buffed multiplier used
    /// </summary>
    private float previousBuff = 1;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/HeadstoneHelmet.json";

        base.Setup(gameSetup);

        AddBuff();

        TriggerManager.BodyDeadTrigger.AddTrigger(Reset);
        TriggerManager.BodyRevivedTrigger.AddTrigger(Reset);
    }

    /// <summary>
    /// Called when a body dies or revives, resets the buff with the new number of bodies
    /// </summary>
    /// <param name="body">The body that died or revived</param>
    /// <returns>The body that died or revived</returns>
    private BodyController Reset(BodyController body)
    {
        RemoveBuff();
        AddBuff();

        return body;
    }

    /// <summary>
    /// Buffs the head
    /// </summary>
    private void AddBuff()
    {
        gameSetup.HeadController.Head.healthBuff.AddBuff(HealthBuff, true, null);
        previousBuff = HealthBuff;

        bodiesDiedOrRevivedCount++;

        // levels up the item if necessary
        if (bodiesDiedOrRevivedCount >= bodiesDeadLevelUp && Levelable)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// Removes the buff from the head
    /// </summary>
    private void RemoveBuff()
    {
        gameSetup.HeadController.Head.healthBuff.AddBuff(1 / previousBuff, true, null);
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.SetupAction(ref healthPercentagePerBody, nameof(healthPercentagePerBody), RemoveBuff, AddBuff, true);
        jsonVariables.Setup(ref bodiesDeadLevelUp, nameof(bodiesDeadLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the old count of bodies died or revived
        bodiesDiedOrRevivedCount -= bodiesDeadLevelUp;

        if (jsonLoaded)
        {
            base.LevelUp();
        }
    }
}

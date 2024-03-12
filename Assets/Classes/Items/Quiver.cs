using UnityEngine;

/// <summary>
/// The quiver item
/// </summary>
internal class Quiver : Item
{
    /// <summary>
    /// The attack speed multiplier buff for the archers
    /// </summary>
    private float attackSpeedModifier;

    /// <summary>
    /// The number of archer kills needed to level up
    /// </summary>
    private int archerKillsLevelUp;

    /// <summary>
    /// The current number of archer kills
    /// </summary>
    private int archerKills = 0;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/Quiver.json";

        base.Setup(gameSetup);

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKilled);
        TriggerManager.BodySpawnTrigger.AddTrigger(AddArcherBuff);

        BuffArchers();
    }

    /// <summary>
    /// Adds the buff to a body controller if its an archer
    /// </summary>
    /// <param name="body">The body to buff</param>
    /// <returns>The body to buff</returns>
    private BodyController AddArcherBuff(BodyController body)
    {
        if (body.classNames.Contains(nameof(Archer)))
        {
            body.attackSpeedBuff.AddBuff(attackSpeedModifier, true, null);
        }

        return body;
    }

    /// <summary>
    /// Removes the buff from a body controller if its an archer
    /// </summary>
    /// <param name="body">The body to remove the buff from</param>
    /// <returns>The body to remove the buff from</returns>
    private BodyController RemoveArcherBuff(BodyController body)
    {
        if (body.classNames.Contains(nameof(Archer)))
        {
            body.attackSpeedBuff.AddBuff(1 / attackSpeedModifier, true, null);
        }

        return body;
    }

    /// <summary>
    /// Goes through the snake and buffs each archer
    /// </summary>
    private void BuffArchers()
    {
        BodyController body = gameSetup.HeadController.Head;

        while (body is not null)
        {
            AddArcherBuff(body);

            body = body.next;
        }
    }

    /// <summary>
    /// Goes through the snake and removes the buff from each archer
    /// </summary>
    private void UnBuffArchers()
    {
        BodyController body = gameSetup.HeadController.Head;

        while (body is not null)
        {
            RemoveArcherBuff(body);

            body = body.next;
        }
    }

    /// <summary>
    /// Increases the number of archer kills when a enemy is killed
    /// </summary>
    /// <param name="body">The body that killed the enemy</param>
    /// <returns>The body that killed the enemy</returns>
    private GameObject OnBodyKilled(GameObject body)
    {
        BodyController bodyClass = body.GetComponent<BodyController>();

        if (bodyClass.classNames.Contains(nameof(Archer)))
        {
            // its an archer, so add to the archer kills
            archerKills++;
        }

        // if the archer kills is greater than the level up amount
        if (archerKills >= archerKillsLevelUp && Levelable)
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

        jsonVariables.SetupAction(ref attackSpeedModifier, nameof(attackSpeedModifier), UnBuffArchers, BuffArchers, jsonLoaded);
        jsonVariables.Setup(ref archerKillsLevelUp, nameof(archerKillsLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            // resets the archer kills
            archerKills -= archerKillsLevelUp;
        }

        base.LevelUp();
    }
}

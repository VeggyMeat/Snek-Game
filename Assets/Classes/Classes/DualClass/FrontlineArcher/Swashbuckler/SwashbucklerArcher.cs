using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The swashbuckler archer class, a subclass of the archer class
/// </summary>
internal class SwashbucklerArcher : Archer
{
    /// <summary>
    /// The amount of damage increase per archer in the snake
    /// </summary>
    private int perArcherDamageIncrease;

    /// <summary>
    /// The amount of speed increase per archer in the snake
    /// </summary>
    private int perArcherSpeedIncrease;

    /// <summary>
    /// Holds the number of archers in the snake other than this one
    /// </summary>
    private int archerNumber = -1;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/Swashbuckler/SwashbucklerArcher.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // initial count for the number of archers
        // goes through each body and counts the number of archers
        BodyController bodyController = body.snake.Head;
        while (bodyController != null) 
        {
            if (bodyController.classNames.Contains(nameof(Archer)))
            {
                archerNumber++;
            }

            bodyController = bodyController.next;
        }

        // sets up the triggers
        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseArcher);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseArcher);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseArcher);
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    protected override void LaunchProjectile()
    {
        float archerDamageIncrease = 1 + archerNumber * perArcherDamageIncrease;
        float archerSpeedIncrease = 1 + archerNumber * perArcherSpeedIncrease;

        ProjectileController projectileShot = Projectile.Shoot(projectile, transform.position, Random.Range(0, Mathf.PI * 2), projectileVariables.Variables, this, body.DamageMultiplier * archerDamageIncrease);

        projectileShot.Velocity *= archerSpeedIncrease;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref perArcherDamageIncrease, nameof(perArcherDamageIncrease));
        jsonData.Setup(ref perArcherSpeedIncrease, nameof(perArcherSpeedIncrease));
    }

    /// <summary>
    /// Increases the number of archers when a body is spawned or revived (and is an archer)
    /// </summary>
    /// <param name="bodyController">The body spawned</param>
    /// <returns>The body spawned</returns>
    private BodyController IncreaseArcher(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Archer)))
        {
            archerNumber++;
        }

        return bodyController;
    }

    /// <summary>
    /// Decreases the number of archers when a body dies (and is an archer)
    /// </summary>
    /// <param name="bodyController">The body spawned</param>
    /// <returns>The body spawned</returns>
    private BodyController DecreaseArcher(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Archer)))
        {
            archerNumber--;
        }

        return bodyController;
    }
}

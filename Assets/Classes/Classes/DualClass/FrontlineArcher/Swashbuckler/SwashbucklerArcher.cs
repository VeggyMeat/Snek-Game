using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SwashbucklerArcher : Archer
{
    private int perArcherDamageIncrease;
    private int perArcherSpeedIncrease;

    /// <summary>
    /// Holds the number of archers in the snake other than this one
    /// </summary>
    private int archerNumber = -1;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineArcher/Swashbuckler/SwashbucklerArcher.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // initial count for the number of archers
        BodyController bodyController = body.snake.Head;
        while (bodyController is not null) 
        {
            if (bodyController.classNames.Contains(nameof(Archer)))
            {
                archerNumber++;
            }

            bodyController = bodyController.next;
        }

        TriggerManager.BodySpawnTrigger.AddTrigger(IncreaseArcher);
        TriggerManager.BodyDeadTrigger.AddTrigger(DecreaseArcher);
        TriggerManager.BodyRevivedTrigger.AddTrigger(IncreaseArcher);
    }

    internal override void LaunchProjectile()
    {
        float archerDamageIncrease = 1 + archerNumber * perArcherDamageIncrease;
        float archerSpeedIncrease = 1 + archerNumber * perArcherSpeedIncrease;

        ProjectileController projectileShot = Projectile.Shoot(projectile, transform.position, Random.Range(0, Mathf.PI * 2), projectileVariables.Variables, this, body.DamageMultiplier * archerDamageIncrease);

        projectileShot.Velocity *= archerSpeedIncrease;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref perArcherDamageIncrease, nameof(perArcherDamageIncrease));
        jsonData.Setup(ref perArcherSpeedIncrease, nameof(perArcherSpeedIncrease));
    }

    private BodyController IncreaseArcher(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Archer)))
        {
            archerNumber++;
        }

        return bodyController;
    }

    private BodyController DecreaseArcher(BodyController bodyController)
    {
        if (bodyController.classNames.Contains(nameof(Archer)))
        {
            archerNumber--;
        }

        return bodyController;
    }
}

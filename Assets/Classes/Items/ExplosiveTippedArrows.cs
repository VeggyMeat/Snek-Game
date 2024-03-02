using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExplosiveTippedArrows : Item
{
    private float radius;
    private int damage;

    private int archerKillsLevelUp;
    private int archerKills = 0;

    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/ExplosiveTippedArrows.json";

        base.Setup(gameSetup);

        TriggerManager.ProjectileHitTrigger.AddTrigger(OnHit);

        TriggerManager.BodyKilledTrigger.AddTrigger(OnBodyKilled);
    }

    private GameObject OnHit(GameObject projectileObject)
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(projectileObject.transform.position, radius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D collider in enemiesInCircle)
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();

            if (!enemyController.Dead)
            {
                if (!enemyController.ChangeHealth(-damage))
                {
                    gameSetup.HeadController.IncreaseXP(enemyController.XPDrop);
                }
            }
        }

        return projectileObject;
    }

    private GameObject OnBodyKilled(GameObject body)
    {
        // gets the bodyController from the body
        BodyController bodyClass = body.GetComponent<BodyController>();

        if (bodyClass.classNames.Contains(nameof(Archer)))
        {
            // its an archer, so add to the archer kills
            archerKills++;
        }

        // if the archer kills is greater than the level up amount
        if (archerKills >= archerKillsLevelUp && Levelable)
        {
            // level up
            LevelUp();
        }

        return body;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref radius, nameof(radius));
        jsonVariables.Setup(ref damage, nameof(damage));
        jsonVariables.Setup(ref archerKillsLevelUp, nameof(archerKillsLevelUp));
    }

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

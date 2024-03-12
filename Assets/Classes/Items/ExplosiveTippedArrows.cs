using UnityEngine;

// COMPLETE

/// <summary>
/// The explosive tipped arrows item
/// </summary>
internal class ExplosiveTippedArrows : Item
{
    /// <summary>
    /// The radius of the explosion
    /// </summary>
    private float radius;

    /// <summary>
    /// The damage of the explosion
    /// </summary>
    private int damage;

    /// <summary>
    /// The number of kills made by archers needed to level up
    /// </summary>
    private int archerKillsLevelUp;

    /// <summary>
    /// The current number of kills made by archers
    /// </summary>
    private int archerKills = 0;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/ExplosiveTippedArrows.json";

        base.Setup(gameSetup);

        TriggerManager.ProjectileHitTrigger.AddTrigger(OnHit);
        TriggerManager.BodyKilledTrigger.AddTrigger(EnemyKilled);
    }

    /// <summary>
    /// Called when a projectile hits an enemy
    /// Triggers an explosion
    /// </summary>
    /// <param name="projectileObject">The projectile that hit the enemy</param>
    /// <returns>The projectile that hit the enemy</returns>
    private GameObject OnHit(GameObject projectileObject)
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(projectileObject.transform.position, radius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        // goes through each enemy
        foreach (Collider2D collider in enemiesInCircle)
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            
            // if the enemy is not dead, damages it
            if (!enemyController.Dead)
            {
                if (!enemyController.ChangeHealth(-damage))
                {
                    // if the enemy died, increases the player's XP
                    gameSetup.HeadController.IncreaseXP(enemyController.XPDrop);
                }
            }
        }

        return projectileObject;
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="body">The enemy that was killed</param>
    /// <returns>The enemy that was killed</returns>
    private GameObject EnemyKilled(GameObject body)
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

        jsonVariables.Setup(ref radius, nameof(radius));
        jsonVariables.Setup(ref damage, nameof(damage));
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

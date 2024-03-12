using UnityEngine;

// COMPLETE

/// <summary>
/// The big bullet item
/// </summary>
internal class BigBullet : Item
{
    /// <summary>
    /// The modifier for the size of the projectile
    /// </summary>
    private float projectileSizeModifier;

    /// <summary>
    /// The number of projectiles that have hit enemies
    /// </summary>
    private float projectilesHitCount = 0;

    /// <summary>
    /// The number of projectiles that need to hit enemies to level up
    /// </summary>
    private float projectilesHitLevelUp;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/BigBullet.json";

        base.Setup(gameSetup);

        TriggerManager.ProjectileShotTrigger.AddTrigger(IncreaseProjectileSize);
        TriggerManager.ProjectileHitTrigger.AddTrigger(ProjectileHit);
    }

    /// <summary>
    /// Changes the size of the projectile,
    /// Caled when a projectile is shot
    /// </summary>
    /// <param name="projectile">The projectile that was shot, to be buffed</param>
    /// <returns>The projectile that was shot, to be buffed</returns>
    private GameObject IncreaseProjectileSize(GameObject projectile)
    {
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        projectileController.Scale(projectileSizeModifier);

        return projectile;
    }
    
    /// <summary>
    /// Called when a projectile hits an enemy
    /// </summary>
    /// <param name="projectile">The projectile that hit an enemy</param>
    /// <returns>The projectile that hit an enemy</returns>
    private GameObject ProjectileHit(GameObject projectile)
    {
        projectilesHitCount++;

        // levels up the item if it can be leveled up and the number of projectiles hit is enough
        if (projectilesHitCount >= projectilesHitLevelUp && Levelable)
        {
            LevelUp();
        }

        return projectile;
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref projectileSizeModifier, nameof(projectileSizeModifier));
        jsonVariables.Setup(ref projectilesHitLevelUp, nameof(projectilesHitLevelUp));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the old count of projectiles hit
        if (jsonLoaded)
        {
            projectilesHitCount -= projectilesHitLevelUp;
        }

        base.LevelUp();
    }
}

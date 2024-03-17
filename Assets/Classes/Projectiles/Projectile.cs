using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// Static class that controls the creation of projectiles
/// </summary>
public static class Projectile 
{
    /// <summary>
    /// The z value of the projectile
    /// </summary>
    private const int zValue = 4;

    /// <summary>
    /// Creates a new projectile controller and sets it up
    /// </summary>
    /// <param name="projectile">The prefab for the projectile to be created</param>
    /// <param name="position">Where to create the projectile</param>
    /// <param name="angle">The angle from the position that the projectile is moving in</param>
    /// <param name="jsonVariables">The projectile's variables</param>
    /// <param name="controller">The class that created the projectile, or that controls it</param>
    /// <param name="damageMultiplier">The damage multiplier for the projectile over its normal damage value</param>
    /// <param name="addOwnerVelocity">Whether to add the velocity of the owner or not</param>
    /// <param name="callTrigger">Whether to call TriggerManager's ProjectileShotTrigger that a projectile has been shot or not</param>
    /// <returns>The ProjectileController of the created projectile</returns>
    internal static ProjectileController Shoot(GameObject projectile, Vector3 position, float angle, Dictionary<string, object> jsonVariables, Class controller, float damageMultiplier, bool addOwnerVelocity = true)
    {
        // create the projectile
        GameObject proj = Object.Instantiate(projectile, new Vector3(position.x, position.y, zValue), Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));

        ProjectileController projController = proj.GetComponent<ProjectileController>();

        projController.Setup(jsonVariables, controller, damageMultiplier, addOwnerVelocity);

        // tells the Trigger Manager a projectile has been shot
        TriggerManager.ProjectileShotTrigger.CallTrigger(proj);

        return projController;
    }
}

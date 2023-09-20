using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class Projectile 
{
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
    /// <returns>The ProjectileController of the created projectile</returns>
    public static ProjectileController Shoot(GameObject projectile, Vector3 position, float angle, Dictionary<string, object> jsonVariables, Class controller, float damageMultiplier, bool addOwnerVelocity = true)
    {
        // create the projectile
        GameObject proj = Object.Instantiate(projectile, position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));

        // grabs the projectile controller
        ProjectileController projController = proj.GetComponent<ProjectileController>();

        // sets up the projectile controller
        projController.Setup(jsonVariables, controller, damageMultiplier, addOwnerVelocity);

        return projController;
    }
}

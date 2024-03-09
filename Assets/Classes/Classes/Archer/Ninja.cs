using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The ninja class, a subclass of the archer class
/// </summary>
internal class Ninja : Archer
{
    /// <summary>
    /// The number of projectiles to shoot per attack
    /// </summary>
    private int projectiles;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Ninja.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    protected override void LaunchProjectile()
    {
        // creates and sets up a number of projectiles equal to 'projectiles'
        for (int i = 0; i < projectiles; i++)
        {
            Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref projectiles, nameof(projectiles));
    }
}

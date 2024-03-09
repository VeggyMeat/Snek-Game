using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The bowman class, a subclass of the archer class
/// </summary>
internal class BowMan : Archer
{
    /// <summary>
    /// The number of projectiles launched in each attack
    /// </summary>
    private int projectileCount;

    /// <summary>
    /// The number of attacks launched when an enemy is killed by this class
    /// </summary>
    private int enemyDeathVolleyCount;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    protected override void LaunchProjectile()
    {
        // if its dead, then don't launch a projectile
        if (body.IsDead)
        {
            return;
        }

        // launches a volley of projectiles
        for (int i = 0; i < projectileCount; i++)
        {
            // creates and sets up a new projectile
            Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy">The enemy's gameObject</param>
    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        // launches a volley of projectiles
        for (int i = 0; i < enemyDeathVolleyCount; i++) 
        {
            LaunchProjectile();
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref projectileCount, nameof(projectileCount));
        jsonData.Setup(ref enemyDeathVolleyCount, nameof(enemyDeathVolleyCount));
    }
}

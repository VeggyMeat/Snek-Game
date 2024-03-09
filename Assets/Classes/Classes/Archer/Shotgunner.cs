using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The shotgunner class, a subclass of the archer class
/// </summary>
internal class Shotgunner : Archer
{
    /// <summary>
    /// The number of projectiles in each shot
    /// </summary>
    private int projectileCount;

    /// <summary>
    /// The angle of spread for the projectiles when shot
    /// </summary>
    private float spreadAngle;

    /// <summary>
    /// The range in which the shotgunner can shoot at enemies
    /// </summary>
    private float range;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Shotgunner.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    protected override void LaunchProjectile()
    {
        // gets all the game objects in the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, range);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        // returns if there are no enemies in the range
        if (enemiesInCircle.Length == 0)
        {
            return;
        }

        // get a position of a random enemy
        Vector3 enemyPos = enemiesInCircle[Random.Range(0, enemiesInCircle.Length)].transform.position;

        // gets the angle to the enemy
        float angle = ((Vector2)transform.position).AngleTo((Vector2)enemyPos);

        // repeats this for every projectile to be shot
        for (int i = 0; i < projectileCount; i++)
        {
            // the spread angle that gets added
            float addedAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);

            // shoots the projectile
            Projectile.Shoot(projectile, transform.position, angle + addedAngle, projectileVariables.Variables, this, body.DamageMultiplier);
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
        jsonData.Setup(ref spreadAngle, nameof(spreadAngle));
        jsonData.Setup(ref range, nameof(range));
    }
}

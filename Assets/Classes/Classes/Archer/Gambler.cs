using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// COMPLETE

/// <summary>
/// The gambler class, a subclass of the archer class
/// </summary>
internal class Gambler : Archer
{
    /// <summary>
    /// The minimum radius of the projectile shot
    /// </summary>
    private float minRadius;

    /// <summary>
    /// The maximum radius of the projectile shot
    /// </summary>
    private float maxRadius;

    /// <summary>
    /// The maximum damage multiplier of the projectile shot
    /// </summary>
    private float maxDamageMultiplier;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    internal override void LaunchProjectile()
    {
        // picks a random size
        float radius = Random.Range(minRadius, maxRadius);

        // gets the damage based on the size
        float damageMultiplier = Map(minRadius, maxRadius, 1, maxDamageMultiplier, radius);

        // creates and sets up a new projectile
        ProjectileController controller = Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, damageMultiplier);

        // sets the size of the projectile
        controller.transform.localScale = new Vector3(radius, radius, 1);
    }

    /// <summary>
    /// Maps one set of values onto another set of values
    /// </summary>
    /// <param name="OldMin">The lowest value in the old range</param>
    /// <param name="OldMax">The maximum value in the old range</param>
    /// <param name="NewMin">The lowest value in the new range</param>
    /// <param name="NewMax">The maximum value in the new range</param>
    /// <param name="OldValue">The old value to be mapped to the new value</param>
    /// <returns>The new value that is returned from the old value being mapped</returns>
    private float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        // gets the old range of values
        float OldRange = (OldMax - OldMin);

        // gets the new range of values
        float NewRange = (NewMax - NewMin);

        // maps the old value to the new value
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return NewValue;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref minRadius, nameof(minRadius));
        jsonData.Setup(ref maxRadius, nameof(maxRadius));
        jsonData.Setup(ref maxDamageMultiplier, nameof(maxDamageMultiplier));
    }
}

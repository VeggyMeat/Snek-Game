using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The fire mage class, a subclass of the mage class
/// </summary>
internal class FireMage : Mage
{
    /// <summary>
    /// The number of orbs to fire each attack
    /// </summary>
    private int orbNumber;

    /// <summary>
    /// How far to rotate the mage each attack
    /// </summary>
    private float rotation;

    /// <summary>
    /// The random variation in the fired shots as an angle
    /// </summary>
    private float orbVariation;

    /// <summary>
    /// The current angle the mage is facing
    /// </summary>
    private float angleFacing;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/FireMage.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    protected override void Attack()
    {
        // rotates the mage slightly
        angleFacing += rotation;
        angleFacing %= Mathf.PI * 2;

        for (int i = 0; i < orbNumber; i++) 
        {
            // pick a random angle variation
            angleFacing += Random.Range(-orbVariation, orbVariation);

            // creates and sets up a new projectile
            Projectile.Shoot(orbTemplate, transform.position, angleFacing, orbVariables.Variables, this, body.DamageMultiplier);
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref rotation, nameof(rotation));
        jsonData.Setup(ref orbVariation, nameof(orbVariation));
    }
}

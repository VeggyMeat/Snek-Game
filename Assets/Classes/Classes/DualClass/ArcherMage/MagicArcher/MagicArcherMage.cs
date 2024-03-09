using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The magic archer mage class, a subclass of the mage class
/// </summary>
internal class MagicArcherMage : Mage
{
    /// <summary>
    /// The number of orbs to shoot each attack
    /// </summary>
    private float orbs;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MagicArcher/MagicArcherMage.json";

        // indicates its not the primary class of the body
        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbs, "orbs");
    }
    
    /// <summary>
    /// Called by the magic archer archer to launch the orbs
    /// </summary>
    /// <param name="enemy">The enemy that it should launch the orbs from</param>
    internal void LaunchOrbs(GameObject enemy)
    {
        // picks a random angle between 0 and pi / 2
        float angle = Random.Range(0, Mathf.PI / 2);

        // launches 'orbs' orbs evenly distributed in a circle
        for (double i = 0; i < Mathf.PI * 2; i += Mathf.PI * 2 / orbs)
        {
            // launches a projectile
            Projectile.Shoot(orbTemplate, enemy.transform.position, angle + (float)i, orbVariables.Variables, this, body.DamageMultiplier, addOwnerVelocity: false);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The spectre class, a subclass of the mage class
/// </summary>
internal class Spectre : Mage
{
    /// <summary>
    /// The number of orbs to fire each attack
    /// </summary>
    private int orbNumber;

    /// <summary>
    /// The multiplier to increase the damage by when dead
    /// </summary>
    private float damageMult;

    /// <summary>
    /// The multiplier to increase the attack speed by when dead
    /// </summary>
    private float speedMult;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/Spectre.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    protected override void Attack()
    {
        for (int i = 0; i < orbNumber; i++)
        {
            // if dead, increases the damage by the miltiplier
            if (body.IsDead)
            {
                Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbVariables.Variables, this, body.DamageMultiplier * damageMult);
            }
            else
            {
                Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbVariables.Variables, this, body.DamageMultiplier);
            }
        }
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // reduces the delay of attacking
        timeDelay /= speedMult;

        // continues attacking after death
        StartRepeatingAttack();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        // stops the increased attack speed attack
        StopRepeatingAttack();

        base.Revived();

        // sets the delay back to the original amount
        timeDelay *= speedMult;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref damageMult, nameof(damageMult));
        jsonData.Setup(ref speedMult, nameof(speedMult));
    }
}

using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The ceramic automaton class, a subclass of the mage class
/// </summary>
internal class CeramicAutomaton : Mage
{
    /// <summary>
    /// The maximum number of shield layers it can have
    /// </summary>
    private int maxShield;

    /// <summary>
    /// The time delay between shields regenerating
    /// </summary>
    private float shieldRegenDelay;

    /// <summary>
    /// How many shield layers it currently has
    /// </summary>
    private int shield;

    /// <summary>
    /// Whether the shield is currently regenerating
    /// </summary>
    private bool shieldRegenActive = false;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/CeramicAutomaton.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // sets the shield number to the maxShield number
        shield = maxShield;

        base.Setup();
    }

    /// <summary>
    /// Called when the body takes damage, before the damage is applied
    /// </summary>
    /// <param name="amount">The damage taken</param>
    /// <returns>Returns the new damage value</returns>
    internal override int OnDamageTaken(int amount)
    {
        // if there is a shield left, it ignores damage
        if (shield > 0)
        {
            // removes a shield
            shield--;

            // if currently not regenerating a shield, start regenerating
            if (!shieldRegenActive)
            {
                // start regenerating
                Invoke(nameof(RegenShield), shieldRegenDelay);

                // show that its regenerating
                shieldRegenActive = true;
            }

            // return the base effect on 0
            return base.OnDamageTaken(0);
        }

        // return the base effect on the orginal damage amount
        return base.OnDamageTaken(amount);
    }

    /// <summary>
    /// Regenerates one layer of shield
    /// </summary>
    internal void RegenShield()
    {
        // if the shield is not full
        if (shield < maxShield)
        {
            // regen one layer
            shield++;

            // if its still not full, regen more shield later
            if (shield < maxShield)
            {
                Invoke(nameof(RegenShield), shieldRegenDelay);
            }
            // otherwise stop regenerating shield, and note that
            else
            {
                shieldRegenActive = false;
            }
        }
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    protected override void Attack()
    {
        // gets a random angle
        float angle = Random.Range(0, Mathf.PI * 2);

        // creates and sets up a new projectile
        Projectile.Shoot(orbTemplate, transform.position, angle, orbVariables.Variables, this, body.DamageMultiplier);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        CancelInvoke(nameof(RegenShield));
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // sets the shield number to the maxShield number
        shield = maxShield;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxShield, nameof(maxShield));
        jsonData.Setup(ref shieldRegenDelay, nameof(shieldRegenDelay));
    }
}

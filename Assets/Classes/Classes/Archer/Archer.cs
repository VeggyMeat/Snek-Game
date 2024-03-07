using System;
using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The base class for all archer classes
/// </summary>
internal abstract class Archer : Class
{
    /// <summary>
    /// The delay in time between firing each projectile
    /// </summary>
    protected float timeDelay;

    /// <summary>
    /// The prefab for the projectile object
    /// </summary>
    internal GameObject projectile;

    /// <summary>
    /// Whether the archer should automatically fire projectiles or not
    /// </summary>
    protected bool autoFire;

    /// <summary>
    /// The file path to the projectile prefab
    /// </summary>
    protected string projectilePath = null;

    /// <summary>
    /// The file path to the projectile's json file
    /// </summary>
    protected string projectileJson;

    /// <summary>
    /// The contained variables (data) for the projectiles shot
    /// </summary>
    protected JsonVariable projectileVariables;

    /// <summary>
    /// Whether the archer is currently firing projectiles or not
    /// </summary>
    private bool firingProjectiles = false;

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        // adds the archer class name to the body's classes for identification
        body.classNames.Add(nameof(Archer));

        base.Setup();

        // If the archer has a projectile
        if (projectilePath is not null)
        {
            // grabs the projectile from resources
            projectile = Resources.Load<GameObject>(projectilePath);

            // sets up the variables for the projectiles
            projectileVariables = new JsonVariable(projectileJson);
        }

        if (autoFire)
        {
            StartRepeatingProjectile();
        }
    }

    /// <summary>
    /// Runs the LaunchProjectile function every timeDelay seconds
    /// </summary>
    internal void StartRepeatingProjectile()
    {
        // if firing projectiles, ignore, as it is already firing
        if (firingProjectiles)
        {
            return;
        }

        // start firing projectiles and note that they are firing
        InvokeRepeating(nameof(LaunchProjectile), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        firingProjectiles = true;
    }

    /// <summary>
    /// Stops the repeating projectile from firing
    /// </summary>
    internal void StopRepeatingProjectile()
    {
        // if its already not firing projectiles, ignore
        if (!firingProjectiles)
        {
            return;
        }

        // stop firing projectiles and not they are not firing
        CancelInvoke(nameof(LaunchProjectile));

        firingProjectiles = false;
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    /// <exception cref="Exception">Called when the child class does not override</exception>
    internal virtual void LaunchProjectile()
    {
        throw new Exception("LaunchProjectile not overridden by child class, yet still called");
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // starts projectiles again
        if (autoFire)
        {
            StartRepeatingProjectile();
        }
    }

    /// <summary>
    /// Called when the body is killed
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // stops projectiles from being fired
        if (autoFire)
        {
            StopRepeatingProjectile();
        }
    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // resets the repeating projectile
        if (autoFire)
        {
            StopRepeatingProjectile();
            StartRepeatingProjectile();
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // not allowed to change after initialisation
        jsonData.Setup(ref projectilePath, nameof(projectilePath));
        jsonData.Setup(ref projectileJson, nameof(projectileJson));
        jsonData.Setup(ref autoFire, nameof(autoFire));

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), StopRepeatingProjectile, StartRepeatingProjectile, jsonLoaded);
    }

    /// <summary>
    /// Called when the body levels up
    /// </summary>
    internal override void LevelUp()
    {
        base.LevelUp();

        // updates the arrow variables for the new level if they exist
        if (body.Level != 1)
        {
            if (projectileVariables is not null)
            {
                projectileVariables.IncreaseIndex();
            }
        }
    }
}

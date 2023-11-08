using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Archer : Class
{
    protected float timeDelay;
    internal GameObject projectile;
    protected bool autoFire;

    protected string projectilePath = null;
    protected string projectileJson;

    protected JsonVariable projectileVariables;

    private bool firingProjectiles = false;

    internal override void Setup()
    {
        // adds the archer class name to the body's classes for identification
        body.classNames.Add(nameof(Archer));

        base.Setup();

        if (projectilePath is not null)
        {
            // grabs the projectile from resources
            projectile = Resources.Load<GameObject>(projectilePath);

            // sets up the variables for the projectiles
            projectileVariables = new JsonVariable(projectileJson);
        }

        // starts firing projectiles if autoFire is true
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
        // if firing projectiles, ignore
        if (firingProjectiles)
        {
            return;
        }

        // start firing projectiles and note that they are firing
        InvokeRepeating(nameof(LaunchProjectile), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        firingProjectiles = true;
    }

    /// <summary>
    /// Stops the repeating projectile from happening
    /// </summary>
    internal void StopRepeatingProjectile()
    {
        // if not firing projectiles, ignore
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
    /// <exception cref="System.NotImplementedException"></exception>
    internal virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }

    internal override void Revived()
    {
        base.Revived();

        // starts projectiles again
        if (autoFire)
        {
            StartRepeatingProjectile();
        }
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // stops projectiles from being fired
        if (autoFire)
        {
            StopRepeatingProjectile();
        }
    }

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

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // not allowed to change after initialisation
        jsonData.Setup(ref projectilePath, nameof(projectilePath));
        jsonData.Setup(ref projectileJson, nameof(projectileJson));
        jsonData.Setup(ref autoFire, nameof(autoFire));

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), StopRepeatingProjectile, StartRepeatingProjectile, jsonLoaded);
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        // updates the arrow variables for the new level
        if (body.Level != 1)
        {
            if (projectileVariables is not null)
            {
                projectileVariables.IncreaseIndex();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : Class
{
    protected float timeDelay;
    internal GameObject projectile;
    protected bool autoFire;

    internal override void Setup()
    {
        body.classNames.Add("Archer");

        base.Setup();

        if (autoFire)
        {
            // starts firing the projectiles
            StartRepeatingProjectile();
        }
    }

    /// <summary>
    /// Runs the LaunchProjectile function every timeDelay seconds
    /// </summary>
    internal void StartRepeatingProjectile()
    {
        InvokeRepeating(nameof(LaunchProjectile), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);
    }

    /// <summary>
    /// Stops the repeating projectile from happening
    /// </summary>
    internal void StopRepeatingProjectile()
    {
        CancelInvoke(nameof(LaunchProjectile));
    }

    // creates a base case incase not implemented
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

        jsonData.Setup(ref timeDelay, "timeDelay");
        jsonData.Setup(ref autoFire, "autoFire");
    }
}

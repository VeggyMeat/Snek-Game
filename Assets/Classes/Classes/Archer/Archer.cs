using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : Class
{
    internal float timeDelay;
    internal float velocity;
    internal GameObject projectile;
    internal float lifeSpan;
    internal int projectileDamage;

    internal override void Setup()
    {
        base.Setup();

        // starts firing the projectiles
        StartRepeatingProjectile();
    }

    internal void ResetRepeatingProjectile()
    {
        // stops the current repeat
        CancelInvoke();

        // starts a new one
        StartRepeatingProjectile();
    }

    // runs the LaunchProjectile function every timeDelay seconds
    internal void StartRepeatingProjectile()
    {
        InvokeRepeating(nameof(LaunchProjectile), timeDelay, timeDelay);
    }

    // creates a base case incase not implemented
    internal virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }
}

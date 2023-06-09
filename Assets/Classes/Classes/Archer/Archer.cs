using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : Class
{
    public float timeDelay;
    public GameObject projectile;

    internal override void Setup()
    {
        className = "Archer";

        base.Setup();

        // starts firing the projectiles
        StartRepeatingProjectile();
    }

    // runs the LaunchProjectile function every timeDelay seconds
    internal void StartRepeatingProjectile()
    {
        InvokeRepeating(nameof(LaunchProjectile), timeDelay / attackSpeedBuff.Value, timeDelay / attackSpeedBuff.Value);
    }

    // stops the repeating projectile from happening
    internal void StopRepeatingProjectile()
    {
        CancelInvoke(nameof(LaunchProjectile));
    }

    // stops the projectile from firing, then starts immediatly after
    internal void ResetRepeatingProjectile()
    {
        StopRepeatingProjectile();

        StartRepeatingProjectile();
    }

    // creates a base case incase not implemented
    internal virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }

    // called when the body is revived from the dead
    internal override void Revived()
    {
        base.Revived();

        // starts projectiles again
        StartRepeatingProjectile();
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        // stops projectiles from being fired
        StopRepeatingProjectile();
    }

    // called when the attack speed buff changes
    internal override void AttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.AttackSpeedBuffUpdate(amount, multiplicative);

        // resets the repeating projectile
        ResetRepeatingProjectile();
    }
}

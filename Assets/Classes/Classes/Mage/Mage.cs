using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Class
{
    internal float timeDelay;
    internal bool regularAttack;

    internal override void Setup()
    {
        base.Setup();

        if (regularAttack)
        {
            // starts attacking regularly
            StartRepeatingAttack();
        }
    }

    // stops then starts again the repeating attack
    internal void ResetRepeatingAttack()
    {
        StopRepeatingAttack();

        StartRepeatingAttack();
    }

    // runs the Attack function every timeDelay seconds
    internal void StartRepeatingAttack()
    {
        InvokeRepeating(nameof(Attack), timeDelay, timeDelay);
    }

    // stops the repeating attack from happening
    internal void StopRepeatingAttack()
    {
        CancelInvoke(nameof(Attack));
    }

    // creates a base case incase not implemented
    internal virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    // called when the body is revived from the dead
    internal override void Revived()
    {
        base.Revived();

        if (regularAttack)
        {
            // starts attacking again
            StartRepeatingAttack();
        }
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        if (regularAttack)
        {
            // stops attacking
            StopRepeatingAttack();
        }
    }
}

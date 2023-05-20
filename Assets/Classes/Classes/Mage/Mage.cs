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

    internal void ResetRepeatingAttack()
    {
        // stops the current repeat
        CancelInvoke();

        // starts a new one
        StartRepeatingAttack();
    }

    // runs the Attack function every timeDelay seconds
    internal void StartRepeatingAttack()
    {
        InvokeRepeating(nameof(Attack), timeDelay, timeDelay);
    }

    // creates a base case incase not implemented
    internal virtual void Attack()
    {
        throw new System.NotImplementedException();
    }
}

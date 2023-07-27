using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Class
{
    protected float timeDelay;
    protected bool regularAttack;

    internal override void Setup()
    {
        body.classNames.Add("Mage");

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
        InvokeRepeating(nameof(Attack), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);
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

    // called when the attack speed buff changes
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // resets the repeating attack
        ResetRepeatingAttack();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("timeDelay"))
        {
            timeDelay = float.Parse(jsonData["timeDelay"].ToString());

            if (jsonLoaded)
            {
                ResetRepeatingAttack();
            }
        }
        if (jsonData.ContainsKey("regularAttack"))
        {
            regularAttack = bool.Parse(jsonData["regularAttack"].ToString());

            if (jsonLoaded)
            {
                if (regularAttack)
                {
                    StartRepeatingAttack();
                }
                else
                {
                    StopRepeatingAttack();
                }
            }
        }
    }
}

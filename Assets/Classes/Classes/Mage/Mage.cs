using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mage : Class
{
    protected float timeDelay;
    protected bool regularAttack;

    protected string orbPath;
    protected string orbJson;

    internal GameObject orbTemplate;

    protected JsonVariable orbVariables;

    protected bool attacking = false;

    internal override void Setup()
    {
        body.classNames.Add("Mage");

        base.Setup();

        if (orbPath is not null)
        {
            orbVariables = new JsonVariable(orbJson);
        }

        if (regularAttack)
        {
            // starts attacking regularly
            StartRepeatingAttack();
        }
    }

    /// <summary>
    /// Runs the Attack function every timeDelay seconds
    /// </summary>
    protected void StartRepeatingAttack()
    {
        if (attacking)
        {
            return;
        }

        InvokeRepeating(nameof(Attack), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        attacking = true;
    }

    /// <summary>
    /// Stops the repeating attack from happening
    /// </summary>
    protected void StopRepeatingAttack()
    {
        if (!attacking)
        {

        }

        CancelInvoke(nameof(Attack));

        attacking = false;
    }

    // creates a base case incase not implemented
    internal virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    internal override void Revived()
    {
        base.Revived();

        if (regularAttack)
        {
            // starts attacking again
            StartRepeatingAttack();
        }
    }

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

        if (regularAttack)
        {
            // resets the repeating attack
            StopRepeatingAttack();
            StartRepeatingAttack();
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbJson, nameof(orbJson));

        if (jsonData.ContainsKey(nameof(orbPath)))
        {
            orbPath = jsonData[nameof(orbPath)].ToString();

            // grabs the orb thats shot
            orbTemplate = Resources.Load<GameObject>(orbPath);
        }

        if (jsonData.ContainsKey(nameof(timeDelay)))
        {
            timeDelay = float.Parse(jsonData[nameof(timeDelay)].ToString());

            if (jsonLoaded)
            {
                if (regularAttack)
                {
                    StopRepeatingAttack();
                    StartRepeatingAttack();
                }
            }
        }
        if (jsonData.ContainsKey(nameof(regularAttack)))
        {
            regularAttack = bool.Parse(jsonData[nameof(regularAttack)].ToString());

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

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            orbVariables.IncreaseIndex();
        }
    }
}

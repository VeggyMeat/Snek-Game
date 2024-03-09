using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The base class for all mage classes
/// </summary>
internal abstract class Mage : Class
{
    /// <summary>
    /// The time delay between attacks
    /// </summary>
    protected float timeDelay;

    /// <summary>
    /// Whether the class should automatically attack regularly
    /// </summary>
    protected bool regularAttack = false;

    /// <summary>
    /// The path to the orb prefab
    /// </summary>
    protected string orbPath;

    /// <summary>
    /// The path to the json for the orb
    /// </summary>
    protected string orbJson;

    /// <summary>
    /// The prefab for the orb game object
    /// </summary>
    internal GameObject orbTemplate;

    /// <summary>
    /// The variables for the orb
    /// </summary>
    protected JsonVariable orbVariables;

    /// <summary>
    /// Whether the mage is currently attacking regularly
    /// </summary>
    protected bool attacking = false;

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // adds the mage class name to the body's classes for identification
        body.classNames.Add(nameof(Mage));

        base.Setup();

        // if the orb path is not null, then it loads the orb variables
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
        // if its already attacking, then return
        if (attacking)
        {
            return;
        }

        // calls the attack function every timeDelay seconds
        InvokeRepeating(nameof(Attack), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        attacking = true;
    }

    /// <summary>
    /// Stops the repeating attack from happening
    /// </summary>
    protected void StopRepeatingAttack()
    {
        // if its not attacking, then return
        if (!attacking)
        {

        }

        // cancels the repeating attack
        CancelInvoke(nameof(Attack));

        attacking = false;
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    /// <exception cref="System.NotImplementedException">Throws an error if not overidden by child class</exception>
    protected virtual void Attack()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        if (regularAttack)
        {
            StartRepeatingAttack();
        }
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        if (regularAttack)
        {
            StopRepeatingAttack();
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

        if (regularAttack)
        {
            // resets the repeating attack
            StopRepeatingAttack();
            StartRepeatingAttack();
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbJson, nameof(orbJson));

        if (jsonData.ContainsKey(nameof(orbPath)))
        {
            orbPath = jsonData[nameof(orbPath)].ToString();

            orbTemplate = Resources.Load<GameObject>(orbPath);
        }

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), StopRepeatingAttack, StartRepeatingAttack, jsonLoaded);
        jsonData.Setup(ref regularAttack, nameof(regularAttack));
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal override void LevelUp()
    {
        base.LevelUp();

        if (orbPath is not null)
        {
            if (body.Level != 1)
            {
                orbVariables.IncreaseIndex();
            }
        }
    }
}

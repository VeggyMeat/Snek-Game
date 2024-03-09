using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The clockwork magician class, a subclass of the mage class
/// </summary>
internal class ClockworkMagician : Mage
{
    // has no levels

    /// <summary>
    /// The time delay between permanent buffs
    /// </summary>
    private float buffDelay;

    /// <summary>
    /// The value to increase the attack speed by
    /// </summary>
    private float localAttackSpeedBuff;

    /// <summary>
    /// The value to increase the damage buff by
    /// </summary>
    private float localDamageBuff;

    /// <summary>
    /// the internal level of the magician
    /// </summary>
    private int internalLevel = 0;

    /// <summary>
    /// The number of orbs to be shot each attack
    /// </summary>
    private int orbNumber;

    /// <summary>
    /// Whether the body is currently being regularly buffed
    /// </summary>
    private bool buffing = false;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/ClockworkMagician.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // buffs the magician every buffDelay seconds
        StartBuffing();

        // calls the base setup
        base.Setup();
    }

    /// <summary>
    /// Called to buff the clockwork magician
    /// </summary>
    private void Buff()
    {
        internalLevel++;

        // every 3rd level increases the number of orbs
        if (internalLevel % 3 == 0)
        {
            orbNumber++;
        }

        // gives it the stat buffs at a base level
        body.attackSpeedBuff.AddBuff(localAttackSpeedBuff, true, null);
        body.damageBuff.AddBuff(localDamageBuff, true, null);
        
        // resets the projectile shot timer
        StopRepeatingAttack();
        StartRepeatingAttack();
    }

    /// <summary>
    /// Called regularly by the mage based on timeDelay
    /// </summary>
    protected override void Attack()
    {
        // summons orbNumber orbs at a random angle
        for (int  i = 0; i < orbNumber; i++)
        {
            // spawns the new projectile
            Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, Mathf.PI * 2), orbVariables.Variables, this, body.DamageMultiplier);
        }
    }

    /// <summary>
    /// Starts regularly calling the Buff function
    /// </summary>
    private void StartBuffing()
    {
        // if its already buffing, ignore
        if (buffing)
        {
            return;
        }

        // starts regularly calling the Buff function
        InvokeRepeating(nameof(Buff), buffDelay, buffDelay);

        buffing = true;
    }

    /// <summary>
    /// Stops the regular buffing
    /// </summary>
    private void StopBuffing()
    {
        // if not buffing, ignore
        if (!buffing)
        {
            return;
        }

        // stops calling the buff function
        CancelInvoke(nameof(Buff));

        buffing = false;
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // ends the buff increses on death
        StopBuffing();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // restarts the buff increases when revived
        StartBuffing();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // only ever called once
        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref buffDelay, nameof(buffDelay));
        jsonData.Setup(ref localAttackSpeedBuff, nameof(localAttackSpeedBuff));
        jsonData.Setup(ref localDamageBuff, nameof(localDamageBuff));
    }
}

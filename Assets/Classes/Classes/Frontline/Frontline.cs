using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The base class for all frontline classes
/// </summary>
internal abstract class Frontline : Class
{
    /// <summary>
    /// The time delay between attacks
    /// </summary>
    protected float attackDelay;

    /// <summary>
    /// The radius around the body to scan for enemies
    /// </summary>
    protected float scanRadius;

    /// <summary>
    /// Whether the class should scan for enemies to attack
    /// </summary>
    protected bool scanAttack = true;

    /// <summary>
    /// The damage the class does
    /// </summary>
    protected int damage;

    /// <summary>
    /// Whether the class should automatically attack regularly
    /// </summary>
    protected bool regularAttack;

    /// <summary>
    /// Whether the class is currently scanning for enemies to attack
    /// </summary>
    private bool scanning = false;

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        // adds the frontline class name to the body's classes for identification
        body.classNames.Add(nameof(Frontline));

        base.Setup();

        if (regularAttack)
        {
            StartRepeatingScan();
        }
    }

    /// <summary>
    /// Finds an enemy within range of the body then calls attack on it (or just calls attack if scanAttack is false)
    /// </summary>
    protected void ScanRange()
    {
        // if its not a scan attack, just call the attack function
        if (!scanAttack)
        {
            Attack(transform.position);
        }

        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, scanRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        // if there is an enemy nearby
        if (enemiesInCircle.Length > 0)
        {
            // get a position of a random enemy
            Vector3 enemyPos = enemiesInCircle[Random.Range(0, enemiesInCircle.Length)].transform.position;

            // attack it
            Attack(enemyPos);
        }
    }

    /// <summary>
    /// Calls the ScanRange function every attackDelay seconds
    /// </summary>
    internal void StartRepeatingScan()
    {
        // if scanning already, ignore
        if (scanning)
        {
            return;
        }

        // start scanning and notes it
        InvokeRepeating(nameof(ScanRange), attackDelay / body.attackSpeedBuff.Value, attackDelay / body.attackSpeedBuff.Value);

        scanning = true;
    }

    /// <summary>
    /// Cancels the repeating invoke
    /// </summary>
    internal void StopRepeatingScan()
    {
        // if not scanning, ignore
        if (!scanning)
        {
            return;
        }

        // stop scanning and notes it
        CancelInvoke(nameof(ScanRange));

        scanning = false;
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    /// <exception cref="System.NotImplementedException">Called when the child does not override</exception>
    internal virtual void Attack(Vector3 position)
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
            StartRepeatingScan();
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
            StopRepeatingScan();
        }
    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        if (regularAttack)
        {
            // restsrts the repeating scan so that the attack delay is updated
            StopRepeatingScan();
            StartRepeatingScan();
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref scanRadius, nameof(scanRadius));
        jsonData.Setup(ref damage, nameof(damage));
        jsonData.Setup(ref scanAttack, nameof(scanAttack));
        jsonData.Setup(ref regularAttack, nameof(regularAttack));

        jsonData.SetupAction(ref attackDelay, nameof(attackDelay), StopRepeatingScan, StartRepeatingScan, jsonLoaded);
    }
}

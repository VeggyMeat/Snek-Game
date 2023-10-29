using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public abstract class Frontline : Class
{
    protected float attackDelay;
    protected float scanRadius;

    protected bool scanAttack = true;

    protected int damage;

    protected bool regularAttack;

    private bool scanning = false;

    internal override void Setup()
    {
        body.classNames.Add(nameof(Frontline));

        base.Setup();

        if (regularAttack)
        {
            // starts the repeating scan of enemies to attack
            StartRepeatingScan();
        }
    }

    /// <summary>
    /// Finds an enemy within range of the body then calls attack on it (or just calls attack if scanAttack is false)
    /// </summary>
    internal void ScanRange()
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
    /// <param name="position"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    internal virtual void Attack(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    internal override void Revived()
    {
        base.Revived();

        if (regularAttack)
        {
            StartRepeatingScan();
        }
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        if (regularAttack)
        {
            StopRepeatingScan();
        }
    }

    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        if (regularAttack)
        {
            StopRepeatingScan();
            StartRepeatingScan();
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref scanRadius, nameof(scanRadius));
        jsonData.Setup(ref damage, nameof(damage));
        jsonData.Setup(ref scanAttack, nameof(scanAttack));

        if (jsonData.ContainsKey(nameof(attackDelay)))
        {
            // sets the attack delay
            attackDelay = float.Parse(jsonData[nameof(attackDelay)].ToString());

            // resets scanning if json already loaded
            if (jsonLoaded)
            {
                StopRepeatingScan();
                StartRepeatingScan();
            }
        }
        if (jsonData.ContainsKey(nameof(regularAttack)))
        {
            // sets the regular attack variable
            regularAttack = bool.Parse(jsonData[nameof(regularAttack)].ToString());

            // if the json is already loaded, either stop or start scanning
            if (jsonLoaded)
            {
                if (regularAttack)
                {
                    StartRepeatingScan();
                }
                else
                {
                    StopRepeatingScan();
                }
            }
        }
    }
}

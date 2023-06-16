using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// a class just for the sake of being inherited from
public class Frontline : Class
{
    public float attackDelay;
    public float scanRadius;

    public int damage;

    // currently unused
    public int force;

    public bool regularAttack;

    // called when class body instantiated
    internal override void Setup()
    {
        body.classNames.Add("Frontline");

        base.Setup();

        if (regularAttack )
        {
            // starts the repeating scan of enemies to attack
            StartRepeatingScan();
        }
    }

    // when called, finds an enemy within range of hte body
    internal void ScanRange()
    {
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

    // calls the ScanRange function every attackDelay seconds
    internal void StartRepeatingScan()
    {
        InvokeRepeating(nameof(ScanRange), attackDelay / body.attackSpeedBuff.Value, attackDelay / body.attackSpeedBuff.Value);
    }

    // cancels the repeating invoke
    internal void StopRepeatingScan()
    {
        CancelInvoke(nameof(ScanRange));
    }

    // stops and restarts it, incase of a change in speed
    internal void ResetRepeatingScan()
    {
        StopRepeatingScan();

        StartRepeatingScan();
    }

    // a placeholder which is replaced by the class that inherits from it
    internal virtual void Attack(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    // called when the body is revived from the dead
    internal override void Revived()
    {
        base.Revived();

        if (regularAttack)
        {
            // starts the repeating scan of enemies to attack
            StartRepeatingScan();
        }
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        if (regularAttack)
        {
            // stops attacking
            StopRepeatingScan();
        }
    }

    // called when the attack speed buff changes
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        if (regularAttack)
        {
            // resets the repeating scan
            ResetRepeatingScan();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The bomb class, a miscellanious class
/// </summary>
internal class Bomb : Class
{
    /// <summary>
    /// The chance of an enemy dropping XP
    /// </summary>
    private float XPChance;

    /// <summary>
    /// The delay between attacks
    /// </summary>
    private int timeDelay;
    
    /// <summary>
    /// The radius at which the bomb deals damage to enemies
    /// </summary>
    private int radius;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/Bomb.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // indicates that this is a misc class
        body.classNames.Add("Misc");

        base.Setup();

        // starts attacking
        StartAttacking();
    }

    /// <summary>
    /// Starts repeatedly calling the attack function
    /// </summary>
    internal void StartAttacking()
    {
        InvokeRepeating(nameof(Attack), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);
    }

    /// <summary>
    /// Stops repeatedly calling the attack function
    /// </summary>
    internal void StopAttacking()
    {
        CancelInvoke(nameof(Attack));
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        StopAttacking();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        StartAttacking();
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy">The enemy's GameObject</param>
    internal override void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        // randomly gives xp or not based on XPChance
        if (Random.Range(0f, 1f) < XPChance)
        {
            body.snake.IncreaseXP(enemy.GetComponent<EnemyController>().XPDrop);
        }
    }

    /// <summary>
    /// Kills all the enemies within the radius
    /// </summary>
    private void Attack()
    {
        // gets all the objects within the range of the body
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, radius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        foreach (Collider2D enemy in enemiesInCircle)
        {
            // gets the game object and the script
            GameObject enemyObj = enemy.gameObject;
            EnemyController enemyController = enemyObj.GetComponent<EnemyController>();

            // if the enemy is not dead
            if (!enemyController.Dead)
            {
                // kills the enemy
                enemyController.Die();
                EnemyKilled(enemyObj);
            }
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

        // resets the repeating attack
        StopAttacking();
        StartAttacking();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref XPChance, nameof(XPChance));
        jsonData.Setup(ref timeDelay, nameof(timeDelay));
        jsonData.Setup(ref radius, nameof(radius));
    }
}

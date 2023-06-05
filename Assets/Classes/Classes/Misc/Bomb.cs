using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : Class
{
    public float XPChance;
    public int timeDelay;
    public int radius;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Misc/Bomb.json";

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        // starts attacking
        StartAttacking();

        base.Setup();
    }

    // starts repeatedly calling the attack function
    internal void StartAttacking()
    {
        InvokeRepeating(nameof(Attack), timeDelay, timeDelay);
    }

    // stops repeatedly calling the attack function
    internal void StopAttacking()
    {
        CancelInvoke(nameof(Attack));
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        StopAttacking();
    }

    internal override void Revived()
    {
        base.Revived();

        StartAttacking();
    }

    internal override void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        // randomly gives xp or not based on XPChance
        if (Random.Range(0f, 1f) < XPChance)
        {
            snake.IncreaseXP(enemy.GetComponent<EnemyController>().XPDrop);
        }
    }

    internal void Attack()
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
            if (!enemyController.dead)
            {
                // kills the enemy
                enemyController.Die();
            }
        }
    }
}

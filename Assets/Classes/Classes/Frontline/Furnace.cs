using System.Collections.Generic;
using UnityEngine;
public class Furnace : Frontline
{
    private float burnDelay;

    private float burnRange;

    private readonly List<EnemyController> burnEnemies = new List<EnemyController>();
    private readonly List<EnemyController> deadEnemies = new List<EnemyController>();

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Furnace.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        StartBurningEnemies();
    }

    internal override void Attack(Vector3 position)
    {
        // show an AOE effect of the burn

        // clears the current set of burning enemies
        burnEnemies.Clear();

        // gets all the objects in the range
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(position, burnRange);

        // gets all of the enemies within the range
        Collider2D[] enemiesInRange = System.Array.FindAll(objectsInRange, obj => obj.CompareTag("Enemy"));

        if (enemiesInRange.Length == 0)
        {
            return;
        }

        foreach (Collider2D enemy in enemiesInRange)
        {
            GameObject enemyObject = enemy.gameObject;

            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();

            // adds the enemy to the burn list
            burnEnemies.Add(enemyController);
        }
    }

    private void BurnEnemy(EnemyController enemyController)
    {
        // if the enemy is dead, remove it from the list
        if (enemyController.Dead)
        {
            burnEnemies.Remove(enemyController);
        }
        else
        {
            // apply damage to the enemy
            if (!enemyController.ChangeHealth(-(int)(damage * body.DamageMultiplier)))
            {
                // enemy has been killed
                EnemyKilled(enemyController.gameObject);

                // remove the enemy from the list
                deadEnemies.Add(enemyController);
            }
        }
    }

    private void BurnEnemies()
    {
        // clears the previous dead enemies
        deadEnemies.Clear();

        foreach (EnemyController enemyController in burnEnemies)
        {
            BurnEnemy(enemyController);
        }

        foreach (EnemyController enemyController in deadEnemies)
        {
            burnEnemies.Remove(enemyController);
        }
    }

    private void StartBurningEnemies()
    {
        InvokeRepeating(nameof(BurnEnemies), burnDelay, burnDelay);
    }

    private void StopBurningEnemies()
    {
        CancelInvoke(nameof(BurnEnemies));
    }

    internal override void OnDeath()
    {
        StartBurningEnemies();

        base.OnDeath();
    }

    internal override void Revived()
    {
        base.Revived();

        StartBurningEnemies();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("burnDelay"))
        {
            burnDelay = int.Parse(jsonData["burnDelay"].ToString());

            // reset the burning enemies loop
            StopBurningEnemies();
            StopBurningEnemies();
        }

        jsonData.Setup(ref burnRange, "burnRange");
    }
}

using System.Collections.Generic;
using UnityEngine;
internal class Furnace : Frontline
{
    private float burnDelay;

    private float burnRange;

    private readonly List<EnemyController> burnEnemies = new List<EnemyController>();

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Furnace.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // starts burning the enemies
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

        // if there are no enemies, ignore it
        if (enemiesInRange.Length == 0)
        {
            return;
        }

        foreach (Collider2D enemy in enemiesInRange)
        {
            // grab the gameObject and enemyController
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
                burnEnemies.Remove(enemyController);
            }
        }
    }

    private void BurnEnemies()
    {
        // goes through each enemy and burns them
        for (int i = burnEnemies.Count - 1; i >= 0; i--)
        {
            EnemyController enemyController = burnEnemies[i];
            BurnEnemy(enemyController);
        }
    }

    private void StartBurningEnemies()
    {
        // calls BurnEnemies every burnDelay seconds
        InvokeRepeating(nameof(BurnEnemies), burnDelay, burnDelay);
    }

    private void StopBurningEnemies()
    {
        // stops calling BurnEnemies regularly
        CancelInvoke(nameof(BurnEnemies));
    }

    internal override void OnDeath()
    {
        // stops burning the enemies
        StartBurningEnemies();

        base.OnDeath();
    }

    internal override void Revived()
    {
        base.Revived();

        // continues burning the enemies
        StartBurningEnemies();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref burnDelay, nameof(burnDelay), StopBurningEnemies, StartBurningEnemies, jsonLoaded);
        jsonData.Setup(ref burnRange, nameof(burnRange));
    }
}

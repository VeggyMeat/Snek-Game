using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The Furnace class, a subclass of the frontline class
/// </summary>
internal class Furnace : Frontline
{
    /// <summary>
    /// The delay between burning the enemies
    /// </summary>
    private float burnDelay;

    /// <summary>
    /// The range around the body to burn enemies
    /// </summary>
    private float burnRange;

    /// <summary>
    /// The enemies currently being burned
    /// </summary>
    private readonly List<EnemyController> burnEnemies = new List<EnemyController>();

    /// <summary>
    /// The time the AOEEffect should stay on screen for
    /// </summary>
    private float AOEEffectTime;

    /// <summary>
    /// Whether the AOEEffect should decay in colour over time (true) or not (false)
    /// </summary>
    private bool AOEEffectDecay;

    /// <summary>
    /// The initial colour of the AOEEffect
    /// </summary>
    private Color AOEEffectColour;

    /// <summary>
    /// Whether the furnace is currently burning the enemies
    /// </summary>
    private bool burning = false;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/Furnace.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // starts burning the enemies
        StartBurningEnemies();
    }

    /// <summary>
    /// Called regularly by Frontline based on timeDelay
    /// </summary>
    /// <param name="position">The position which should be attacked</param>
    internal override void Attack(Vector3 position)
    {
        // spawns in the AOEEffect
        AOEEffect.CreateCircle(position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, burnRange);

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

    /// <summary>
    /// Burns a perticular enemy
    /// </summary>
    /// <param name="enemyController">The enemy to burn</param>
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

    /// <summary>
    /// Burns all the enemies in the burnEnemies list
    /// </summary>
    private void BurnEnemies()
    {
        // goes through each enemy and burns them
        for (int i = burnEnemies.Count - 1; i >= 0; i--)
        {
            EnemyController enemyController = burnEnemies[i];
            BurnEnemy(enemyController);
        }
    }

    /// <summary>
    /// Starts burning the enemies
    /// </summary>
    private void StartBurningEnemies()
    {
        if (burning)
        {
            return;
        }

        // calls BurnEnemies every burnDelay seconds
        InvokeRepeating(nameof(BurnEnemies), burnDelay, burnDelay);

        burning = true;
    }

    /// <summary>
    /// Stops burning the enemies
    /// </summary>
    private void StopBurningEnemies()
    {
        if (!burning)
        {
            return;
        }

        // stops calling BurnEnemies regularly
        CancelInvoke(nameof(BurnEnemies));

        burning = false;
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        // stops burning the enemies
        StartBurningEnemies();

        base.OnDeath();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // continues burning the enemies
        StartBurningEnemies();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.SetupAction(ref burnDelay, nameof(burnDelay), StopBurningEnemies, StartBurningEnemies, jsonLoaded);
        jsonData.Setup(ref burnRange, nameof(burnRange));
    }
}

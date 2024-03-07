using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The pyromaniac class, a subclass of the enchanter class
/// </summary>
internal class Pyromaniac : Enchanter
{
    /// <summary>
    /// How much damage the burn does each tick
    /// </summary>
    private int burnDamage;

    /// <summary>
    /// The number of stacks of burn to apply to an enemy (how many times it burns before stopping)
    /// </summary>
    private int burnStacks;

    /// <summary>
    /// The delay between each burn tick
    /// </summary>
    private float burnDelay;

    /// <summary>
    /// Whether the class is currently burning enemies or not
    /// </summary>
    private bool burning = false;

    /// <summary>
    /// The list of enemies that are currently being burnt
    /// </summary>
    private readonly List<List<EnemyController>> enemies = new List<List<EnemyController>>();

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Pyromaniac.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // creates the blank lists containing the enemies
        for (int i = 0; i < burnStacks; i++)
        {
            enemies.Add(new List<EnemyController>());
        }

        // applies burn to each enemy that loses health
        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);

        // starts burning the enemies in the 'enemies' list
        StartBurning();
    }

    /// <summary>
    /// Applies burn to an enemy
    /// </summary>
    /// <param name="enemy">The enemy to apply burn to</param>
    /// <returns>The enemy to apply burn to</returns>
    private EnemyController ApplyBurn(EnemyController enemy)
    {
        Debug.Log("Applying burn to enemy");

        // adds the enemy to the list of enemies to be burned, in the final list
        enemies[burnStacks - 1].Add(enemy);

        return enemy;
    }

    /// <summary>
    /// Deals damage to the bodies that are meant to be burned,
    /// Then removes the first list of enemies and adds a new blank one
    /// </summary>
    private void BurnBodies()
    {
        // goes through each list of enemies
        foreach (List<EnemyController> enemyList in enemies)
        {
            // goes through each enemy in that list
            for (int i = enemyList.Count - 1; i > -1; i--)
            {
                // grabs the enemy
                EnemyController enemy = enemyList[i];

                // if the enemy is dead, remove it from the list, then continue
                if (enemy.Dead)
                {
                    enemyList.RemoveAt(i);
                }
                // otherwise deal damage to every enemy, making the enemy not send a trigger
                else
                {
                    enemy.ChangeHealth(-(int)(burnDamage * body.DamageMultiplier), true);
                }
            }
        }

        // removes the first list, and adds a new blank one
        enemies.RemoveAt(0);
        if (enemies.Count < burnStacks)
        {
            enemies.Add(new List<EnemyController>());
        }
    }

    /// <summary>
    /// Starts repeatedly calling the burn bodies method
    /// </summary>
    private void StartBurning()
    {
        // if burning, ignore
        if (burning)
        {
            return;
        }

        // start burning and note that it is burning
        InvokeRepeating(nameof(BurnBodies), burnDelay, burnDelay);

        burning = true;
    }

    /// <summary>
    /// Stops repeatedly calling the burn bodies method
    /// </summary>
    private void StopBurning()
    {
        // if not burning, ignore
        if (!burning)
        {
            return;
        }

        // stop burning and note that it is no longer burning
        CancelInvoke(nameof(BurnBodies));

        burning = false;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // if burnStacks changes
        if (jsonData.ContainsKey("burnStacks"))
        {
            // get the new value
            int newBurnStacks = int.Parse(jsonData["burnStacks"].ToString());

            if (newBurnStacks > burnStacks)
            {
                // adds the difference number of lists to 'enemies' (it will always be greater)
                for (int i = 0; i < newBurnStacks - burnStacks; i++)
                {
                    enemies.Add(new List<EnemyController>());
                }
            }

            // sets the new value
            burnStacks = newBurnStacks;
        }

        jsonData.Setup(ref burnDamage, nameof(burnDamage));
        jsonData.SetupAction(ref burnDelay, nameof(burnDelay), StopBurning, StartBurning, jsonLoaded);
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    { 
        base.Revived();

        // adds back the trigger
        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the trigger
        TriggerManager.EnemyLostHealthTrigger.RemoveTrigger(ApplyBurn);
    }
}

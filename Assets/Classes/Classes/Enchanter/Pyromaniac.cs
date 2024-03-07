using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

internal class Pyromaniac : Enchanter
{
    private int burnDamage;
    private int burnStacks;
    private float burnDelay;

    private bool burning = false;

    private readonly List<List<EnemyController>> enemies = new List<List<EnemyController>>();

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Pyromaniac.json";

        base.ClassSetup();
    }

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

    private EnemyController ApplyBurn(EnemyController enemy)
    {
        // adds the enemy to the list of enemies to be burned, in the final list
        enemies[burnStacks - 1].Add(enemy);

        return null;
    }

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
                // otherwise
                else
                {
                    // deal damage to every enemy
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
                // adds the difference number of lists to 'enemies'
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

    internal override void Revived()
    { 
        base.Revived();

        // adds back the trigger
        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the trigger
        TriggerManager.EnemyLostHealthTrigger.RemoveTrigger(ApplyBurn);
    }
}

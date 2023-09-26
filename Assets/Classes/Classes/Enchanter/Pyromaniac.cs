using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class Pyromaniac : Enchanter
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

        for (int i = 0; i < burnStacks; i++)
        {
            enemies.Add(new List<EnemyController>());
        }

        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);

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
        foreach (List<EnemyController> enemyList in enemies)
        {
            for (int i = enemyList.Count - 1; i > -1; i--)
            {
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
        enemies.Add(new List<EnemyController>());
    }

    private void StartBurning()
    {
        if (burning)
        {
            return;
        }

        InvokeRepeating(nameof(BurnBodies), burnDelay, burnDelay);

        burning = true;
    }

    private void StopBurning()
    {
        if (!burning)
        {
            return;
        }

        CancelInvoke(nameof(BurnBodies));

        burning = false;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("burnStacks"))
        {
            int newBurnStacks = int.Parse(jsonData["burnStacks"].ToString());

            if (newBurnStacks > burnStacks)
            {
                for (int i = 0; i < newBurnStacks - burnStacks; i++)
                {
                    enemies.Add(new List<EnemyController>());
                }
            }

            burnStacks = newBurnStacks;
        }

        jsonData.Setup(ref burnDamage, nameof(burnDamage));
        
        if (jsonData.ContainsKey("burnDelay"))
        {
            burnDelay = float.Parse(jsonData["burnDelay"].ToString());

            StopBurning();
            StartBurning();
        }
    }

    internal override void Revived()
    { 
        base.Revived();

        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);
    }

    internal override void OnDeath()
    {
        TriggerManager.EnemyLostHealthTrigger.AddTrigger(ApplyBurn);

        base.OnDeath();
    }
}

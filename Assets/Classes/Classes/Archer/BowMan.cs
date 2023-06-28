using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BowMan : Archer
{
    private int projectileCount;
    private int enemyDeathVolleyCount;
    
    private string projectilePath;
    private string projectileJson;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        if (body.isDead)
        {
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            // creates and sets up a new projectile
            Projectile.Shoot(projectile, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), projectileJson, this, body.DamageMultiplier);
        }
    }

    // on killing an enemy, shoots a volley round of projectiles
    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        for (int i = 0; i < enemyDeathVolleyCount; i++) 
        {
            LaunchProjectile();
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "projectileCount":
                    projectileCount = Convert.ToInt32(jsonData[item]);
                    break;
                case "enemyDeathVolleyCount":
                    enemyDeathVolleyCount = Convert.ToInt32(jsonData[item]);
                    break;
                case "projectilePath":
                    projectilePath = Convert.ToString(jsonData[item]);
                    break;
                case "projectileJson":
                    projectileJson = Convert.ToString(jsonData[item]);
                    break;
            }
        }
    }
}

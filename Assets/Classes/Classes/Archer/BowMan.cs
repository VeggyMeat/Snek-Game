using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BowMan : Archer
{
    public int projectileCount;
    public int enemyDeathVolleyCount;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";
    
    public string projectilePath;
    public string projectileJson;

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        if (isDead)
        {
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            // creates and sets up a new projectile
            Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileJson, this);
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
}

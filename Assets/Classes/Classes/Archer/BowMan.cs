using Newtonsoft.Json;
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

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";

        base.ClassSetup();
    }

    internal override void LaunchProjectile()
    {
        // if its dead, then dont launch a projectile
        if (body.IsDead)
        {
            return;
        }

        // launches a volley of projectiles
        for (int i = 0; i < projectileCount; i++)
        {
            // creates and sets up a new projectile
            Projectile.Shoot(projectile, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        // launches a volley of projectiles
        for (int i = 0; i < enemyDeathVolleyCount; i++) 
        {
            LaunchProjectile();
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref projectileCount, nameof(projectileCount));
        jsonData.Setup(ref enemyDeathVolleyCount, nameof(enemyDeathVolleyCount));
    }
}

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
    
    private string projectilePath;
    private string projectileJson;

    private JsonVariable arrowVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // gets the json data and loads it into the arrowVariables
        arrowVariables = new JsonVariable(projectileJson);

        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        if (body.IsDead)
        {
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            // creates and sets up a new projectile
            Projectile.Shoot(projectile, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), arrowVariables.Variables, this, body.DamageMultiplier);
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

        jsonData.Setup(ref projectileCount, "projectileCount");
        jsonData.Setup(ref enemyDeathVolleyCount, "enemyDeathVolleyCount");
        jsonData.Setup(ref projectilePath, "projectilePath");
        jsonData.Setup(ref projectileJson, "projectileJson");
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            arrowVariables.IncreaseIndex();
        }
    }
}

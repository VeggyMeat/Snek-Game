using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gambler : Archer
{
    private string projectilePath;
    private string projectileJson;

    private float minRadius;
    private float maxRadius;

    private float minDamage;
    private float maxDamage;

    private List<Dictionary<string, object>> projectileVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        projectileVariables = Projectile.LoadVariablesFromJson(projectileJson);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        // picks a random size
        float radius = Random.Range(minRadius, maxRadius);
        float damage = Map(minRadius, maxRadius, minDamage, maxDamage, radius);

        // creates and sets up a new projectile
        ProjectileController controller = Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables[body.Level - 1], this, 1f);

        // sets the damage of the projectile
        controller.damage = (int)(damage * body.DamageMultiplier);

        // sets the size of the projectile
        controller.transform.localScale = new Vector3(radius, radius, 1);
    }

    // maps one set of values to another
    private float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "projectilePath":
                    projectilePath = (string)jsonData[item];
                    break;
                case "projectileJson":
                    projectileJson = (string)jsonData[item];
                    break;
                case "minRadius":
                    minRadius = Convert.ToSingle(jsonData[item]);
                    break;
                case "maxRadius":
                    maxRadius = Convert.ToSingle(jsonData[item]);
                    break;
                case "minDamage":
                    minDamage = Convert.ToSingle(jsonData[item]);
                    break;
                case "maxDamage":
                    maxDamage = Convert.ToSingle(jsonData[item]);
                    break;
            }
        }
    }
}

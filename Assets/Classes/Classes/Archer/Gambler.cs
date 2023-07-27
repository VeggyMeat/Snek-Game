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

    private JsonVariable projectileVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        projectileVariables = new JsonVariable(projectileJson);

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
        ProjectileController controller = Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, 1f);

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

        jsonData.Setup(ref projectilePath, "projectilePath");
        jsonData.Setup(ref projectileJson, "projectileJson");
        jsonData.Setup(ref minRadius, "minRadius");
        jsonData.Setup(ref maxRadius, "maxRadius");
        jsonData.Setup(ref minDamage, "minDamage");
        jsonData.Setup(ref maxDamage, "maxDamage");
    }

    internal override void LevelUp()
    {
        base.LevelUp();
        if (body.Level != 1)
        {
            projectileVariables.IncreaseIndex();
        }
    }
}

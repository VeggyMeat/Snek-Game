using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gambler : Archer
{
    private float minRadius;
    private float maxRadius;

    private float maxDamageMultiplier;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

        base.ClassSetup();
    }

    internal override void LaunchProjectile()
    {
        // picks a random size
        float radius = Random.Range(minRadius, maxRadius);

        // gets the damage based on the size
        float damageMultiplier = Map(minRadius, maxRadius, 1, maxDamageMultiplier, radius);

        // creates and sets up a new projectile
        ProjectileController controller = Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, damageMultiplier);

        // sets the size of the projectile
        controller.transform.localScale = new Vector3(radius, radius, 1);
    }

    // maps one set of values onto another
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

        // not allowed to change after initialisation
        jsonData.Setup(ref projectilePath, nameof(projectilePath));
        jsonData.Setup(ref projectileJson, nameof(projectileJson));

        jsonData.Setup(ref minRadius, nameof(minRadius));
        jsonData.Setup(ref maxRadius, nameof(maxRadius));
        jsonData.Setup(ref maxDamageMultiplier, nameof(maxDamageMultiplier));
    }

    internal override void LevelUp()
    {
        base.LevelUp();
    }
}

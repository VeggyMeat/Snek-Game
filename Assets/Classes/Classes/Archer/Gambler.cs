using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Gambler : Archer
{
    public string projectilePath;
    public string projectileJson;

    public float minRadius;
    public float maxRadius;

    public float minDamage;
    public float maxDamage;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Gambler.json";

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
        // picks a random size
        float radius = Random.Range(minRadius, maxRadius);
        float damage = Map(minRadius, maxRadius, minDamage, maxDamage, radius);

        // creates and sets up a new projectile
        ProjectileController controller = Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileJson, this, 1f);

        // sets the damage of the projectile
        controller.damage = (int)(damage * body.DamageMultiplier);

        // sets the size of the projectile
        controller.transform.localScale = new Vector3(radius, radius, 1);
    }

    // maps one set of values to another
    public float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}

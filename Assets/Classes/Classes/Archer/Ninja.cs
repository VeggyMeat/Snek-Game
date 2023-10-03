using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Archer
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Ninja.json";

        base.ClassSetup();
    }

    internal override void LaunchProjectile()
    {
        // creates and sets up a new projectile
        Projectile.Shoot(projectile, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
    }
}

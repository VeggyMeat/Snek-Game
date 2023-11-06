using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Archer
{
    private int projectiles;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Ninja.json";

        base.ClassSetup();
    }

    internal override void LaunchProjectile()
    {
        // creates and sets up 'projectiles' projectiles
        for (int i = 0; i < projectiles; i++)
        {
            Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref projectiles, nameof(projectiles));
    }
}

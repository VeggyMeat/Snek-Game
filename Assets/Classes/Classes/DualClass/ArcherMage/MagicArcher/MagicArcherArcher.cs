using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArcherArcher : Archer
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MagicArcher/MagicArcherArcher.json";

        base.ClassSetup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        // creates and sets up a new projectile
        Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
    }

    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        // tells the mage to launch the orbs
        ((MagicArcherMage)body.classes[1]).LaunchOrbs(enemy);
    }
}

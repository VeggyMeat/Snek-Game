using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArcherMage : Mage
{
    private float orbs;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MagicArcher/MagicArcherMage.json";

        // indicates its not the primary class of the body
        primary = false;

        base.ClassSetup();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbs, "orbs");
    }

    internal void LaunchOrbs(GameObject enemy)
    {
        // picks a random angle between 0 and pi / 2
        float angle = Random.Range(0, Mathf.PI / 2);

        // launches 'orbs' orbs evenly distributed in a circle
        for (double i = 0; i < Mathf.PI * 2; i += Mathf.PI * 2 / orbs)
        {
            // launches a projectile
            Projectile.Shoot(orbTemplate, enemy.transform.position, angle + (float)i, orbVariables.Variables, this, body.DamageMultiplier, addOwnerVelocity: false);
        }
    }
}

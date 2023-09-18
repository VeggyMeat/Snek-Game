using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArcherMage : Mage
{
    private JsonVariable orbVariables;

    private string projectilePath;
    private string projectileJson;

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            orbVariables.IncreaseIndex();
        }
    }

    internal void LaunchOrbs(GameObject enemy)
    {
        // picks a random angle between 0 and pi / 2
        float angle = Random.Range(0, Mathf.PI / 2);

        for (float i = 0; i < Mathf.PI * 2;  i += Mathf.PI / 2)
        {
            // launches a projectile
            Projectile.Shoot()
        }
    }
}

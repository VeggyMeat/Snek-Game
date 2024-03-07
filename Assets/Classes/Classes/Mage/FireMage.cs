using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

internal class FireMage : Mage
{
    private int orbNumber;
    private float rotation;
    private float orbVariation;

    private float angleFacing;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/FireMage.json";

        base.ClassSetup();
    }

    internal override void Attack()
    {
        // rotates the mage slightly
        angleFacing += rotation;
        angleFacing %= Mathf.PI * 2;

        for (int i = 0; i < orbNumber; i++) 
        {
            // pick a random angle variation
            angleFacing += Random.Range(-orbVariation, orbVariation);

            // creates and sets up a new projectile
            Projectile.Shoot(orbTemplate, transform.position, angleFacing, orbVariables.Variables, this, body.DamageMultiplier);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref rotation, nameof(rotation));
        jsonData.Setup(ref orbVariation, nameof(orbVariation));
    }
}

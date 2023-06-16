using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FireMage : Mage
{
    public int orbNumber;
    public float rotation;
    public float orbVariation;

    public string orbPath;
    public string orbJson;

    internal GameObject orb;

    private float angleFacing;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/FireMage.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the orb thats shot
        orb = Resources.Load<GameObject>(orbPath);

        // calls the base setup
        base.Setup();
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
            Projectile.Shoot(orb, transform.position, angleFacing, orbJson, this, body.DamageMultiplier);
        }
    }
}

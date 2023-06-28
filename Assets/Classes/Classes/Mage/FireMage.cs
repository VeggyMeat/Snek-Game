using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FireMage : Mage
{
    private int orbNumber;
    private float rotation;
    private float orbVariation;

    private string orbPath;
    private string orbJson;

    private GameObject orb;

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

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "orbNumber":
                    orbNumber = int.Parse(jsonData[item].ToString());
                    break;
                case "rotation":
                    rotation = float.Parse(jsonData[item].ToString());
                    break;
                case "orbVariation":
                    orbVariation = float.Parse(jsonData[item].ToString());
                    break;
                case "orbPath":
                    orbPath = jsonData[item].ToString();

                    if (jsonLoaded)
                    {
                        // grabs the orb thats shot
                        orb = Resources.Load<GameObject>(orbPath);
                    }

                    break;
                case "orbJson":
                    orbJson = jsonData[item].ToString();
                    break;
            }
        }
    }
}

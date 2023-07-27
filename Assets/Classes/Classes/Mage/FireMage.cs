using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class FireMage : Mage
{
    private int orbNumber;
    private float rotation;
    private float orbVariation;

    private string orbPath;
    private string orbJson;

    private GameObject orb;

    private float angleFacing;

    private JsonVariable orbVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/FireMage.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the orb thats shot
        orb = Resources.Load<GameObject>(orbPath);

        orbVariables = new JsonVariable(orbJson);

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
            Projectile.Shoot(orb, transform.position, angleFacing, orbVariables.Variables, this, body.DamageMultiplier);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbNumber, "orbNumber");
        jsonData.Setup(ref rotation, "rotation");
        jsonData.Setup(ref orbVariation, "orbVariation");
        jsonData.Setup(ref orbJson, "orbJson");
        
        if (jsonData.ContainsKey("orbPath"))
        {
            orbPath = jsonData["orbPath"].ToString();

            if (jsonLoaded)
            {
                // grabs the orb thats shot
                orb = Resources.Load<GameObject>(orbPath);
            }
        }
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            orbVariables.IncreaseIndex();
        }
    }
}

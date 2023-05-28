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

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Mage/FireMage.json";

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        // grabs the orb thats shot
        orb = Resources.Load<GameObject>(orbPath);

        // calls the base setup
        base.Setup();
    }

    internal override void Attack()
    {
        angleFacing += rotation;
        angleFacing %= Mathf.PI * 2;

        for (int i = 0; i < orbNumber; i++) 
        {
            // pick a random angle variation
            angleFacing += Random.Range(-orbVariation, orbVariation);

            // create the orb
            GameObject newOrb = Instantiate(orb, transform.position, Quaternion.Euler(0, 0, angleFacing));

            // gets the controller of the projectile and sets it up
            ProjectileController controller = newOrb.GetComponent<ProjectileController>();
            controller.Setup(orbJson, this);
        }
    }
}

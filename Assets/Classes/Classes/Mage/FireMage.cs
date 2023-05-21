using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FireMage : Mage
{
    public float velocity;
    public float lifeSpan;
    public int orbDamage;
    public int orbNumber;
    public float rotation;
    public float orbVariation;

    public string orbPath;

    internal GameObject orb;

    private float angleFacing;

    internal string jsonPath = "Assets/Resources/jsons/Classes/Mage/FireMage.json";

    internal override void Setup()
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

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

            // create the movement vector
            Vector2 movement = new Vector2(Mathf.Cos(angleFacing), Mathf.Sin(angleFacing)) * velocity;

            // create the orb
            GameObject newOrb = Instantiate(orb, transform.position, Quaternion.identity);

            // gets the controller of the projectile and adds it to the list
            MagicOrbController controller = newOrb.GetComponent<MagicOrbController>();
            controller.Setup(movement + lastMoved, lifeSpan, orbDamage, this);
        }
    }
}

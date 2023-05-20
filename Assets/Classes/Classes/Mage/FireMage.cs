using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMage : Mage
{
    internal float velocity;
    internal GameObject orb;
    internal float lifeSpan;
    internal int orbDamage;
    internal int orbNumber;
    internal float rotation;
    internal float orbVariation;

    private float angleFacing;

    internal override void Setup()
    {
        // sets up the base variables
        velocity = 15f;
        lifeSpan = 1f;
        orbDamage = 10;
        orbNumber = 3;
        rotation = Mathf.PI / 20;
        timeDelay = 0.1f;
        orbVariation = Mathf.PI / 64;
        regularAttack = true;

        // sets up starting variables for the body
        defence = 0;
        maxHealth = 75;
        contactDamage = 5;
        contactForce = 1500;
        velocityContribution = 10f;

        // grabs the orb thats shot
        orb = Resources.Load<GameObject>("Orb1");

        // calls the base setup
        base.Setup();

        // sets the body's colour to a dark green
        GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.1f, 0.1f);
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

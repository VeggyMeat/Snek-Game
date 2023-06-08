using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeramicAutomaton : Mage
{
    public int maxShield;
    public int shieldRegenDelay;

    public string boltJson;
    public string boltPath;

    internal string jsonPath = "Assets/Resources/Jsons/Classes/Mage/CeramicAutomaton.json";

    private int shield;
    private GameObject boltTemplate;

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        // sets the shield number to the maxShield number
        shield = maxShield;

        // grabs the projectile template from the path
        boltTemplate = Resources.Load<GameObject>(boltPath);

        base.Setup();
    }

    // Called when the body takes damage
    internal override bool ChangeHealth(int quantity)
    {
        // if there is a shield left, it ignores damage
        if (shield > 0)
        {
            shield--;
            return true;
        }

        // otherwise it takes damage normally
        return base.ChangeHealth(quantity);
    }

    internal override void Attack()
    {
        // gets a random angle
        float angle = Random.Range(0, Mathf.PI * 2);

        // creates and sets up a new projectile
        Projectile.Shoot(boltTemplate, transform.position, angle, boltJson, this, DamageMultiplier);
    }
}

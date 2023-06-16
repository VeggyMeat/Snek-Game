using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectre : Mage
{
    public int orbNumber;

    public string orbPath;
    public string orbJson;

    public float damageMult;
    public float speedMult;

    internal GameObject orbTemplate;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/Spectre.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the orb thats shot
        orbTemplate = Resources.Load<GameObject>(orbPath);

        // calls the base setup
        base.Setup();
    }

    internal override void Attack()
    {
        for (int i = 0; i < orbNumber; i++)
        {
            // creates and sets up a new projectile
            ProjectileController controller = Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbJson, this, body.DamageMultiplier);

            // if dead, increases the damage by the miltiplier
            if (body.isDead)
            {
                controller.damage = (int)(controller.damage * damageMult);
            }
        }
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // reduces the delay of attacking
        timeDelay /= speedMult;

        // continues attacking after death
        StartRepeatingAttack();
    }

    internal override void Revived()
    {
        // stops the increased attack speed attack
        StopRepeatingAttack();

        base.Revived();

        // sets the delay back to the original amount
        timeDelay *= speedMult;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spectre : Mage
{
    private int orbNumber;

    private string orbPath;
    private string orbJson;

    private float damageMult;
    private float speedMult;

    internal GameObject orbTemplate;

    private JsonVariable orbVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/Spectre.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the orb thats shot
        orbTemplate = Resources.Load<GameObject>(orbPath);

        orbVariables = new JsonVariable(orbJson);

        // calls the base setup
        base.Setup();
    }

    internal override void Attack()
    {
        for (int i = 0; i < orbNumber; i++)
        {
            // creates and sets up a new orb
            ProjectileController controller = Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbVariables.Variables, this, body.DamageMultiplier);

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
                case "damageMult":
                    damageMult = float.Parse(jsonData[item].ToString());
                    break;
                case "speedMult":
                    speedMult = float.Parse(jsonData[item].ToString());
                    break;
                case "orbPath":
                    orbPath = jsonData[item].ToString();

                    if (jsonLoaded)
                    {
                        // grabs the orb thats shot
                        orbTemplate = Resources.Load<GameObject>(orbPath);
                    }

                    break;
                case "orbJson":
                    orbJson = jsonData[item].ToString();
                    break;
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

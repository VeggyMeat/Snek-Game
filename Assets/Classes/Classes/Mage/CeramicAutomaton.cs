using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class CeramicAutomaton : Mage
{
    private int maxShield;
    private float shieldRegenDelay;

    private int shield;
    private bool shieldRegenActive = false;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/CeramicAutomaton.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // sets the shield number to the maxShield number
        shield = maxShield;

        base.Setup();
    }

    internal override float OnDamageTaken(float amount)
    {
        // if there is a shield left, it ignores damage
        if (shield > 0)
        {
            // removes a shield
            shield--;

            // if currently not regenerating a shield, start regenerating
            if (!shieldRegenActive)
            {
                // start regenerating
                Invoke(nameof(RegenShield), shieldRegenDelay);

                // show that its regenerating
                shieldRegenActive = true;
            }

            // return the base effect on 0
            return base.OnDamageTaken(0);
        }

        // return the base effect on the orginal damage amount
        return base.OnDamageTaken(amount);
    }

    // regens one layer of shield
    internal void RegenShield()
    {
        if (shield < maxShield)
        {
            shield++;

            if (shield < maxShield)
            {
                Invoke(nameof(RegenShield), shieldRegenDelay);
            }
            else
            {
                shieldRegenActive = false;
            }
        }
    }

    internal override void Attack()
    {
        // gets a random angle
        float angle = Random.Range(0, Mathf.PI * 2);

        // creates and sets up a new projectile
        Projectile.Shoot(orbTemplate, transform.position, angle, orbVariables.Variables, this, body.DamageMultiplier);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxShield, nameof(maxShield));
        jsonData.Setup(ref shieldRegenDelay, nameof(shieldRegenDelay));
    }
}

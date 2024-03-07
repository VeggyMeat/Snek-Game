using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

internal class Spectre : Mage
{
    private int orbNumber;

    private float damageMult;
    private float speedMult;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/Spectre.json";

        base.ClassSetup();
    }

    internal override void Attack()
    {
        for (int i = 0; i < orbNumber; i++)
        {
            // if dead, increases the damage by the miltiplier
            if (body.IsDead)
            {
                Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbVariables.Variables, this, body.DamageMultiplier * damageMult);
            }
            else
            {
                Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, 2 * Mathf.PI), orbVariables.Variables, this, body.DamageMultiplier);
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

        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref damageMult, nameof(damageMult));
        jsonData.Setup(ref speedMult, nameof(speedMult));
    }
}

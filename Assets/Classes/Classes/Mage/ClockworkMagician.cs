using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ClockworkMagician : Mage
{
    // wont have levels

    private float buffDelay;
    private float localAttackSpeedBuff;
    private float localDamageBuff;

    private float damageMult = 1;
    private int internalLevel = 0;

    private int orbNumber;

    private bool buffing = false;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/ClockworkMagician.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // buffs the magician every buffDelay seconds
        StartBuff();

        // calls the base setup
        base.Setup();
    }

    // called when the magician is buffed
    private void Buff()
    {
        // increases the level
        internalLevel++;

        // every 3rd level increases the number of orbs
        if (internalLevel % 3 == 0)
        {
            orbNumber++;
        }

        // gives it the stat buffs at a base level
        timeDelay /= localAttackSpeedBuff;
        damageMult *= localDamageBuff;
        
        // resets the projectile shot timer
        StopRepeatingAttack();
        StartRepeatingAttack();
    }
    
    // called periodically
    internal override void Attack()
    {
        // summons orbNumber orbs at a random angle
        for (int  i = 0; i < orbNumber; i++)
        {
            // spawns the new projectile
            Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, Mathf.PI * 2), orbVariables.Variables, this, damageMult * body.DamageMultiplier);
        }
    }

    // starts the buff increases every buffDelay seconds
    internal void StartBuff()
    {
        if (buffing)
        {
            return;
        }

        InvokeRepeating(nameof(Buff), buffDelay, buffDelay);

        buffing = true;
    }

    // stops the buff increases
    internal void EndBuff()
    {
        if (!buffing)
        {
            return;
        }

        CancelInvoke(nameof(Buff));

        buffing = false;
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // ends the buff increses on death
        EndBuff();
    }

    internal override void Revived()
    {
        base.Revived();

        // restarts the buff increases when revived
        StartBuff();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbNumber, nameof(orbNumber));
        jsonData.Setup(ref buffDelay, nameof(buffDelay));
        jsonData.Setup(ref localAttackSpeedBuff, nameof(localAttackSpeedBuff));
        jsonData.Setup(ref localDamageBuff, nameof(localDamageBuff));
    }
}

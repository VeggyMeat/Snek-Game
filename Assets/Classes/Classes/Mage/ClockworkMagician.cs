using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockworkMagician : Mage
{
    // wont have levels

    public float buffDelay;
    public float localAttackSpeedBuff;
    public float localDamageBuff;

    private float damageMult = 1;
    private int internalLevel = 0;

    public int orbNumber;

    public string orbPath;
    public string orbJson;

    internal GameObject orbTemplate;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/ClockworkMagician.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // grabs the orb thats shot
        orbTemplate = Resources.Load<GameObject>(orbPath);

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
            ProjectileController projectile = Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, Mathf.PI * 2), orbJson, this, 1f);
            
            // updates its damage
            projectile.damage = (int)(projectile.damage * damageMult * body.DamageMultiplier);
        }
    }

    // starts the buff increases every buffDelay seconds
    internal void StartBuff()
    {
        InvokeRepeating(nameof(Buff), buffDelay, buffDelay);
    }

    // stops the buff increases
    internal void EndBuff()
    {
        CancelInvoke(nameof(Buff));
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
}

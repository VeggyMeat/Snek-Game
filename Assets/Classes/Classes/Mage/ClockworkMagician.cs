using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockworkMagician : Mage
{
    // wont have levels

    private float buffDelay;
    private float localAttackSpeedBuff;
    private float localDamageBuff;

    private float damageMult = 1;
    private int internalLevel = 0;

    private int orbNumber;

    private string orbPath;
    private string orbJson;

    private GameObject orbTemplate;

    private JsonVariable orbVariables;

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

        orbVariables = new JsonVariable(orbJson);

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
            ProjectileController projectile = Projectile.Shoot(orbTemplate, transform.position, Random.Range(0, Mathf.PI * 2), orbVariables.Variables, this, 1f);
            
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
                case "buffDelay":
                    buffDelay = float.Parse(jsonData[item].ToString());
                    break;
                case "localAttackSpeedBuff":
                    localAttackSpeedBuff = float.Parse(jsonData[item].ToString());
                    break;
                case "localDamageBuff":
                    localDamageBuff = float.Parse(jsonData[item].ToString());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro : Class
{
    // potential issue on death, triggerController will still call, a death function will need to be added, called by the body

    // not implemented
    internal float timeDelay = 0;

    internal int maxSummoned = 10;
    internal int lifeSpan = 30;

    internal float zombieSpeed = 3f;
    internal int zombieHealth = 50;
    internal int zombieContactDamage = 20;
    internal int zombieDespawnRadius = 50;
    internal float zombieAngularVelocity = 120f;
    internal int zombieTimeAlive = 15;

    internal TriggerController controller;

    private GameObject zombie;

    internal List<NecromancerZombieController> summonedZombies;

    internal override void Setup()
    {
        // sets up starting variables for the body
        defence = 0;
        maxHealth = 105;
        contactDamage = 10;
        contactForce = 2000;
        velocityContribution = 7.5f;

        // sets up a list of the controlled zombies
        summonedZombies = new List<NecromancerZombieController>();

        // adds this to the enemy death trigger list
        triggerController.addEnemyDeathTrigger(this);

        // gets the zombie asset ready
        zombie = Resources.Load<GameObject>("Zombie1");

        // sets the body's colour to a dark gray
        spriteRenderer.color = new Color(0.25f, 0.25f, 0.25f);

        base.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal override void EnemyKilledTrigger(GameObject enemy)
    {
        base.EnemyKilledTrigger(enemy);

        // if there is capacity for another zombie spawn it
        if (summonedZombies.Count < maxSummoned)
        {
            SummonZombie(enemy.transform);
        }
    }

    // spawns a new friendly zombie at a certain position
    internal void SummonZombie(Transform position)
    {
        // spawns a zombie at the position
        GameObject summonedZombie = Instantiate(zombie, new Vector3(position.position.x, position.position.y, 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        
        // sets up the zombie
        NecromancerZombieController controller = summonedZombie.GetComponent<NecromancerZombieController>();
        controller.Setup(zombieSpeed, zombieHealth, zombieContactDamage, zombieDespawnRadius, zombieAngularVelocity, this, zombieTimeAlive);

        // adds the zombie to the list of controlled zombies
        summonedZombies.Add(controller);
    }

    internal void ZombieDeath(GameObject zombie)
    {
        summonedZombies.Remove(zombie.GetComponent<NecromancerZombieController>());
    }

    // called when the body is revived from the dead
    internal override void Revived()
    {
        base.Revived();

        // adds this back to the list for enemy death triggers
        triggerController.addEnemyDeathTrigger(this);
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes this from the list of enemy death triggers
        triggerController.removeEnemyDeathTrigger(this);
    }
}

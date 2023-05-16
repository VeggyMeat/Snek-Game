using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro : Class
{
    // not implemented
    internal float timeDelay = 0;

    internal int maxSummoned = 10;
    internal int lifeSpan = 30;

    internal float zombieSpeed = 3f;
    internal int zombieHealth = 50;
    internal int zombieContactDamage = 20;
    internal int zombieDespawnRadius = 50;
    internal float zombieAngularVelocity = 60f;
    internal int zombieTimeAlive = 15;

    internal TriggerController controller;

    private GameObject zombie;

    internal List<NecromancerZombieController> summonedZombies;

    internal override void Setup()
    {
        summonedZombies = new List<NecromancerZombieController>();

        base.Setup();

        // gets the controller, and adds this to the enemy death trigger list
        controller.addEnemyDeathTrigger(this);

        zombie = Resources.Load<GameObject>("Zombie1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal override void EnemyKilledTrigger(GameObject enemy)
    {
        base.EnemyKilledTrigger(enemy);

        if (summonedZombies.Count < maxSummoned)
        {
            SummonZombie(enemy.transform);
        }
    }

    internal void SummonZombie(Transform position)
    {
        GameObject summonedZombie = Instantiate(zombie, new Vector3(position.position.x, position.position.y, 0f), Quaternion.identity);

        // summoned zombie
        
        NecromancerZombieController controller = summonedZombie.GetComponent<NecromancerZombieController>();
        
        summonedZombies.Add(controller);

        controller.Setup(zombieSpeed, zombieHealth, zombieContactDamage, zombieDespawnRadius, zombieAngularVelocity, this, zombieTimeAlive);

        Debug.Log(controller.transform.position.x + controller.transform.position.y.ToString() + 0);
    }

    internal void ZombieDeath(GameObject zombie)
    {
        summonedZombies.Remove(zombie.GetComponent<NecromancerZombieController>());
    }
}

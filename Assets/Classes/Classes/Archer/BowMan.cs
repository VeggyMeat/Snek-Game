using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Archer
{
    List<ProjectileController> controllerList = new List<ProjectileController>();
    internal int projectileCount;
    internal int enemyDeathVolleyCount;

    internal override void Setup()
    {
        // calls the archer's setup
        base.Setup();

        // sets up starting variables for the archer
        timeDelay = 0.25f;
        velocity = 10f;
        projectile = Resources.Load<GameObject>("Projectile1");
        lifeSpan = 2.5f;
        projectileDamage = 20;
        projectileCount = 3;
        enemyDeathVolleyCount = 1;
        levelUps = new List<int> { 500, 2500 };

        // also sets up starting variables for the body
        // body.contactDamage = 20;
        // body.contactForce = 2000;
    }

    void Update()
    {

    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        for (int i = 0; i < projectileCount; i++)
        {
            // pick a random angle
            float angle = Random.Range(0, 2 * Mathf.PI);

            // create the movement vector
            Vector2 movement = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * velocity;

            // create the projectile
            GameObject projectile = Instantiate(base.projectile, transform.position, Quaternion.identity);

            // gets the controller of the projectile and adds it to the list
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            controllerList.Add(controller);

            // gives the projectile its properties
            controller.movement = movement + gameObject.GetComponent<BodyController>().lastMoved;
            controller.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + 90);
            controller.lifeSpan = lifeSpan;
            controller.damage = projectileDamage;
            controller.archer = this;
        }
    }

    // on killing an enemy, shoots a volley round of projectiles
    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        for (int i = 0; i < enemyDeathVolleyCount; i++) 
        {
            LaunchProjectile();
        }
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        // on level 2
        if (level == 2)
        {
            // increases projectileCount, decreases time inbetween
            projectileCount = 5;
            timeDelay = 0.2f;
        }
        // on level 3
        else if (level == 3) 
        {
            // doubles the damage of projectiles, increases death volley
            projectileDamage = 40;
            enemyDeathVolleyCount = 3;
        }

        ResetRepeatingProjectile();
    }
}

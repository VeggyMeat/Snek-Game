using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BowMan : Archer
{
    public int projectileCount;
    public int enemyDeathVolleyCount;

    internal string jsonPath = "Assets/Resources/jsons/Classes/Archer/BowMan.json";
    
    public string projectilePath;

    internal override void Setup()
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        if (isDead)
        {
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            // pick a random angle
            float angle = Random.Range(0, 2 * Mathf.PI);

            // create the movement vector
            Vector2 movement = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * velocity;

            // create the projectile
            GameObject projectile = Instantiate(base.projectile, transform.position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90));

            // gets the controller of the projectile and adds it to the list
            ProjectileController controller = projectile.GetComponent<ProjectileController>();
            controller.Setup(movement + lastMoved, lifeSpan, projectileDamage, this);
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
}

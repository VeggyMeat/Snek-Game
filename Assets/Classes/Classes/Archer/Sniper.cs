using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sniper : Archer
{
    private float scanRadius;
    private int damage;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Sniper.json";

        base.ClassSetup();
    }

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, scanRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        if (enemiesInCircle.Length > 0)
        {
            // picks a random enemy
            GameObject enemyObj = enemiesInCircle[Random.Range(0, enemiesInCircle.Length)].gameObject;
            Debug.DrawLine(transform.position, enemyObj.transform.position, Color.white, 3);

            // draw a tracer line indicating the shot (TODO)

            // gets the vector between the enemy and the sniper
            UnityEngine.Vector2 dif = enemyObj.transform.position - transform.position;

            // casts a ray from the sniper to the enemy (and through) getting all things hit
            RaycastHit2D[] objectsHit = Physics2D.RaycastAll(transform.position, dif.normalized);

            foreach (RaycastHit2D objectHit in objectsHit)
            {
                // gets the object hit
                GameObject obj = objectHit.rigidbody.gameObject;

                // if the object hit is an enemy
                if (obj.tag == "Enemy")
                {
                    // gets the enemy controller from the enemy object
                    EnemyController enemy = obj.GetComponent<EnemyController>();

                    // if the enemy is not dead
                    if (!enemy.dead)
                    {
                        // deals damage to the enemy
                        if (!enemy.ChangeHealth(-(int)(damage * body.DamageMultiplier)))
                        {
                            EnemyKilled(obj);
                        }
                    }
                }
            }
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "scanRadius":
                    scanRadius = Convert.ToSingle(jsonData[item]);
                    break;
                case "damage":
                    damage = Convert.ToInt32(jsonData[item]);
                    break;
            }
        }
    }
}
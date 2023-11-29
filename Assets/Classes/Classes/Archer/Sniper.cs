using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class Sniper : Archer
{
    private float scanRadius;
    private int damage;

    private float AOEEffectTime;
    private bool AOEEffectDecay;
    private Color AOEEffectColour;
    private float AOEEffectWidth;

    private const float AOEEffectHeight = 100;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Sniper.json";

        base.ClassSetup();
    }

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

            // draw a tracer line indicating the shot (TODO)
            AOEEffect.CreateRectangle(transform.position, AOEEffectTime, AOEEffectDecay, AOEEffectColour, Mathf.Rad2Deg * ((Vector2)transform.position).AngleTo((Vector2)enemyObj.transform.position), AOEEffectHeight, AOEEffectWidth);

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
                    if (!enemy.Dead)
                    {
                        // deals damage to the enemy
                        if (!enemy.ChangeHealth(-(int)(damage * body.DamageMultiplier)))
                        {
                            // notifies the class that the enemy was killed
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

        jsonData.Setup(ref scanRadius, nameof(scanRadius));
        jsonData.Setup(ref damage, nameof(damage));
        jsonData.Setup(ref AOEEffectColour, nameof(AOEEffectColour));
        jsonData.Setup(ref AOEEffectDecay, nameof(AOEEffectDecay));
        jsonData.Setup(ref AOEEffectTime, nameof(AOEEffectTime));
        jsonData.Setup(ref AOEEffectWidth, nameof(AOEEffectWidth));
    }
}
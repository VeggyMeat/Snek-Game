using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

// COMPLETE

/// <summary>
/// The sniper class, a subclass of the archer class
/// </summary>
internal class Sniper : Archer
{
    /// <summary>
    /// The radius in which the sniper scans for enemies to shoot
    /// </summary>
    private float scanRadius;

    /// <summary>
    /// The damage the sniper does when hitting an enemy
    /// </summary>
    private int damage;


    /// <summary>
    /// The time the AOEEffect should stay on screen for
    /// </summary>
    private float AOEEffectTime;

    /// <summary>
    /// Whether the AOEEffect should decay in colour over time (true) or not (false)
    /// </summary>
    private bool AOEEffectDecay;

    /// <summary>
    /// The initial colour of the AOEEffect
    /// </summary>
    private Color AOEEffectColour;

    /// <summary>
    /// The width of the AOEEffect rectangle
    /// </summary>
    private float AOEEffectWidth;

    /// <summary>
    /// The height of the AOEEffect rectangle
    /// </summary>
    private const float AOEEffectHeight = 100;


    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Sniper.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    protected override void LaunchProjectile()
    {
        // gets all the objects within the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, scanRadius);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        if (enemiesInCircle.Length > 0)
        {
            // picks a random enemy
            GameObject enemyObj = enemiesInCircle[Random.Range(0, enemiesInCircle.Length)].gameObject;


            // draw a tracer line indicating the shot
            // gets the angle the rectangle is facing
            float angle = Mathf.Rad2Deg * ((Vector2)transform.position).AngleTo((Vector2)enemyObj.transform.position) + 90;
            
            // gets the middle position of the AOEEffect rectangle
            Vector2 AOEPosition = new Vector2(transform.position.x + AOEEffectHeight / 2 * (float)Math.Cos(angle * Mathf.Deg2Rad), transform.position.y + AOEEffectHeight / 2 * (float)Math.Sin(angle * Mathf.Deg2Rad));
            
            // creates the AOEEffect rectangle
            AOEEffect.CreateRectangle(AOEPosition, AOEEffectTime, AOEEffectDecay, AOEEffectColour, angle, AOEEffectHeight, AOEEffectWidth);


            // gets the vector between the enemy and the sniper
            Vector2 dif = enemyObj.transform.position - transform.position;

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

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
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
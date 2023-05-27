using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectileController : ProjectileController
{
    private int bounces;

    public int maxBounces;

    // I dont like using start, but here it is
    private void Start()
    {
        bounces = maxBounces;
    }

    // triggers when the projectile collides with something
    internal override void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            if (!body.dead)
            {
                // apply damage to the enemy
                if (!body.ChangeHealth(-damage))
                {
                    // enemy has been killed
                    owner.EnemyKilled(collision.gameObject);

                    bounces--;

                    // if its not out of bounces, bounce it
                    if (bounces >= 0)
                    {
                        // grabs a random angle and sets it as the new angle
                        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2);
                        transform.rotation = Quaternion.Euler(0, 0, angle);

                        // sets the new movement vector
                        SetMovement();
                        return;
                    }
                }
                // kill the projectile
                Die();
            }
        }
    }
}

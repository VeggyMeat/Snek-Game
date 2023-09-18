using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectileController : ProjectileController
{
    // public so that they can be set manually, maybe update this
    public float enemySlowMultiplier;
    public float enemySlowDuration;
    public float spin;

    private void Setup()
    {
        // grabs the velocity
        Rigidbody rb = GetComponent<Rigidbody>();

        // sets the angular velocity
        rb.angularVelocity = new Vector3 (0, 0, spin);
    }

    // triggers when the projectile collides with something
    internal override void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            if (!body.Dead)
            {
                // apply damage to the enemy
                if (!body.ChangeHealth(-damage))
                {
                    // enemy has been killed
                    owner.EnemyKilled(collision.gameObject);
                }
                else
                {
                    // if it survives applies the slow debuff
                    body.speedBuff.AddBuff(enemySlowMultiplier, true, enemySlowDuration);
                }
                // kill the projectile
                Die();
            }
        }
    }
}
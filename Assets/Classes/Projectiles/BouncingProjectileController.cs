using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectileController : ProjectileController
{
    private int bounces;

    private int maxBounces;

    internal override void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        base.Setup(variables, owner, damageMultiplier, addOwnerVelocity);

        // sets bounces equal to max bounces
        bounces = maxBounces;
    }

    // triggers when the projectile collides with something
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

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

                    // reduces the number of bounces left
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

                TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

                // kill the projectile
                Die();

                isDead = true;
            }
        }
    }

    internal override void LoadVariables()
    {
        base.LoadVariables();

        variables.Setup(ref maxBounces, nameof(maxBounces));
    }
}

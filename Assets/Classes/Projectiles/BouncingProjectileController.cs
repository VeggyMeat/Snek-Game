using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// An alternate version of the projectile controller that bounces off of enemies if they die
/// </summary>
internal class BouncingProjectileController : ProjectileController
{
    /// <summary>
    /// The number of remaining bounces
    /// </summary>
    private int bounces;

    /// <summary>
    /// The maximum number of bounces
    /// </summary>
    private int maxBounces;

    /// <summary>
    /// Called by the body to set the projectile up
    /// </summary>
    /// <param name="variables">The projectile's variables</param>
    /// <param name="owner">The owner of the projectile</param>
    /// <param name="damageMultiplier">The damage multiplier of the projectile</param>
    /// <param name="addOwnerVelocity">Whether to add the owner's velocity or not</param>
    internal override void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        base.Setup(variables, owner, damageMultiplier, addOwnerVelocity);

        bounces = maxBounces;
    }

    // Called by unity when the projectile collides with something
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            if (!body.Dead)
            {
                // apply damage to the enemy
                if (!body.ChangeHealth(-damage))
                {
                    owner.EnemyKilled(collision.gameObject);

                    bounces--;

                    // if its not out of bounces, bounce it
                    if (bounces >= 0)
                    {
                        // grabs a random angle and sets it as the new angle
                        float angle = Random.Range(0, Mathf.PI * 2);
                        transform.rotation = Quaternion.Euler(0, 0, angle);

                        SetMovement();

                        TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

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

    /// <summary>
    /// Loads in all the variables
    /// </summary>
    internal override void LoadVariables()
    {
        base.LoadVariables();

        variables.Setup(ref maxBounces, nameof(maxBounces));
    }
}

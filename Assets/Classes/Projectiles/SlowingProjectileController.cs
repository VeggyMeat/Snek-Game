using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class SlowingProjectileController : ProjectileController
{
    // public so that they can be set manually, maybe update this
    private float enemySlowMultiplier;
    private float enemySlowDuration;

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
                }
                else
                {
                    // if it survives applies the slow debuff
                    body.speedBuff.AddBuff(enemySlowMultiplier, true, enemySlowDuration);
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

        variables.Setup(ref enemySlowMultiplier, nameof(enemySlowMultiplier));
        variables.Setup(ref enemySlowDuration, nameof(enemySlowDuration));
    }
}

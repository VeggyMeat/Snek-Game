using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingProjectileController : ProjectileController
{
    // public so that they can be set manually, maybe update this
    private float enemySlowMultiplier;
    private float enemySlowDuration;
    private float spin;

    internal override void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        base.Setup(variables, owner, damageMultiplier, addOwnerVelocity);

        // sets the angular velocity
        selfRigid.angularVelocity = spin;
    }

    // triggers when the projectile collides with something
    protected override void OnTriggerEnter2D(Collider2D collision)
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

                TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

                // kill the projectile
                Die();
            }
        }
    }

    internal override void LoadVariables()
    {
        base.LoadVariables();

        variables.Setup(ref enemySlowMultiplier, nameof(enemySlowMultiplier));
        variables.Setup(ref enemySlowDuration, nameof(enemySlowDuration));
        variables.Setup(ref spin, nameof(spin));
    }
}

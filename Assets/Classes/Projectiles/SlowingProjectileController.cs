using UnityEngine;

// COMPLETE

/// <summary>
/// An alternate version of the projectile controller that slows enemies after they are hit
/// </summary>
internal class SlowingProjectileController : ProjectileController
{
    /// <summary>
    /// Speed multiplier for the enemy
    /// </summary>
    private float enemySlowMultiplier;

    /// <summary>
    /// How long the speed multiplier lasts for
    /// </summary>
    private float enemySlowDuration;

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
                }
                else
                {
                    // if it survives applies the slow debuff
                    body.speedBuff.AddBuff(enemySlowMultiplier, true, enemySlowDuration);
                }

                TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

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

        variables.Setup(ref enemySlowMultiplier, nameof(enemySlowMultiplier));
        variables.Setup(ref enemySlowDuration, nameof(enemySlowDuration));
    }
}

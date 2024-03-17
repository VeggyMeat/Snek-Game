using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// Controls the bullets that are shot by the turrets of the engineer body class
/// </summary>
public class BulletController : MonoBehaviour
{
    /// <summary>
    /// The parent of the bullet
    /// </summary>
    private TurretController parent;

    /// <summary>
    /// The variables for the bullet
    /// </summary>
    private Dictionary<string, object> variables;

    /// <summary>
    /// The bullet's velocity
    /// </summary>
    private float velocity;

    /// <summary>
    /// The bullet's life span
    /// </summary>
    private float lifeSpan;

    /// <summary>
    /// The bullet's damage
    /// </summary>
    private int damage;

    /// <summary>
    /// Sets up the bullet
    /// </summary>
    /// <param name="variables">The bullet's variables</param>
    /// <param name="parent">The parent that created this</param>
    /// <param name="DamageMultiplier">The damage multiplier for the bullet</param>
    internal void Setup(Dictionary<string, object> variables, TurretController parent, float DamageMultiplier)
    {
        this.parent = parent;

        this.variables = variables;
        LoadVariables();

        // kills the projectile when it should die
        Invoke(nameof(Die), lifeSpan);

        // sets the velocity of the bullet
        float angle = transform.rotation.eulerAngles.z;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * velocity, Mathf.Sin(angle) * velocity);

        // updates the damage based upon the damage multiplier of the parent's parent (the engineer)
        damage = (int)(damage * DamageMultiplier);
    }

    // Called by unity when the projectile collides with something
    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // if the dealt damage kills the enemy
            if (!body.ChangeHealth(-damage))
            {
                parent.EnemyKilled(collision.gameObject);
            }

            Die();
        }
    }

    /// <summary>
    /// Called when the bullet dies
    /// </summary>
    internal void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Loads the variables from the json
    /// </summary>
    internal void LoadVariables()
    {
        variables.Setup(ref velocity, nameof(velocity));
        variables.Setup(ref lifeSpan, nameof(lifeSpan));
        variables.Setup(ref damage, nameof(damage));
    }
}

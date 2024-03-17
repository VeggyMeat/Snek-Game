using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The controller that is placed on projectile game objects
/// </summary>
internal class ProjectileController : MonoBehaviour
{
    /// <summary>
    /// The parent of the projectile
    /// </summary>
    protected Class owner;

    /// <summary>
    /// The parent of the projectile
    /// </summary>
    internal Class Owner
    {
        get
        {
            return owner;
        }
    }

    /// <summary>
    /// The rigid body of the projectile
    /// </summary>
    protected Rigidbody2D selfRigid;

    /// <summary>
    /// The velocity of the projectile
    /// </summary>
    protected float velocity;

    /// <summary>
    /// The velocity of the projectile
    /// </summary>
    internal float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    /// <summary>
    /// The life span of the projectile
    /// </summary>
    protected float lifeSpan;

    /// <summary>
    /// The damage the projectile does on contact
    /// </summary>
    protected int damage;

    /// <summary>
    /// The damage the projectile does on contact
    /// </summary>
    internal int Damage
    {
        get
        {
            return damage;
        }
        set 
        { 
            damage = value; 
        }
    }

    /// <summary>
    /// The colour of the projectile
    /// </summary>
    protected Color colour;

    /// <summary>
    /// The x scale of the projectile's body
    /// </summary>
    protected float scaleX;

    /// <summary>
    /// The y scale of the projectile's body
    /// </summary>
    protected float scaleY;

    /// <summary>
    /// Whether the projectile is dead or not
    /// </summary>
    protected bool isDead = false;

    /// <summary>
    /// The variables for the projectile
    /// </summary>
    protected Dictionary<string, object> variables;

    /// <summary>
    /// The id of the projectile
    /// </summary>
    protected int id;

    /// <summary>
    /// The id of the projectile
    /// </summary>
    internal int ID
    {
        get
        {
            return id;
        }
    }

    /// <summary>
    /// Called by the body to set the projectile up
    /// </summary>
    /// <param name="variables">The projectile's variables</param>
    /// <param name="owner">The owner of the projectile</param>
    /// <param name="damageMultiplier">The damage multiplier of the projectile</param>
    /// <param name="addOwnerVelocity">Whether to add the owner's velocity or not</param>
    internal virtual void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        this.variables = variables;
        LoadVariables();

        // sets the scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);

        this.owner = owner;

        damage = (int)(damage * damageMultiplier);

        selfRigid = gameObject.GetComponent<Rigidbody2D>();

        SetMovement(addOwnerVelocity);

        // kills the projectile in lifeSpan seconds
        Invoke(nameof(Die), lifeSpan);

        GetComponent<SpriteRenderer>().color = colour;
    }

    /// <summary>
    /// Called when the projectile dies
    /// </summary>
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when the projectile collides with something
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // if the enemy is dead, ignore it
            if (body.Dead)
            {
                return;
            }

            // apply damage to the enemy
            if (!body.ChangeHealth(-damage))
            {
                owner.EnemyKilled(collision.gameObject);
            }

            TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

            // projectile dies after hitting an enemy
            Die();

            isDead = true;
        }
    }

    /// <summary>
    /// Sets the movement vector based on the place facing, and the velocity
    /// </summary>
    /// <param name="addParentVelocity"></param>
    protected void SetMovement(bool addParentVelocity = false)
    {
        // sets the movement of the projectile
        float angle = transform.rotation.eulerAngles.z + 90;

        if (addParentVelocity)
        {
            // adds the parent velocity to it
            selfRigid.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * velocity, Mathf.Sin(angle * Mathf.Deg2Rad) * velocity) + owner.body.lastMoved;
        }
        else
        {
            // doesnt add the parent velocity
            selfRigid.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * velocity, Mathf.Sin(angle * Mathf.Deg2Rad) * velocity);
        }
    }

    /// <summary>
    /// Loads in all the variables
    /// </summary>
    internal virtual void LoadVariables()
    {
        variables.Setup(ref velocity, nameof(velocity));
        variables.Setup(ref lifeSpan, nameof(lifeSpan));
        variables.Setup(ref damage, nameof(damage));
        variables.Setup(ref colour, nameof(colour));
        variables.Setup(ref scaleX, nameof(scaleX));
        variables.Setup(ref scaleY, nameof(scaleY));
        variables.Setup(ref id, nameof(id));
    }

    /// <summary>
    /// Scale the projectile by a factor
    /// </summary>
    /// <param name="factor">The factor to scale by</param>
    internal void Scale(float factor)
    {
        scaleX *= factor;
        scaleY *= factor;

        // sets the scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}

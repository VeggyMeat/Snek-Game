using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    protected Class owner;

    public Class Owner
    {
        get
        {
            return owner;
        }
    }

    protected Rigidbody2D selfRigid;

    protected float velocity;

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

    protected float lifeSpan;
    protected int damage;

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

    protected float r;
    protected float g;
    protected float b;

    protected float scaleX;
    protected float scaleY;

    protected Dictionary<string, object> variables;

    /// <summary>
    /// Called by the body to set the projectile up
    /// </summary>
    /// <param name="variables">The projectile's variables</param>
    /// <param name="owner">The owner of the projectile</param>
    /// <param name="damageMultiplier">The damage multiplier of the projectile</param>
    /// <param name="addOwnerVelocity">Whether to add the owner's velocity or not</param>
    internal virtual void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        // loads in all the variables from the json
        this.variables = variables;
        LoadVariables();

        // sets the scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);

        // sets the owner
        this.owner = owner;

        // scales the damage based on the multiplier
        damage = (int)(damage * damageMultiplier);

        // gets the rigid body
        selfRigid = gameObject.GetComponent<Rigidbody2D>();

        // sets the movement of the projectile
        SetMovement(addOwnerVelocity);

        // kills the projectile in lifeSpan seconds
        Invoke(nameof(Die), lifeSpan);

        // sets the color of the object
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
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
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // if the enemy is dead, ignore it
            if (body.Dead)
            {
                return;
            }

            // apply damage to the enemy
            if (!body.ChangeHealth(-damage))
            {
                // enemy has been killed
                owner.EnemyKilled(collision.gameObject);
            }

            TriggerManager.ProjectileHitTrigger.CallTrigger(gameObject);

            // destroy the projectile
            Die();
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
        variables.Setup(ref r, nameof(r));
        variables.Setup(ref g, nameof(g));
        variables.Setup(ref b, nameof(b));
        variables.Setup(ref scaleX, nameof(scaleX));
        variables.Setup(ref scaleY, nameof(scaleY));
    }

    internal void Scale(float factor)
    {
        scaleX *= factor;
        scaleY *= factor;

        // sets the scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}

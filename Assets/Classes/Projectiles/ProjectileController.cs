using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Class owner;

    private Rigidbody2D selfRigid;

    public float velocity;
    public float lifeSpan;
    public int damage;

    public float r;
    public float g;
    public float b;

    public float scaleX;
    public float scaleY;

    private Dictionary<string, object> variables;

    internal virtual void Setup(Dictionary<string, object> variables, Class owner, float damageMultiplier, bool addOwnerVelocity = true)
    {
        // loads in all the variables from the json
        this.variables = variables;
        LoadVariables();

        Debug.Log(scaleX);
        Debug.Log(scaleY);

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

    // called when the projectile dies
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    // triggers when the projectile collides with something
    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // apply damage to the enemy
            if (!body.ChangeHealth(-damage))
            {
                // enemy has been killed
                owner.EnemyKilled(collision.gameObject);
            }

            // calls the function to do something after the projectile has hit something
            ProjectilePostHit();
        }
    }

    // called after the projectile has hit something
    internal virtual void ProjectilePostHit()
    {
        // destroy the projectile
        Die();
    }

    // sets the movement vector based on the place facing, and the velocity
    internal void SetMovement(bool addParentVelocity = false)
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
            selfRigid.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * velocity, Mathf.Sin(angle * Mathf.Deg2Rad) * velocity);
        }
    }

    internal void LoadVariables()
    {
        variables.Setup(ref velocity, "velocity");
        variables.Setup(ref lifeSpan, "lifeSpan");
        variables.Setup(ref damage, "damage");
        variables.Setup(ref r, "r");
        variables.Setup(ref g, "g");
        variables.Setup(ref b, "b");
        variables.Setup(ref scaleX, "scaleX");
        variables.Setup(ref scaleY, "scaleY");
    }
}

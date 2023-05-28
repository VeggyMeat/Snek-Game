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

    internal virtual void Setup(string jsonPath, Class owner)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // sets the owner
        this.owner = owner;

        // gets the rigid body
        selfRigid = gameObject.GetComponent<Rigidbody2D>();

        SetMovement(true);

        // kills the projectile in lifeSpan seconds
        Invoke(nameof(Die), lifeSpan);
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
            selfRigid.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * velocity, Mathf.Sin(angle * Mathf.Deg2Rad) * velocity) + owner.lastMoved;
        }
        else
        {
            selfRigid.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * velocity, Mathf.Sin(angle * Mathf.Deg2Rad) * velocity);
        }
    }
}

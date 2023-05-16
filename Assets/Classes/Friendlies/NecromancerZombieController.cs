using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NecromancerZombieController : MonoBehaviour
{
    internal float speed;
    internal float maxHealth;
    internal int contactDamage;
    internal int despawnRadius;
    internal float angularVelocity;
    internal int timeAlive;

    internal float health;
    internal Rigidbody2D selfRigid;
    internal Necro parent;

    private GameObject target = null;
    private Transform targetPos = null;

    // Called just after creation, by whatever created the object
    internal void Setup(float speed, float maxHealth, int contactDamage, int despawnRadius, float angularVelocity, Necro parent, int timeAlive)
    {
        this.speed = speed;
        this.maxHealth = maxHealth;
        this.contactDamage = contactDamage;
        this.despawnRadius = despawnRadius;
        this.angularVelocity = angularVelocity;
        this.parent = parent;
        this.timeAlive = timeAlive;
    }

    void Start()
    {
        // sets up the rigid body
        selfRigid = GetComponent<Rigidbody2D>();

        // sets the health to the max health
        health = maxHealth;

        // sets the body mildly rotating
        selfRigid.angularVelocity = angularVelocity;

        // kills the projectile in lifeSpan seconds
        Invoke(nameof(Die), timeAlive);
    }

    void FixedUpdate()
    {
        if (target is null)
        {
            // shoots 1 ray out infront, if it hits it locks onto that enemy

            RaycastHit2D hit;

            // shoots a raycast infront of the zombie

            hit = Physics2D.Raycast(transform.position, transform.rotation.eulerAngles, 50f);

            if (hit)
            {
                // grabs the enemy gameobject that was hit
                target = hit.collider.gameObject;

                if (target.tag == "Enemy")
                {
                    // grabs the enemy's transform
                    targetPos = target.transform;

                    // stops the zombie from rotating
                    selfRigid.angularVelocity = 0f;
                }
            }
        }
        else
        {
            if (target.IsDestroyed())
            {
                // if the target is dead, forget about it
                target = null;
                targetPos = null;
                selfRigid.angularVelocity = angularVelocity;
                return;
            }

            // gets the Vector of the difference between the player and the enemy
            Vector2 difference = (Vector2) targetPos.position - selfRigid.position;

            // if its too far away, despawns
            if (difference.magnitude > despawnRadius)
            {
                Die();
            }

            // moves directly towards the player
            selfRigid.MovePosition(speed * difference.normalized * Time.deltaTime + selfRigid.position);
        }
    }

    // returns whether the body survives or not
    internal bool ChangeHealth(int quantity)
    {
        health += quantity;

        if (quantity > 0)
        {
            // increase health trigger

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else if (quantity < 0)
        {
            // lost health trigger (not implemented)

            if (health <= 0)
            {

                health = 0;
                Die();

                return false;
            }
        }

        return true;
    }

    // gets called when the enemy is due to die
    internal virtual void Die()
    {
        // deletes this object
        Destroy(gameObject);

        // removes this object from the parent's list
        parent.ZombieDeath(gameObject);
    }


    // checks for collision against enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            // get the enemy controller
            BodyController body = collision.gameObject.GetComponent<BodyController>();

            // apply damage to the enemy
            body.ChangeHealth(-contactDamage);

            // take damage from the body
            ChangeHealth(body.contactDamage);

            // get hit away from the enemy
            selfRigid.AddForce((selfRigid.position - (Vector2)targetPos.position).normalized * body.contactForce);
        }
    }
}

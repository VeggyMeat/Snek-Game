using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class NecromancerZombieController : MonoBehaviour
{
    private float speed;
    private float maxHealth;
    private int contactDamage;
    private int despawnRadius;
    private float angularVelocity;
    private int timeAlive;
    private float contactForce;

    private float health;

    internal Rigidbody2D selfRigid;
    internal Necro parent;

    private Dictionary<string, object> jsonVariables;

    private GameObject target = null;
    private EnemyController targetScript = null;
    private Transform targetPos = null;

    // Called just after creation, by whatever created the object
    internal void Setup(Dictionary<string, object> jsonData, Necro necro, float damageMultiplier)
    {
        parent = necro;

        jsonVariables = jsonData;

        JsonSetup();

        contactDamage = (int)(damageMultiplier * contactDamage);
    }

    void Start()
    {
        // sets up the rigid body
        selfRigid = GetComponent<Rigidbody2D>();

        health = maxHealth;

        // sets the body mildly rotating
        selfRigid.angularVelocity = angularVelocity;

        // kills the projectile in timeAlive seconds
        Invoke(nameof(Die), timeAlive);
    }

    void FixedUpdate()
    {
        if (target is null)
        {
            // shoots 1 ray out infront, if it hits it locks onto that enemy

            RaycastHit2D hit;

            // shoots a raycast infront of the zombie
            hit = Physics2D.Raycast(transform.position, transform.up);

            if (hit)
            {
                // grabs the enemy gameobject that was hit
                target = hit.collider.gameObject;
                
                // if it hits an enemy, targets it
                if (target.tag == "Enemy")
                {
                    // grabs the enemy's transform
                    targetPos = target.transform;

                    // grabs the enemy's script
                    targetScript = target.GetComponent<EnemyController>();

                    // stops the zombie from rotating
                    selfRigid.angularVelocity = 0f;
                }
                else
                {
                    target = null;
                }
            }
        }
        else
        {
            if (targetScript.Dead)
            {
                // if the target is dead, forget about it
                target = null;
                targetPos = null;
                targetScript = null;
                selfRigid.angularVelocity = angularVelocity;
                selfRigid.velocity = Vector3.zero;
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
    internal void Die()
    {
        // deletes this object
        Destroy(gameObject);

        // removes this object from the parent's list
        parent.ZombieDeath(gameObject);
    }


    // checks for collision against enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            // take damage from the body
            ChangeHealth(enemy.ContactDamage);

            // apply damage to the enemy
            if (!enemy.ChangeHealth(-contactDamage))
            {
                // killed an enemy
                parent.EnemyKilled(collision.gameObject);
            }

            if (!enemy.Dead)
            {
                // get hit away from the enemy
                selfRigid.AddForce((selfRigid.position - (Vector2)enemy.transform.position).normalized * contactForce);
            }
        }
    }

    private void JsonSetup()
    {
        // sets up the varibales
        jsonVariables.Setup(ref speed, nameof(speed));
        jsonVariables.Setup(ref maxHealth, nameof(maxHealth));
        jsonVariables.Setup(ref contactDamage, nameof(contactDamage));
        jsonVariables.Setup(ref despawnRadius, nameof(despawnRadius));
        jsonVariables.Setup(ref timeAlive, nameof(timeAlive));
        jsonVariables.Setup(ref angularVelocity, nameof(angularVelocity));
        jsonVariables.Setup(ref contactForce, nameof(contactForce));
    }
}

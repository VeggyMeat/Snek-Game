using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class NecromancerZombieController : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public int contactDamage;
    public int despawnRadius;
    public float angularVelocity;
    public int timeAlive;

    internal float health;
    internal Rigidbody2D selfRigid;
    internal Necro parent;

    private GameObject target = null;
    private EnemyController targetScript = null;
    private Transform targetPos = null;

    // Called just after creation, by whatever created the object
    internal void Setup(string jsonPath, Necro necro)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // sets the parent variable up
        parent = necro;
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
            if (targetScript.dead)
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
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // take damage from the body
            ChangeHealth(body.ContactDamage);

            // apply damage to the enemy
            if (!body.ChangeHealth(-contactDamage))
            {
                // killed an enemy
                parent.EnemyKilled(collision.gameObject);
            }

            if (!body.dead)
            {
                // get hit away from the enemy
                selfRigid.AddForce((selfRigid.position - (Vector2)body.transform.position).normalized * body.ContactDamage);
            }
        }
    }
}

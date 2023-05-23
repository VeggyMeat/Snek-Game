using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public float speed;
    public float angularVelocity;
    public int maxHealth;
    public int contactDamage;
    public int XPDrop;
    public int despawnRadius;
    public int contactForce;
    public bool walkTowards;

    internal int health;
    internal bool dead = false;
    internal Rigidbody2D selfRigid;
    internal EnemySummonerController summoner;
    internal TriggerController triggerController;

    internal EnemyPassiveHandler passiveHandler;

    private Transform player;

    internal virtual void Setup()
    {
        // gets a new passive handler
        passiveHandler = gameObject.AddComponent<EnemyPassiveHandler>();
        passiveHandler.Setup(this);

        // sets up the rigid body and the player location
        selfRigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>(); 

        // sets the spinning of the enemy
        selfRigid.angularVelocity = angularVelocity;

        // sets the health to the max health
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (walkTowards)
        {
            // gets the Vector of the difference between the player and the enemy
            Vector2 difference = (Vector2)player.position - selfRigid.position;

            // if its too far away, despawns
            if (difference.magnitude > despawnRadius)
            {
                Despawn();
            }

            // moves directly towards the player
            selfRigid.MovePosition((1 + passiveHandler.passiveValues["SpeedBuff"] / 100) * speed * difference.normalized * Time.deltaTime + selfRigid.position);
        }
    }

    // returns whether the body survives or not
    internal bool ChangeHealth(int quantity)
    {
        // if invulnerable just ignores damage
        if (passiveHandler.passiveValues["Invulnerability"] > 0)
        {
            return true;
        }

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
        // declares to other objects that this is dead
        dead = true;

        // increases the count of dead enemies for the summoner
        summoner.enemiesDead++;

        // makes a trigger to the trigger controller that this has died
        triggerController.enemyDied(gameObject);

        // deletes this object
        Destroy(gameObject);
    }

    // gets called when the enemy is despawned because of distance
    internal virtual void Despawn()
    {
        // declares to other objects that this is dead
        dead = true;

        // deletes this object
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks for collision against player
        if (collision.gameObject.tag == "Player")
        {
            // get the player controller
            BodyController body = collision.gameObject.GetComponent<BodyController>();

            // apply damage to the player
            body.ChangeHealth(-contactDamage);

            // take damage from the body
            ChangeHealth(body.contactDamage);

            // get hit away from the player
            selfRigid.AddForce((selfRigid.position - (Vector2)player.position).normalized * body.contactForce);
        }

    }
}

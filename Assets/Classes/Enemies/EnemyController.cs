using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    [SerializeField] private float speed;
    [SerializeField] private float angularVelocity;
    [SerializeField] private int maxHealth;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    [SerializeField] private int contactDamage;

    public int ContactDamage
    {
        get
        {
            return contactDamage;
        }
    }

    [SerializeField] private int xPDrop;

    public int XPDrop
    {
        get
        {
            return xPDrop;
        }
    }

    [SerializeField] private int despawnRadius;
    [SerializeField] private int contactForce;
    [SerializeField] private bool walkTowards;
    [SerializeField] private string enemyType;

    public string EnemyType
    {
        get
        {
            return enemyType;
        }
    }

    internal int health;
    internal bool dead = false;
    internal Rigidbody2D selfRigid;
    internal EnemySummonerController summoner;

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

        // calls the enemy spawn trigger
        TriggerManager.EnemySpawnTrigger.CallTrigger(gameObject);
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

    /// <summary>
    /// Change the health of the enemy by the quantity
    /// </summary>
    /// <param name="quantity">The value to change the health by</param>
    /// <returns>Whether the enemy survived or not (true / false)</returns>
    internal bool ChangeHealth(int quantity)
    {
        // if invulnerable just ignores damage
        if (passiveHandler.passiveValues["Invulnerability"] > 0)
        {
            return true;
        }

        // change the health by the quantity
        health += quantity;

        if (quantity > 0)
        {
            // if the enemy has too much health, set it back down to max
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else if (quantity < 0)
        {
            // if the health is less than 0, kill it
            if (health <= 0)
            {
                health = 0;
                Die();

                return false;
            }
        }

        // return saying that the enemy survived the hit
        return true;
    }

    // gets called when the enemy is due to die
    internal virtual void Die()
    {
        if (passiveHandler.passiveValues["ExtraLives"] > 0)
        {
            // if the enemy has extra lives, it just loses one
            passiveHandler.passiveValues["ExtraLives"]--;
            health = maxHealth;
            return;
        }

        // declares to other objects that this is dead
        dead = true;

        // increases the count of dead enemies for the summoner
        summoner.enemiesDead++;

        // makes a trigger to the trigger controller that this has died
        TriggerManager.EnemyDeadTrigger.CallTrigger(gameObject);

        // deletes this object
        Destroy(gameObject);
    }

    // gets called when the enemy is despawned because of distance
    internal virtual void Despawn()
    {
        // declares to other objects that this is dead
        dead = true;

        // tells the enemy controller that this has despawned
        summoner.EnemyDespawned(this);

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

            // apply damage to the player affected by the passive effects
            body.ChangeHealth(-contactDamage * (1 + passiveHandler.passiveValues["DamageBuff"] / 100));

            // take damage from the body
            ChangeHealth(-body.ContactDamage);

            // get hit away from the player
            selfRigid.AddForce((selfRigid.position - (Vector2)player.position).normalized * body.ContactForce);
        }

    }
}

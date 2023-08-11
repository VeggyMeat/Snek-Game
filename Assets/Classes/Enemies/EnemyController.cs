using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    [SerializeField] private float speed;

    public float Speed
    {
        get
        {
            return speedBuff.Value;
        }
    }

    [SerializeField] private float angularVelocity;
    [SerializeField] private int maxHealth;

    /// <summary>
    /// The maxHealth of the enemy
    /// </summary>
    public int MaxHealth
    {
        get
        {
            return (int)healthBuff.Value;
        }
    }

    [SerializeField] private int contactDamage;

    /// <summary>
    /// The damage dealt to a body when it touches the enemy
    /// </summary>
    public int ContactDamage
    {
        get
        {
            return contactDamage;
        }
    }

    [SerializeField] private int xPDrop;

    /// <summary>
    /// The amount of XP given to the player when killed
    /// </summary>
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

    /// <summary>
    /// The type of enemy (small, medium, large, special) that this enemy is (should be changed)
    /// </summary>
    public string EnemyType
    {
        get
        {
            return enemyType;
        }
    }

    private int health;

    /// <summary>
    /// The remaining health of the enemy
    /// </summary>
    public int Health
    {
        get
        {
            return health;
        }
    }

    private bool dead = false;

    /// <summary>
    /// Whether the enemy is dead or not
    /// </summary>
    public bool Dead
    {
        get
        {
            return dead;
        }
    }

    /// <summary>
    /// The RigidBody2D of the enemy
    /// </summary>
    internal Rigidbody2D selfRigid;

    /// <summary>
    /// The EnemySummonerController that spawned this enemy
    /// </summary>
    internal EnemySummonerController summoner;

    /// <summary>
    /// The body the enemy is targetting
    /// </summary>
    private Transform player;

    private int id;

    /// <summary>
    /// The unique ID of the enemy
    /// </summary>
    public int ID
    {
        get
        {
            return id;
        }
    }

    private int extraLives = 0;

    /// <summary>
    /// The extra lives on the enemy
    /// </summary>
    private int ExtraLives
    {
        get
        {
            return extraLives;
        }
    }

    /// <summary>
    /// The buff controlling whether the enemy is invulnerable or more (any addition will make it invulnerable)
    /// </summary>
    internal Buff invulnerabilityBuff;

    /// <summary>
    /// The buff controlling the movement speed of the enemy
    /// </summary>
    internal Buff speedBuff;

    /// <summary>
    /// The buff controlling the damage of the enemy
    /// </summary>
    internal Buff damageBuff;
    
    /// <summary>
    /// The buff controlling the health of the enemy
    /// </summary>
    internal Buff healthBuff;

    /// <summary>
    /// List of actions that are called on the death of the enemy
    /// </summary>
    internal List<Action> onDeath = new List<Action>();

    /// <summary>
    /// Whether the enemy is invulnerable or not
    /// </summary>
    public bool Invulnerable
    {
        get
        {
            return invulnerabilityBuff.Value > 1.1;
        }
    }

    /// <summary>
    /// Sets up the enemy script
    /// </summary>
    /// <param name="summoner">The summoner that spawned this enemy</param>
    /// <param name="id">The unique id for this enemy</param>
    internal virtual void Setup(EnemySummonerController summoner, int id)
    {
        // sets up the summoner
        this.summoner = summoner;

        // sets up the id of the enemy
        this.id = id;

        // sets up the rigid body and the player location
        selfRigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>(); 

        // sets the spinning of the enemy
        selfRigid.angularVelocity = angularVelocity;

        // sets the health to the max health
        health = maxHealth;

        // calls the enemy spawn trigger
        TriggerManager.EnemySpawnTrigger.CallTrigger(gameObject);

        // sets up the buffs
        healthBuff = gameObject.AddComponent<Buff>();
        healthBuff.Setup(HealthBuffUpdate, maxHealth);

        speedBuff = gameObject.AddComponent<Buff>();
        speedBuff.Setup(null, speed);

        damageBuff = gameObject.AddComponent<Buff>();
        damageBuff.Setup(null, 1f);

        invulnerabilityBuff = gameObject.AddComponent<Buff>();
        invulnerabilityBuff.Setup(null, 1f);
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
            selfRigid.MovePosition(Speed * Time.deltaTime * difference.normalized + selfRigid.position);
        }
    }

    /// <summary>
    /// Change the health of the enemy by the quantity
    /// </summary>
    /// <param name="quantity">The value to change the health by</param>
    /// <returns>Whether the enemy survived or not (true / false)</returns>
    internal bool ChangeHealth(int quantity)
    {
        // if this is dealing damage
        if (quantity < 0)
        {
            // if the enemy is invulnerable
            if (Invulnerable)
            {
                // ignore the damage and return that it survived
                return true;
            }
        }

        // change the health by the quantity
        health += quantity;

        // return the response from the health check
        return HealthChangeCheck();
    }

    /// <summary>
    /// Called to kill the enemy
    /// </summary>
    internal virtual void Die()
    {
        // declares to other objects that this is dead
        dead = true;

        // increases the count of dead enemies for the summoner
        summoner.enemiesDead++;

        // makes a trigger to the trigger controller that this has died
        TriggerManager.EnemyDeadTrigger.CallTrigger(gameObject);

        // deletes this object
        Destroy(gameObject);
    }

    /// <summary>
    /// Called to despawn the enemy when its out of the player range
    /// </summary>
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
            body.ChangeHealth(-contactDamage);

            // take damage from the body, if this dies give the kill to the body
            if (!ChangeHealth(-body.ContactDamage))
            {
                // gives credit to the first class in the list
                body.classes[0].EnemyKilled(gameObject);
            }
            
            // get hit away from the player
            selfRigid.AddForce((selfRigid.position - (Vector2)player.position).normalized * body.ContactForce);
        }

    }

    /// <summary>
    /// Checks stuff about the enemy health after the health has been changed
    /// </summary>
    /// <returns>Whether the enemy survived</returns>
    private bool HealthChangeCheck()
    {
        // if it has too much health set it down
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
            Die();

            // return saying it did not survive
            return false;
        }

        // return saying that the enemy survived the hit
        return true;
    }

    /// <summary>
    /// Called when the healthBuff is changed
    /// </summary>
    /// <param name="amount">the value of the change by the health buff</param>
    /// <param name="multiplicative">Whether its a value added to max health (false) or a scaler (true)</param>
    private void HealthBuffUpdate(float amount, bool multiplicative)
    {
        // if its a multiplying one, multiply the health by that much
        if (multiplicative)
        {
            health = (int)(health * amount);
        }
        // if its an additive one, increase (/decrease) the health by the amount
        else
        {
            health += (int)amount;
        }

        // call a health change check to see if body has died
        HealthChangeCheck();
    }
}

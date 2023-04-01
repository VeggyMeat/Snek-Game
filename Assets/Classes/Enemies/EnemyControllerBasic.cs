using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBasic : MonoBehaviour
{
    public float speed = 2f;
    public float angularVelocity = 30f;
    public int maxHealth = 100;
    public int contactDamage = 5;
    public int XPDrop = 10;
    public int despawnRadius = 30;
    
    internal int health;
    internal Rigidbody2D selfRigid;
    internal EnemySummonerController summoner;

    private Transform player;

    void Start()
    {
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
        // gets the Vector of the difference between the player and the enemy
        Vector2 difference = (Vector2)player.position - selfRigid.position;

        // if its too far away, despawns
        if (difference.magnitude > despawnRadius)
        {
            Despawn();
        }

        // moves directly towards the player
        selfRigid.MovePosition(speed * difference.normalized * Time.deltaTime + selfRigid.position);
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
        // increases the count of dead enemies for the summoner
        summoner.enemiesDead++;

        // deletes this object
        Destroy(gameObject);
    }

    // gets called when the enemy is despawned because of distance
    internal virtual void Despawn()
    {
        // deletes this object
        Destroy(gameObject);
    }

    // checks for collision against player
    private void OnTriggerEnter2D(Collider2D collision)
    {
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

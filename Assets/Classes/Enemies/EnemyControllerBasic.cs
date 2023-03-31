using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBasic : MonoBehaviour
{
    public float speed = 2f;
    public float angularVelocity = 30f;
    public int maxHealth = 100;
    public int contactDamage = 5;
    
    internal int health;
    internal Rigidbody2D selfRigid;

    private Transform player;

    void Start()
    {
        selfRigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Transform>();

        selfRigid.angularVelocity = angularVelocity;

        health = maxHealth;
    }

    void FixedUpdate()
    {
        Vector2 difference = (Vector2)player.position - selfRigid.position;

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
            // lost health trigger

            if (health < 0)
            {
                // death trigger

                health = 0;
                Destroy(gameObject);

                return false;
            }
        }

        return true;
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

            Destroy(gameObject);
        }
    }
}

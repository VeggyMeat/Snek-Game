using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    internal Archer archer;
    internal Rigidbody2D selfRigid;

    internal Vector2 movement;
    internal float lifeSpan;
    internal int damage;

    private float timeAlive = 0f;

    void Start()
    {
        // gets the rigid body and sets the velocity of the projectile
        selfRigid = gameObject.GetComponent<Rigidbody2D>();
        selfRigid.velocity = movement;
    }

    void Update()
    {
        // checks if the projectile has been alive longer than its lifespan, destroying it if it has
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeSpan)
        {
            Die();
        }
    }

    // called when the projectile dies
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    // triggers when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyControllerBasic body = collision.gameObject.GetComponent<EnemyControllerBasic>();

            // apply damage to the enemy
            if (body.ChangeHealth(-damage))
            {
                // enemy has been killed
                archer.EnemyKilled(collision.gameObject);
            }

            // destroy the projectile
            Destroy(gameObject);
        }
    }
}

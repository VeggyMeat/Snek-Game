using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    internal Archer archer;
    internal Rigidbody2D selfRigid;

    private Vector2 movement;
    private float lifeSpan;
    private int damage;

    internal void Setup(Vector2 movement, float lifeSpan, int damage, Archer archer)
    {
        // sets the variables given by the archer
        this.movement = movement;
        this.lifeSpan = lifeSpan;
        this.damage = damage;
        this.archer = archer;

        // gets the rigid body and sets the velocity of the projectile
        selfRigid = gameObject.GetComponent<Rigidbody2D>();
        selfRigid.velocity = movement;

        // kills the projectile in lifeSpan seconds
        Invoke(nameof(Die), lifeSpan);
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
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

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

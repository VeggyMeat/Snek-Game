using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    internal Rigidbody2D selfRigid;
    internal Vector2 movement;
    internal float lifeSpan;
    internal int damage;

    private float timeAlive = 0f;

    void Start()
    {
        selfRigid = gameObject.GetComponent<Rigidbody2D>();
        selfRigid.velocity = movement;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeSpan)
        {
            Destroy(gameObject);
        }
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
            body.ChangeHealth(-damage);

            // destroy the projectile
            Destroy(gameObject);
        }
    }
}

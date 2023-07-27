using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletController : MonoBehaviour
{
    private TurretController parent;

    private Dictionary<string, object> variables;

    private float velocity;
    private float lifeSpan;
    private int damage;

    internal void Setup(Dictionary<string, object> variables, TurretController parent, float DamageMultiplier)
    {
        // sets the engineer as the owner
        this.parent = parent;

        // loads in all the variables from the json
        this.variables = variables;
        LoadVariables();

        // kills the projectile when it should die
        Invoke(nameof(Die), lifeSpan);

        float angle = transform.rotation.eulerAngles.z;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle) * velocity, Mathf.Sin(angle) * velocity);

        // updates the damage based upon the damage multiplier of the parent's parent (the engineer)
        damage = (int)(damage * DamageMultiplier);
    }

    // triggers when the projectile collides with something
    internal virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // if the projectile collides with a body
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController body = collision.gameObject.GetComponent<EnemyController>();

            // apply damage to the enemy
            if (!body.ChangeHealth(-damage))
            {
                // enemy has been killed
                parent.EnemyKilled(collision.gameObject);
            }

            // kills the projectile
            Die();
            return;
        }
    }

    // called when the bullet dies
    internal void Die()
    {
        Destroy(gameObject);
    }

    internal void LoadVariables()
    {
        variables.Setup(ref velocity, "velocity");
        variables.Setup(ref lifeSpan, "lifeSpan");
        variables.Setup(ref damage, "damage");
    }
}

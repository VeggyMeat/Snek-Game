using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private TurretController parent;

    public float velocity;
    public float lifeSpan;
    public int damage;

    internal void Setup(string jsonPath, TurretController parent, float DamageMultiplier)
    {
        // sets the engineer as the owner
        this.parent = parent;

        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

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
}

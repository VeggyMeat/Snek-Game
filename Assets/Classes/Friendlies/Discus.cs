using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discus : MonoBehaviour
{
    private JsonVariable jsonVariables;

    private int contactDamage;
    private float contactForce;

    private DiscusMan parent;

    internal void Setup(ref JsonVariable variables, DiscusMan parent)
    {
        // sets the variables up
        jsonVariables = variables;
        LoadVariables();

        // sets the parent up
        this.parent = parent;

        // adds this to the list of discuses
        parent.discuses.Add(this);
    }

    /// <summary>
    /// Called when the Discus should die
    /// </summary>
    internal void Die()
    {
        // removes it from the DiscusMan's list
        parent.discuses.Remove(this);

        // destroys the gameObject
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if the collision is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // gets the enemy's EnemyController
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();

            // damages the enemy, if it dies, gives the kill to the discusMan
            if (!enemyController.ChangeHealth((int)(-contactDamage * parent.body.damageBuff.Value)))
            {
                parent.EnemyKilled(collision.gameObject);
            }

            // applies a force to the enemy
            enemyController.selfRigid.AddForce((enemyController.transform.position - transform.position).normalized * contactForce);
        }
    }

    internal void LoadVariables()
    {
        jsonVariables.Variables.Setup(ref contactDamage, "contactDamage");
        jsonVariables.Variables.Setup(ref contactForce, "contactForce");
    }
}

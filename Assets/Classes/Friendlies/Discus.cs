using UnityEngine;

// COMPLETE

/// <summary>
/// The discus controller that is on the discus game objects
/// </summary>
public class Discus : MonoBehaviour
{
    /// <summary>
    /// The JsonVariable that holds the variables for the discus
    /// </summary>
    private JsonVariable jsonVariables;

    /// <summary>
    /// The damage the discus does on contact
    /// </summary>
    private int contactDamage;

    /// <summary>
    /// The force the discus applies on contact
    /// </summary>
    private float contactForce;

    /// <summary>
    /// The parent DiscusMan that manages the discus
    /// </summary>
    private DiscusMan parent;

    /// <summary>
    /// Called when the discus is created by discusMan
    /// </summary>
    /// <param name="variables">The variables for the discus</param>
    /// <param name="parent">The discus man that threw this discus</param>
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
    /// Called when DiscusMan levels up
    /// </summary>
    internal void OnLevelUp()
    {
        jsonVariables.IncreaseIndex();
        LoadVariables();
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

    // Called by Unity when the discus collides with something
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

    /// <summary>
    /// Loads the variables from the JsonVariable
    /// </summary>
    private void LoadVariables()
    {
        jsonVariables.Variables.Setup(ref contactDamage, "contactDamage");
        jsonVariables.Variables.Setup(ref contactForce, "contactForce");
    }
}

using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The controller for the necromancer class's zombies
/// </summary>
public class NecromancerZombieController : MonoBehaviour
{
    /// <summary>
    /// The speed of the zombie body
    /// </summary>
    private float speed;

    /// <summary>
    /// The maximum health of the zombie body
    /// </summary>
    private float maxHealth;

    /// <summary>
    /// The damage the zombie body does on contact
    /// </summary>
    private int contactDamage;

    /// <summary>
    /// The radius at which the zombie body despawns away from the player
    /// </summary>
    private int despawnRadius;

    /// <summary>
    /// The time the zombie body is alive for
    /// </summary>
    private int timeAlive;

    /// <summary>
    /// The force at which the zombie body is pushed away from the enemy after contact
    /// </summary>
    private float contactForce;

    /// <summary>
    /// The current health of the zombie body
    /// </summary>
    private float health;

    /// <summary>
    /// The rigid body of the zombie body
    /// </summary>
    private Rigidbody2D selfRigid;

    /// <summary>
    /// The parent necromancer that created this zombie
    /// </summary>
    private Necro parent;

    /// <summary>
    /// The variables from the json file
    /// </summary>
    private Dictionary<string, object> jsonVariables;

    /// <summary>
    /// The target that the zombie is currently locked onto
    /// </summary>
    private GameObject target = null;

    /// <summary>
    /// The enemy controller of the target
    /// </summary>
    private EnemyController targetScript = null;

    /// <summary>
    /// Called just after creation, by whatever created the object
    /// </summary>
    /// <param name="jsonData">The jsonData for the zombie</param>
    /// <param name="necro">The necromancer object that spawned this</param>
    /// <param name="damageMultiplier">The damageMultiplier for the zombie's contact damage</param>
    internal void Setup(Dictionary<string, object> jsonData, Necro necro, float damageMultiplier)
    {
        parent = necro;
        jsonVariables = jsonData;

        JsonSetup();

        contactDamage = (int)(damageMultiplier * contactDamage);
    }

    // Called by unity before the first frame
    void Start()
    {
        // sets up the rigid body
        selfRigid = GetComponent<Rigidbody2D>();

        health = maxHealth;

        // kills the projectile in timeAlive seconds
        Invoke(nameof(Die), timeAlive);
    }

    // Called by unity every frame before doing any physics calculations
    void FixedUpdate()
    {
        if (target is null)
        {
            // shoots 1 ray out infront, if it hits it locks onto that enemy

            RaycastHit2D hit;

            // shoots a raycast infront of the zombie
            hit = Physics2D.Raycast(transform.position, transform.up);

            if (hit)
            {
                // grabs the enemy gameobject that was hit
                target = hit.collider.gameObject;
                
                // if it hits an enemy, targets it
                if (target.tag == "Enemy")
                {
                    // grabs the enemy's script
                    targetScript = target.GetComponent<EnemyController>();

                    // stops the zombie from rotating
                    selfRigid.angularVelocity = 0f;
                }
                else
                {
                    // if it hits anything else, forgets about it
                    target = null;
                }
            }
        }
        else
        {
            if (targetScript.Dead)
            {
                // if the target is dead, forget about it
                target = null;
                targetScript = null;
                selfRigid.velocity = Vector3.zero;
                return;
            }

            // gets the Vector of the difference between the player and the enemy
            Vector2 difference = (Vector2) targetScript.transform.position - selfRigid.position;

            // if its too far away, despawns
            if (difference.magnitude > despawnRadius)
            {
                Die();
            }

            // moves directly towards the player
            selfRigid.MovePosition(speed * difference.normalized * Time.deltaTime + selfRigid.position);
        }
    }

    /// <summary>
    /// Called to change the health of the zombie
    /// </summary>
    /// <param name="quantity">The quantity to change the health by</param>
    /// <returns>Whether the body is still alive (true) or is dead (false)</returns>
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

    /// <summary>
    /// Gets called when the enemy is due to die
    /// </summary>
    internal void Die()
    {
        // deletes this object
        Destroy(gameObject);

        // removes this object from the parent's list
        parent.ZombieDeath();
    }


    // Called by unity when the object collides with another object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // only does something if it hits an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // get the enemy controller
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            // take damage from the body
            ChangeHealth(enemy.ContactDamage);

            // apply damage to the enemy
            if (!enemy.ChangeHealth(-contactDamage))
            {
                // killed an enemy
                parent.EnemyKilled(collision.gameObject);
            }

            if (!enemy.Dead)
            {
                // get hit away from the enemy
                selfRigid.AddForce((selfRigid.position - (Vector2)enemy.transform.position).normalized * contactForce);
            }
        }
    }

    /// <summary>
    /// Sets up the values from the json data
    /// </summary>
    private void JsonSetup()
    {
        // sets up the varibales
        jsonVariables.Setup(ref speed, nameof(speed));
        jsonVariables.Setup(ref maxHealth, nameof(maxHealth));
        jsonVariables.Setup(ref contactDamage, nameof(contactDamage));
        jsonVariables.Setup(ref despawnRadius, nameof(despawnRadius));
        jsonVariables.Setup(ref timeAlive, nameof(timeAlive));
        jsonVariables.Setup(ref contactForce, nameof(contactForce));
    }
}

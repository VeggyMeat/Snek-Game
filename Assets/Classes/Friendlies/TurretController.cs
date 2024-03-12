using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The controller for the turret game objects
/// </summary>
public class TurretController : MonoBehaviour
{
    /// <summary>
    /// The parent engineer that created this turret
    /// </summary>
    private Engineer parent;

    /// <summary>
    /// The lifespan of the turret
    /// </summary>
    private float lifeSpan;

    /// <summary>
    /// The path to the bullet json file
    /// </summary>
    private string bulletJson;

    /// <summary>
    /// The path to the bullet prefab
    /// </summary>
    private string bulletPath;

    /// <summary>
    /// The angular velocity of the turret
    /// </summary>
    private float angularVelocity;

    /// <summary>
    /// The time delay between the turret attacking
    /// </summary>
    private float timeDelay;

    /// <summary>
    /// The prefab of the bullet game object
    /// </summary>
    private GameObject bulletTemplate;

    /// <summary>
    /// The data for the turret
    /// </summary>
    private Dictionary<string, object> variables;

    /// <summary>
    /// The variables for the bullet
    /// </summary>
    private JsonVariable bulletVariables;

    /// <summary>
    /// Called by the parent engineer to set up the turret
    /// </summary>
    /// <param name="variables">The data for the turret</param>
    /// <param name="parent">The engineer that created it</param>
    internal void Setup(Dictionary<string, object> variables, Engineer parent)
    {
        this.parent = parent;
        this.variables = variables;
        LoadVariables();

        // loads the bullet in
        bulletTemplate = Resources.Load<GameObject>(bulletPath);

        // makes the turret spin
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;

        // loads the bullet's json file in
        bulletVariables = new JsonVariable(bulletJson, parent.body.Level - 1);

        // starts firing bullets regularly
        InvokeRepeating(nameof(FireBullet), timeDelay, timeDelay);

        // kills the turret in lifeSpan seconds
        Invoke(nameof(Die), lifeSpan);
    }

    /// <summary>
    /// Fires a bullet in a random direction
    /// </summary>
    private void FireBullet()
    {
        // gets a random angle
        float angle = Random.value * 360;

        // creates the new bullet
        GameObject bullet = Instantiate(bulletTemplate, transform.position, Quaternion.Euler(0, 0, angle));

        // grabs the bullet controller
        BulletController controller = bullet.GetComponent<BulletController>();

        // sets up the bullet
        // gets the variable information based on the bullet json corresponding to the body's level
        controller.Setup(bulletVariables.Variables, this, parent.body.DamageMultiplier);
    }

    /// <summary>
    /// Called when a bullet kills an enemy
    /// </summary>
    /// <param name="enemy"></param>
    internal void EnemyKilled(GameObject enemy)
    {
        parent.EnemyKilled(enemy);
    }

    /// <summary>
    /// Destroys the turret
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Loads in the variables from those given by the parent object
    /// </summary>
    private void LoadVariables()
    {
        variables.Setup(ref lifeSpan, "lifeSpan");
        variables.Setup(ref bulletJson, "bulletJson");
        variables.Setup(ref bulletPath, "bulletPath");
        variables.Setup(ref angularVelocity, "angularVelocity");
        variables.Setup(ref timeDelay, "timeDelay");
    }
}

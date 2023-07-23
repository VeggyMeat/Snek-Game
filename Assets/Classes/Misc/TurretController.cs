using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Engineer parent;

    private float lifeSpan;

    private string bulletJson;
    private string bulletPath;

    private float angularVelocity;

    private float timeDelay;

    private GameObject bulletTemplate;

    private Dictionary<string, object> variables;

    private JsonVariable bulletVariables;

    internal void Setup(Dictionary<string, object> variables, Engineer parent)
    {
        // sets the engineer as the owner
        this.parent = parent;

        // loads in all the variables from the json
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

    internal void FireBullet()
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

    // called when a bullet kills an enemy
    internal void EnemyKilled(GameObject enemy)
    {
        parent.EnemyKilled(enemy);
    }

    // kills the gameObject
    internal void Die()
    {
        Destroy(gameObject);
    }

    // loads in the variables from those given by the parent object
    internal void LoadVariables()
    {
        foreach(string item in variables.Keys)
        {
            switch (item)
            {
                case "lifeSpan":
                    lifeSpan = float.Parse(variables[item].ToString());
                    break;
                case "bulletJson":
                    bulletJson = variables[item].ToString();
                    break;
                case "bulletPath":
                    bulletPath = variables[item].ToString();
                    break;
                case "angularVelocity":
                    angularVelocity = float.Parse(variables[item].ToString());
                    break;
                case "timeDelay":
                    timeDelay = float.Parse(variables[item].ToString());
                    break;
            }
        }
    }
}

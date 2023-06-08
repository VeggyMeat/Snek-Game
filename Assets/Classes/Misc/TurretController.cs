using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private Engineer parent;

    public float lifeSpan;

    public string bulletJson;
    public string bulletPath;

    public float angularVelocity;

    public float timeDelay;

    private GameObject bulletTemplate;

    internal void Setup(string jsonPath, Engineer parent)
    {
        // sets the engineer as the owner
        this.parent = parent;

        // loads in all the variables from the json
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);

        // loads the bullet in
        bulletTemplate = Resources.Load<GameObject>(bulletPath);

        // makes the turret spin
        GetComponent<Rigidbody2D>().angularVelocity = angularVelocity;

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
        controller.Setup(bulletJson, this, parent.DamageMultiplier);
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
}

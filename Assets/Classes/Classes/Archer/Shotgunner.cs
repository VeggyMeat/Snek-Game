using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shotgunner : Archer
{
    private int projectileCount;
    private float spreadAngle;
    private float range;

    private string projectilePath;
    private string projectileJson;

    private JsonVariable arrowVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/Shotgunner.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // gets the json data and loads it into the arrowVariables
        arrowVariables = new JsonVariable(projectileJson);

        // grabs the projectile from resources
        projectile = Resources.Load<GameObject>(projectilePath);

        // calls the archer's setup
        base.Setup();
    }

    internal override void LaunchProjectile()
    {
        // gets all the enemies in the range
        Collider2D[] objectsInCircle = Physics2D.OverlapCircleAll(transform.position, range);

        // gets all of the enemies within the range
        Collider2D[] enemiesInCircle = System.Array.FindAll(objectsInCircle, obj => obj.CompareTag("Enemy"));

        // returns if there are no enemies in the range
        if (enemiesInCircle.Length == 0)
        {
            return;
        }

        // get a position of a random enemy
        Vector3 enemyPos = enemiesInCircle[Random.Range(0, enemiesInCircle.Length)].transform.position;

        // gets the angle to the enemy
        float angle = ((Vector2)transform.position).AngleTo((Vector2)enemyPos);

        // repeats this for every projectile to be shot
        for (int i = 0; i < projectileCount; i++)
        {
            // the spread angle that gets added
            float addedAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);

            // shoots the projectile
            Projectile.Shoot(projectile, transform.position, angle + addedAngle, arrowVariables.Variables, this, body.DamageMultiplier);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref projectileCount, nameof(projectileCount));
        jsonData.Setup(ref spreadAngle, nameof(spreadAngle));
        jsonData.Setup(ref range, nameof(range));
        jsonData.Setup(ref projectilePath, "projectilePath");
        jsonData.Setup(ref projectileJson, "projectileJson");
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            arrowVariables.IncreaseIndex();
        }
    }
}

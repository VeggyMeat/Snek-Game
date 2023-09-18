using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArcherArcher : Archer
{
    private string projectilePath;
    private string projectileJson;

    private JsonVariable arrowVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Archer/BowMan.json";

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

    // called regularly by archer
    internal override void LaunchProjectile()
    {
        if (body.IsDead)
        {
            return;
        }

        // creates and sets up a new projectile
        Projectile.Shoot(projectile, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), arrowVariables.Variables, this, body.DamageMultiplier);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

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

    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        // tells the mage to launch the orbs
        ((MagicArcherMage)body.classes[1]).LaunchOrbs(enemy);
    }
}

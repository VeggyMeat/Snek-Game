using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusHelmet : Item
{
    private int projectiles;
    private string projectileJson;
    private string projectilePath;

    private JsonVariable projectileVariables;
    private GameObject projectileObject;

    private int projectileHits = 0;
    private int projectileHitsLevelUp;

    private int id;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/CactusHelmet.json";

        base.Setup();

        // sets up the projectileVariables
        projectileVariables = new JsonVariable(projectileJson);

        // gets the gameObject for the projectile
        projectileObject = Resources.Load<GameObject>(projectilePath);

        // adds the OnHit trigger
        TriggerManager.BodyLostHealthTrigger.AddTrigger(OnHit);
        TriggerManager.ProjectileHitTrigger.AddTrigger(OnProjectileHit); ;

        id = int.Parse(projectileVariables.Variables["id"].ToString());
    }

    private (BodyController, int) OnHit((BodyController, int) info)
    {
        BodyController bodyController = info.Item1;

        // launch 'projectiles' projectiles
        float angle = Random.Range(0, Mathf.PI * 2);
        float dif = Mathf.PI * 2 / projectiles;

        for (int i = 0; i < projectiles; i++)
        {
            // shoots a projectile
            Projectile.Shoot(projectileObject, bodyController.transform.position, angle, projectileVariables.Variables, bodyController.classes[0], bodyController.DamageMultiplier);

            // increases the angle
            angle += dif;
        }

        return info;
    }

    private GameObject OnProjectileHit(GameObject projectileObject)
    {
        // gets the projectileController
        ProjectileController projectileController = projectileObject.GetComponent<ProjectileController>();

        // if the projectile is from the cactus helmet
        if (projectileController.ID == id)
        {
            projectileHits++;

            if (projectileHits >= projectileHitsLevelUp && Levelable)
            {
                LevelUp();
            }
        }

        return projectileObject;
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            projectileHits -= projectileHitsLevelUp;
        }

        base.LevelUp();

        if (level != 1)
        {
            // levels up the projectile variables
            projectileVariables.IncreaseIndex();
        }
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref projectiles, nameof(projectiles));
        jsonVariables.Setup(ref projectileJson, nameof(projectileJson));
        jsonVariables.Setup(ref projectilePath, nameof(projectilePath));
        jsonVariables.Setup(ref projectileHitsLevelUp, nameof(projectileHitsLevelUp));
    }
}

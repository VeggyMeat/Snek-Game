using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigBullet : Item
{
    private float projectileSizeModifier;

    private float projectilesHitCount = 0;
    private float projectilesHitLevelUp;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/BigBullet.json";

        base.Setup();

        TriggerManager.ProjectileShotTrigger.AddTrigger(IncreaseProjectileSize);
        TriggerManager.ProjectileHitTrigger.AddTrigger(ProjectileHit);
    }

    private GameObject IncreaseProjectileSize(GameObject projectile)
    {
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        projectileController.Scale(projectileSizeModifier);

        return projectile;
    }

    private GameObject ProjectileHit(GameObject projectile)
    {
        projectilesHitCount++;

        if (projectilesHitCount >= projectilesHitLevelUp && Levelable)
        {
            LevelUp();
        }

        return projectile;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref projectileSizeModifier, nameof(projectileSizeModifier));
        jsonVariables.Setup(ref projectilesHitLevelUp, nameof(projectilesHitLevelUp));
    }

    protected override void LevelUp()
    {
        if (jsonLoaded)
        {
            projectilesHitCount -= projectilesHitLevelUp;
        }

        base.LevelUp();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigBullet : Item
{
    private float projectileSizeModifier;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/BigBullet.json";

        base.Setup();

        TriggerManager.ProjectileShotTrigger.AddTrigger(IncreaseProjectileSize);
    }

    private GameObject IncreaseProjectileSize(GameObject projectile)
    {
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        projectileController.Scale(projectileSizeModifier);

        return projectile;
    }

    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref projectileSizeModifier, nameof(projectileSizeModifier));
    }
}

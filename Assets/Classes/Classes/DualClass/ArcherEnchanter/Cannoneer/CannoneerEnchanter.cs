using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannoneerEnchanter : Enchanter
{
    private float scaleFactor;
    private float damageMultiplier;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/Cannoneer/CannoneerEnchanter.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        TriggerManager.ProjectileShotTrigger.AddTrigger(ModifyProjectile);
    }

    private GameObject ModifyProjectile(GameObject projectile)
    {
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        projectileController.Scale(scaleFactor);
        projectileController.Damage = (int)(projectileController.Damage * damageMultiplier);

        return projectile;
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        TriggerManager.ProjectileShotTrigger.RemoveTrigger(ModifyProjectile);
    }

    internal override void Revived()
    {
        base.Revived();

        TriggerManager.ProjectileShotTrigger.AddTrigger(ModifyProjectile);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref scaleFactor, nameof(scaleFactor));
        jsonData.Setup(ref damageMultiplier, nameof(damageMultiplier));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMageArcher : Archer
{
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MirrorMage/MirrorMageArcher.json";

        primary = false;

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // sets up the trigger
        TriggerManager.ProjectileShotTrigger.AddTrigger(OnProjectileShot);
    }

    internal override void LaunchProjectile()
    {
        Projectile.Shoot(projectile, transform.position, Random.Range(0, Mathf.PI * 2), projectileVariables.Variables, this, body.DamageMultiplier);
    }

    private GameObject OnProjectileShot(GameObject obj)
    {
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();

        if (projectileController.Owner.body.Name != body.Name)
        {
            LaunchProjectile();
        }

        return obj;
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the trigger
        TriggerManager.ProjectileShotTrigger.RemoveTrigger(OnProjectileShot);
    }

    internal override void Revived()
    {
        base.Revived();

        // adds the trigger back
        TriggerManager.ProjectileShotTrigger.AddTrigger(OnProjectileShot);
    }
}

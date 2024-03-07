using UnityEngine;

// COMPLETE

/// <summary>
/// The mirror mage archer class, a subclass of the archer class
/// </summary>
internal class MirrorMageArcher : Archer
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MirrorMage/MirrorMageArcher.json";

        // indicates that this is not the primary class of the object
        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // sets up the trigger
        TriggerManager.ProjectileShotTrigger.AddTrigger(OnProjectileShot);
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    internal override void LaunchProjectile()
    {
        Projectile.Shoot(projectile, transform.position, Random.Range(0, Mathf.PI * 2), projectileVariables.Variables, this, body.DamageMultiplier);
    }

    /// <summary>
    /// Called whenever a projectile is shot by any body
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private GameObject OnProjectileShot(GameObject obj)
    {
        // grabs the projectile controller
        ProjectileController projectileController = obj.GetComponent<ProjectileController>();

        // if the projectile was not shot by this body, then shoot a new projectile
        if (projectileController.Owner.body.Name != body.Name)
        {
            LaunchProjectile();
        }

        return obj;
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the trigger
        TriggerManager.ProjectileShotTrigger.RemoveTrigger(OnProjectileShot);
    }

    /// <summary>
    /// Called when the body is killed
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // adds the trigger back
        TriggerManager.ProjectileShotTrigger.AddTrigger(OnProjectileShot);
    }
}

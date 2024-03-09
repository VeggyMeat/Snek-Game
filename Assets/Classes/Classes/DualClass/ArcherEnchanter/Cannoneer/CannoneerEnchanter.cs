using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The cannoneer enchanter class, a subclass of the enchanter class
/// </summary>
internal class CannoneerEnchanter : Enchanter
{
    /// <summary>
    /// The scale factor for the projectiles
    /// </summary>
    private float scaleFactor;

    /// <summary>
    /// The damage multiplier for the projectiles
    /// </summary>
    private float damageMultiplier;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/Cannoneer/CannoneerEnchanter.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        TriggerManager.ProjectileShotTrigger.AddTrigger(ModifyProjectile);
    }

    /// <summary>
    /// Modifies a projectile with the given buffs of the cannoneer
    /// </summary>
    /// <param name="projectile">The projectile to buff</param>
    /// <returns>The projectile to buff</returns>
    private GameObject ModifyProjectile(GameObject projectile)
    {
        // grabs the projectile controller
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();

        // scales the projectile by the scaleFactor
        projectileController.Scale(scaleFactor);

        // multiplies the damage by the damageMultiplier
        projectileController.Damage = (int)(projectileController.Damage * damageMultiplier);

        return projectile;
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the trigger to buff the projectiles
        TriggerManager.ProjectileShotTrigger.RemoveTrigger(ModifyProjectile);
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // adds the trigger back
        TriggerManager.ProjectileShotTrigger.AddTrigger(ModifyProjectile);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref scaleFactor, nameof(scaleFactor));
        jsonData.Setup(ref damageMultiplier, nameof(damageMultiplier));
    }
}

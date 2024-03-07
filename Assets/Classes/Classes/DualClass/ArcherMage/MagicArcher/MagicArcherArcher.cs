using UnityEngine;

// COMPLETE

/// <summary>
/// The magic archer archer class, a subclass of the archer class
/// </summary>
internal class MagicArcherArcher : Archer
{
    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MagicArcher/MagicArcherArcher.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called regularly by the archer based on timeDelay
    /// </summary>
    internal override void LaunchProjectile()
    {
        // creates and sets up a new projectile
        Projectile.Shoot(projectile, transform.position, Random.Range(0, 2 * Mathf.PI), projectileVariables.Variables, this, body.DamageMultiplier);
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy"></param>
    internal override void EnemyKilled(GameObject enemy)
    {
        base.EnemyKilled(enemy);

        // tells the mage to launch the orbs
        ((MagicArcherMage)body.classes[1]).LaunchOrbs(enemy);
    }
}

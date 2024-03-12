using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The class that is added to all body game objects by the fairy with a gun class
/// </summary>
internal class Gun : MonoBehaviour
{
    /// <summary>
    /// The level of the gun
    /// </summary>
    private int level = 0;

    /// <summary>
    /// The level of the gun
    /// </summary>
    internal int Level
    {
        get
        {
            return level;
        }
    }

    /// <summary>
    /// The data from the json for the gun
    /// </summary>
    private List<Dictionary<string, object>> jsonData;

    /// <summary>
    /// The class the gun is attatched to
    /// </summary>
    private Class attatched;

    /// <summary>
    /// The time delay between each bullet
    /// </summary>
    private float timeDelay;

    /// <summary>
    /// Whether the json has been loaded or not
    /// </summary>
    private bool jsonLoaded = false;

    /// <summary>
    /// The prefab for the bullet game object
    /// </summary>
    private GameObject bulletPrefab;

    /// <summary>
    /// The path to the bullet prefab
    /// </summary>
    private string bulletPath;

    /// <summary>
    /// The path to the bullet json
    /// </summary>
    private string bulletJson;

    /// <summary>
    /// The variables for the bullet
    /// </summary>
    private JsonVariable bulletVariables;

    /// <summary>
    /// The damage multiplication for the gun
    /// </summary>
    private float damageMultiplication;

    /// <summary>
    /// Called by fairy with a gun on creation
    /// </summary>
    /// <param name="attatched">The class the gun is attatched to</param>
    /// <param name="damageMultiplication">The damage multiplication for the gun</param>
    /// <param name="variables">The data from the json for the gun</param>
    internal void Setup(Class attatched, float damageMultiplication, List<Dictionary<string, object>> variables)
    {
        // matches the jsonData to the variables
        jsonData = variables;

        // Levels the gun up to level 1
        UpgradeGun();

        StartFiringProjectiles();

        // loads in the bullet
        bulletPrefab = Resources.Load<GameObject>(bulletPath);

        // loads the bullet variables in
        bulletVariables = new JsonVariable(bulletJson);

        // attatches the gun to the class
        this.attatched = attatched;

        // sets the damageMultiplication
        this.damageMultiplication = damageMultiplication;
    }

    /// <summary>
    /// Upgrades the gun
    /// </summary>
    internal void UpgradeGun()
    {
        // increases the level
        level++;

        // loads in the data from the json into the variables
        LoadVariables();

        if (level > 1)
        {
            bulletVariables.IncreaseIndex();
        }
    }

    /// <summary>
    /// Loads in the variables for the gun from the variables data
    /// </summary>
    private void LoadVariables()
    {
        // gets the next set of data for the gun
        Dictionary<string, object> data = jsonData[level - 1];

        data.Setup(ref bulletJson, "bulletJson");
        data.Setup(ref bulletPath, "bulletPath");
        data.SetupAction(ref timeDelay, nameof(timeDelay), StopFiringProjectiles, StartFiringProjectiles, jsonLoaded);

        if (!jsonLoaded)
        {
            jsonLoaded = true;
        }
    }

    /// <summary>
    /// Fires a projectile from the gun
    /// </summary>
    private void FireProjectile()
    {
        Projectile.Shoot(bulletPrefab, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), bulletVariables.Variables, attatched, damageMultiplication * attatched.body.DamageMultiplier);
    }

    /// <summary>
    /// Starts firing projectiles repeatedly
    /// </summary>
    private void StartFiringProjectiles()
    {
        InvokeRepeating(nameof(FireProjectile), timeDelay, timeDelay);
    }

    /// <summary>
    /// Stops firing projectiles
    /// </summary>
    private void StopFiringProjectiles()
    {
        CancelInvoke(nameof(FireProjectile));
    }
}

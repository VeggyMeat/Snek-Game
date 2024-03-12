using UnityEngine;

// COMPLETE

/// <summary>
/// The cactus helmet item
/// </summary>
internal class CactusHelmet : Item
{
    /// <summary>
    /// The number of projectiles to fire
    /// </summary>
    private int projectiles;

    /// <summary>
    /// The path to the json file for the projectile
    /// </summary>
    private string projectileJson;

    /// <summary>
    /// The path to the prefab for the projectile
    /// </summary>
    private string projectilePath;

    /// <summary>
    /// The variables for the projectile that is being shot
    /// </summary>
    private JsonVariable projectileVariables;

    /// <summary>
    /// The prefab for the projectile
    /// </summary>
    private GameObject projectileObject;

    /// <summary>
    /// The number of catcus helmet projectiles that have hit enemies
    /// </summary>
    private int projectileHits = 0;

    /// <summary>
    /// The number of cactus helmet projectiles that need to hit enemies to level up
    /// </summary>
    private int projectileHitsLevelUp;

    /// <summary>
    /// The id of the projectile's shot
    /// </summary>
    private int id;

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    internal override void Setup(IGameSetup gameSetup)
    {
        jsonPath = "Assets/Resources/Jsons/Items/CactusHelmet.json";

        base.Setup(gameSetup);

        // sets up the projectileVariables
        projectileVariables = new JsonVariable(projectileJson);

        // gets the gameObject for the projectile
        projectileObject = Resources.Load<GameObject>(projectilePath);

        // adds the OnHit trigger
        TriggerManager.BodyLostHealthTrigger.AddTrigger(OnHit);
        TriggerManager.ProjectileHitTrigger.AddTrigger(OnProjectileHit); ;

        // grabs the id from the projectiles' variables
        id = int.Parse(projectileVariables.Variables["id"].ToString());
    }

    /// <summary>
    /// Called when an object loses health,
    /// Releases projectiles
    /// </summary>
    /// <param name="info">The body that was hit and the damage it took</param>
    /// <returns>The body that was hit and the damage it took</returns>
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

    /// <summary>
    /// Called when a projectile hits an enemy
    /// </summary>
    /// <param name="projectileObject">The projectile that hit an enemy</param>
    /// <returns>The projectile that hit an enemy</returns>
    private GameObject OnProjectileHit(GameObject projectileObject)
    {
        // gets the projectileController
        ProjectileController projectileController = projectileObject.GetComponent<ProjectileController>();

        // if the projectile is from the cactus helmet
        if (projectileController.ID == id)
        {
            projectileHits++;

            // levels up the item if it can be leveled up and the number of projectiles hit is enough
            if (projectileHits >= projectileHitsLevelUp && Levelable)
            {
                LevelUp();
            }
        }

        return projectileObject;
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected override void LevelUp()
    {
        // resets the old count of projectiles hit
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

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected override void JsonSetup()
    {
        base.JsonSetup();

        jsonVariables.Setup(ref projectiles, nameof(projectiles));
        jsonVariables.Setup(ref projectileJson, nameof(projectileJson));
        jsonVariables.Setup(ref projectilePath, nameof(projectilePath));
        jsonVariables.Setup(ref projectileHitsLevelUp, nameof(projectileHitsLevelUp));
    }
}

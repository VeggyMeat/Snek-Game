using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The engineer class, a miscellanious class
/// </summary>
internal class Engineer : Class
{
    /// <summary>
    /// The delay between spawning turrets
    /// </summary>
    private float spawnDelay;

    /// <summary>
    /// The path to the turret prefab
    /// </summary>
    private string turretPath;

    /// <summary>
    /// The path to the turret json
    /// </summary>
    private string turretJson;

    /// <summary>
    /// The list of turrets
    /// </summary>
    internal List<GameObject> turrets;

    /// <summary>
    /// The prefab for the turret
    /// </summary>
    private GameObject turret;

    /// <summary>
    /// The variables for the turret
    /// </summary>
    private JsonVariable turretVariables;

    /// <summary>
    /// The z value for the turret to be spawned at
    /// </summary>
    private const int zValue = 3;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/Engineer.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // indicates that this is a misc class
        body.classNames.Add("Misc");

        base.Setup();

        // gets the turret asset ready
        turret = Resources.Load<GameObject>(turretPath);

        // sets up the turret variables
        turretVariables = new JsonVariable(turretJson);

        StartTurretRepeating();
    }

    private void SummonTurret()
    {
        // create the turret
        GameObject turretSpawned = Instantiate(turret, new Vector3(transform.position.x, transform.position.y, zValue), Quaternion.identity);

        // grabs the turret controller
        TurretController controller = turretSpawned.GetComponent<TurretController>();

        // sets up the turret
        controller.Setup(turretVariables.Variables, this);
    }

    /// <summary>
    /// Starts repeatedly spawning the turrets
    /// </summary>
    internal void StartTurretRepeating()
    {
        InvokeRepeating(nameof(SummonTurret), spawnDelay / body.attackSpeedBuff.Value, spawnDelay / body.attackSpeedBuff.Value);
    }

    /// <summary>
    /// Stops repeatedly spawning the turrets
    /// </summary>
    internal void CancelTurretRepeating()
    {
        CancelInvoke(nameof(SummonTurret));
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        CancelTurretRepeating();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        StartTurretRepeating();
    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // resets the repeating spawn
        CancelTurretRepeating();
        StartTurretRepeating();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref turretJson, nameof(turretJson));
        jsonData.SetupAction(ref spawnDelay, nameof(spawnDelay), CancelTurretRepeating, StartTurretRepeating, jsonLoaded);
        jsonData.Setup(ref turretPath, nameof(turretPath));
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            turretVariables.IncreaseIndex();
        }
    }
}

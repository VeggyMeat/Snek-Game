using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Class
{
    internal string jsonPath = "Assets/Resources/Jsons/Classes/Misc/Engineer.json";

    public float spawnDelay;

    public string turretPath;
    public string turretJson;

    internal List<GameObject> turrets;

    private GameObject turret;

    internal override void Setup()
    {
        // sets up the json data into the class
        JsonSetup(jsonPath);

        base.Setup();

        // gets the turret asset ready
        turret = Resources.Load<GameObject>(turretPath);

        StartTurretRepeating();
    }

    private void SummonTurret()
    {
        // create the turret
        GameObject turretSpawned = Instantiate(turret, transform.position, Quaternion.identity);

        // grabs the turret controller
        TurretController controller = turretSpawned.GetComponent<TurretController>();

        // sets up the turret
        controller.Setup(turretJson, this);
    }

    // starts the invoke to spawn turrets
    internal void StartTurretRepeating()
    {
        InvokeRepeating(nameof(SummonTurret), spawnDelay, spawnDelay);
    }

    // stops the invoke to spawn turrets
    internal void CancelTurretRepeating()
    {
        CancelInvoke(nameof(SummonTurret));
    }

    // when it dies, stop it from spawning turrets
    internal override void OnDeath()
    {
        base.OnDeath();

        CancelTurretRepeating();
    }

    // when its revived, start it spawning turrets again
    internal override void Revived()
    {
        base.Revived();

        StartTurretRepeating();
    }
}

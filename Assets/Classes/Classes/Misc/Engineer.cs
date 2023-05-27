using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Class
{
    internal string jsonPath = "Assets/Resources/jsons/Classes/Misc/Engineer.json";

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

        InvokeRepeating(nameof(SummonTurret), spawnDelay, spawnDelay);
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
}

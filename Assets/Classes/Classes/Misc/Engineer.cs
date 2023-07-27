using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class Engineer : Class
{
    private float spawnDelay;

    private string turretPath;
    private string turretJson;

    internal List<GameObject> turrets;

    private GameObject turret;

    private JsonVariable turretVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/Engineer.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        body.classNames.Add("misc");

        base.Setup();

        // gets the turret asset ready
        turret = Resources.Load<GameObject>(turretPath);

        turretVariables = new JsonVariable(turretJson);

        StartTurretRepeating();
    }

    private void SummonTurret()
    {
        // create the turret
        GameObject turretSpawned = Instantiate(turret, transform.position, Quaternion.identity);

        // grabs the turret controller
        TurretController controller = turretSpawned.GetComponent<TurretController>();

        // sets up the turret
        controller.Setup(turretVariables.Variables, this);
    }

    // starts the invoke to spawn turrets
    internal void StartTurretRepeating()
    {
        InvokeRepeating(nameof(SummonTurret), spawnDelay / body.attackSpeedBuff.Value, spawnDelay / body.attackSpeedBuff.Value);
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

    // called when the attack speed buff changes
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // calls the base function
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // resets the repeating spawn
        CancelTurretRepeating();
        StartTurretRepeating();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref turretJson, "turretJson");

        if (jsonData.ContainsKey("spawnDelay"))
        {
            spawnDelay = float.Parse(jsonData["spawnDelay"].ToString());

            if (jsonLoaded)
            {
                CancelTurretRepeating();
                StartTurretRepeating();
            }
        }
        if (jsonData.ContainsKey("turretPath"))
        {
            turretPath = jsonData["turretPath"].ToString();

            if (jsonLoaded)
            {
                turret = Resources.Load<GameObject>(turretPath);
            }
        }
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            turretVariables.IncreaseIndex();
        }
    }
}

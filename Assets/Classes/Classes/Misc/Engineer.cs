using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Engineer : Class
{
    private float spawnDelay;

    private string turretPath;
    private string turretJson;

    internal List<GameObject> turrets;

    private GameObject turret;

    private List<Dictionary<string, object>> turretVariables;

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

        // loads in the text from the json file
        StreamReader reader = new StreamReader(turretJson);
        string text = reader.ReadToEnd();
        reader.Close();

        // converts the text to the variable list
        turretVariables = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);

        StartTurretRepeating();
    }

    private void SummonTurret()
    {
        // create the turret
        GameObject turretSpawned = Instantiate(turret, transform.position, Quaternion.identity);

        // grabs the turret controller
        TurretController controller = turretSpawned.GetComponent<TurretController>();

        // sets up the turret
        controller.Setup(turretVariables[body.Level - 1], this);
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

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "spawnDelay":
                    spawnDelay = float.Parse(jsonData[item].ToString());

                    if (jsonLoaded)
                    {
                        CancelTurretRepeating();
                        StartTurretRepeating();
                    }

                    break;
                case "turretPath":
                    turretPath = jsonData[item].ToString();

                    if (jsonLoaded)
                    {
                        turret = Resources.Load<GameObject>(turretPath);
                    }

                    break;
                case "turretJson":
                    turretJson = jsonData[item].ToString();
                    break;
            }
        }
    }
}

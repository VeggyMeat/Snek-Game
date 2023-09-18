using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArcherMage : Mage
{
    private JsonVariable orbVariables;

    private string orbPath;
    private string orbJson;

    private float orbs;

    private GameObject orbTemplate;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MagicArcher/MagicArcherMage.json";

        // indicates its not the primary class of the body
        primary = false;

        base.ClassSetup();
    }

    internal override void Setup()
    {
        orbTemplate = Resources.Load<GameObject>(orbPath);

        orbVariables = new JsonVariable(orbJson);

        base.Setup();
    }

    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            orbVariables.IncreaseIndex();
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref orbPath, "orbPath");
        jsonData.Setup(ref orbJson, "orbJson");
        jsonData.Setup(ref orbs, "orbs");
    }

    internal void LaunchOrbs(GameObject enemy)
    {
        // picks a random angle between 0 and pi / 2
        float angle = Random.Range(0, Mathf.PI / 2);

        // launches 'orbs' orbs evenly distributed in a circle
        for (double i = 0; i < Mathf.PI * 2; i += Mathf.PI * 2 / orbs)
        {
            // launches a projectile
            Projectile.Shoot(orbTemplate, enemy.transform.position, angle + (float)i, orbVariables.Variables, this, body.DamageMultiplier);
        }
    }
}

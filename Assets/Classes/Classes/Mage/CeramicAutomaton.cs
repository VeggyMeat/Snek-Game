using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CeramicAutomaton : Mage
{
    private int maxShield;
    private float shieldRegenDelay;

    private string boltJson;
    private string boltPath;

    private int shield;
    private bool shieldRegenActive = false;

    private GameObject boltTemplate;

    private List<Dictionary<string, object>> boltVariables;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Mage/CeramicAutomaton.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // sets the shield number to the maxShield number
        shield = maxShield;

        // grabs the projectile template from the path
        boltTemplate = Resources.Load<GameObject>(boltPath);

        // adds ChangeHealthTrigger to the BodyLostHealthTrigger
        TriggerManager.BodyLostHealthTrigger.AddTrigger(ChangeHealthTrigger);

        boltVariables = Projectile.LoadVariablesFromJson(boltJson);

        base.Setup();
    }

    // Called when the body takes damage
    internal int ChangeHealthTrigger(int quantity)
    {
        // if there is a shield left, it ignores damage
        if (shield > 0)
        {
            shield--;

            if (!shieldRegenActive)
            {
                Invoke(nameof(RegenShield), shieldRegenDelay);
                shieldRegenActive = true;
            }

            return 0;
        }

        return quantity;
    }

    // regens one layer of shield
    internal void RegenShield()
    {
        if (shield < maxShield)
        {
            shield++;

            if (shield < maxShield)
            {
                Invoke(nameof(RegenShield), shieldRegenDelay);
            }
            else
            {
                shieldRegenActive = false;
            }
        }
    }

    internal override void Attack()
    {
        // gets a random angle
        float angle = Random.Range(0, Mathf.PI * 2);

        // creates and sets up a new projectile
        Projectile.Shoot(boltTemplate, transform.position, angle, boltVariables[body.Level - 1], this, body.DamageMultiplier);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "boltPath":
                    boltPath = jsonData[item].ToString();

                    if (jsonLoaded)
                    {
                        // grabs the orb thats shot
                        boltTemplate = Resources.Load<GameObject>(boltPath);
                    }

                    break;
                case "boltJson":
                    boltJson = jsonData[item].ToString();
                    break;
                case "maxShield":
                    maxShield = int.Parse(jsonData[item].ToString());
                    break;
                case "shieldRegenDelay":
                    shieldRegenDelay = float.Parse(jsonData[item].ToString());
                    break;
            }
        }
    }
}

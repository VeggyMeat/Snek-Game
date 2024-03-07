using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ShieldmanEnchanter : Enchanter
{
    private int healthIncrease;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Shieldman/ShieldmanEnchanter.json";

        base.ClassSetup();
    }

    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // if its a frontline, double the health increase
        if (playerBody.classNames.Contains("Frontline"))
        {
            // increases the body's health by 2x healthIncrease
            playerBody.healthBuff.AddBuff(2 * healthIncrease, false, null);
        }
        else
        {
            // increases the body's health by healthIncrease
            playerBody.healthBuff.AddBuff(healthIncrease, false, null);
        }
    }

    protected override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // if its a frontline, double the health decrease
        if (playerBody.classNames.Contains("Frontline"))
        {
            // decreases the body's health by 2x healthIncrease
            playerBody.healthBuff.AddBuff(2 * -healthIncrease, false, null);
        }
        else
        {
            // decreases the body's health by healthIncrease
            playerBody.healthBuff.AddBuff(-healthIncrease, false, null);
        }
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("healthIncrease"))
        {
            if (jsonLoaded)
            {
                UnbuffAllBodies();
            }

            healthIncrease = int.Parse(jsonData["healthIncrease"].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }
    }
}

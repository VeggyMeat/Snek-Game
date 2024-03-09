using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The shieldman enchanter class, a subclass of the enchanter class
/// </summary>
internal class ShieldmanEnchanter : Enchanter
{
    /// <summary>
    /// The amount the bodies' health is increased by
    /// </summary>
    private int healthIncrease;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Shieldman/ShieldmanEnchanter.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Increases a body's health by the healthIncrease
    /// </summary>
    /// <param name="player">The body to buff</param>
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

    /// <summary>
    /// Decreases a body's health by the healthIncrease
    /// </summary>
    /// <param name="player">The body to remove the buff from</param>
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

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // sets up the healthIncrease variable and unbuffs and rebuffs all the bodies if the json is already loaded
        jsonData.SetupAction(ref healthIncrease, nameof(healthIncrease), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
    }
}

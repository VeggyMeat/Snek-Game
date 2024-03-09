using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The prince enchanter class, a subclass of the enchanter class
/// </summary>
internal class PrinceEnchanter : Enchanter
{
    /// <summary>
    /// The amount the bodies' damage is increased by
    /// </summary>
    private float damageIncrease;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Prince/PrinceEnchanter.json";

        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Increases a body's damage by the damageIncrease multiplier
    /// </summary>
    /// <param name="player">The body to buff</param>
    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // increases the body's damage by the damageIncrease multiplier
        playerBody.damageBuff.AddBuff(damageIncrease, true, null);
    }

    /// <summary>
    /// Removes the damage increase buff from the body
    /// </summary>
    /// <param name="player">The body to remove the buff from</param>
    protected override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // decreases the body's damage by the damageIncrease multiplier
        playerBody.damageBuff.AddBuff(1/damageIncrease, true, null);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // sets up the damageIncrease variable, unbuffing and rebuffing all the bodies if the json is already loaded
        jsonData.SetupAction(ref damageIncrease, nameof(damageIncrease), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
    }
}

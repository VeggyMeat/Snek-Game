using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The blacksmith class, a subclass of the enchanter class
/// </summary>
internal class Blacksmith : Enchanter
{
    /// <summary>
    /// The amount the bodies' defences are increased by 
    /// </summary>
    private int defenceIncrease;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Blacksmith.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Adds the buff to the body
    /// </summary>
    /// <param name="player">The body to add the buff to</param>
    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // increases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(defenceIncrease, false, null);
    }

    /// <summary>
    /// Removes thje buff from the body
    /// </summary>
    /// <param name="player">The body to remove the buff from</param>
    protected override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // decreases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(-defenceIncrease, false, null);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref defenceIncrease, nameof(defenceIncrease), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
    }
}

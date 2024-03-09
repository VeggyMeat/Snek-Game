using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// COMPLETE

/// <summary>
/// The fairy with a gun enchanter class, a subclass of the enchanter class
/// </summary>
internal class FairyWithAGunEnchanter : Enchanter
{
    /// <summary>
    /// The json for the gun data
    /// </summary>
    private string gunJson;

    /// <summary>
    /// The data for the guns
    /// </summary>
    private List<Dictionary<string, object>> gunData;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherEnchanter/FairyWithAGun/FairyWithAGunEnchanter.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Loads the gun data from the json into 'gunData'
    /// </summary>
    private void LoadGunData()
    {
        // loads in the text from the gunJson file
        StreamReader reader = new StreamReader(gunJson);
        string text = reader.ReadToEnd();
        reader.Close();

        // deserializes the json into a list of dictionaries containing the variables' contents for each level
        gunData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);
    }

    /// <summary>
    /// Buffs the body by adding a gun object onto their gameObject
    /// </summary>
    /// <param name="player">A body in the snake to buff</param>
    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // gives the body a gun
        Gun gun = player.AddComponent<Gun>();

        float damageModifier = 1f;

        // if the body is an archer, it gets a damage modifier of 2
        if (body.classNames.Contains("Archer"))
        {
            damageModifier = 2f;
        }

        // sets up the gun
        gun.Setup(body.classes[0], damageModifier, gunData);

        // matches the gun to the current level
        for (int i = 1; i < body.Level; i++)
        {
            gun.UpgradeGun();
        }
    }

    /// <summary>
    /// Removes the gun from the body
    /// </summary>
    /// <param name="player">The body in the snake to remove the buff from</param>
    /// <exception cref="System.Exception">Throws an error if the body doesn't have a gun</exception>
    protected override void RemoveBuff(GameObject player)
    {
        // if it doesnt have a gun, raise an error
        if (!player.TryGetComponent(out Gun gun))
        {
            throw new System.Exception();
        }

        // otherwise destroy the gun
        Destroy(gun);
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    /// <exception cref="System.Exception">Throws an exception if one of the bodies does not have a gun</exception>
    internal override void LevelUp()
    {
        base.LevelUp();

        // if its not level one, upgrade the bullets
        if (body.Level != 1)
        {
            // starts at the head
            BodyController gunBody = body.snake.Head;

            while (gunBody is not null)
            {
                Gun gun;

                // if it doesnt have a gun, raise an error
                if (!gunBody.TryGetComponent(out gun))
                {
                    throw new System.Exception();
                }

                // upgrade the gun
                gun.UpgradeGun();

                // get the next body in the snake
                gunBody = gunBody.next;
            }
        }
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        jsonData.SetupAction(ref gunJson, nameof(gunJson), null, LoadGunData, true);

        base.InternalJsonSetup(jsonData);
    }
}

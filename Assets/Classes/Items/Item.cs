using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Item
{
    protected string itemName;
    protected string itemDescription;

    protected string jsonPath;
    protected List<Dictionary<string, object>> jsonData;
    protected Dictionary<string, object> jsonVariables;

    protected bool jsonLoaded = false;

    protected int level = 0;
    protected int maxLevel;

    /// <summary>
    /// The name of the item
    /// </summary>
    public string ItemName { get => itemName; }

    /// <summary>
    /// The description that goes with the item explaining what it does
    /// </summary>
    public string ItemDescription { get => itemDescription; }

    /// <summary>
    /// Whether the item can be leveled more or not
    /// </summary>
    public bool Levelable { get => level < maxLevel; }

    /// <summary>
    /// The current level of the item (starting at 1)
    /// </summary>
    public int Level { get => level; }

    /// <summary>
    /// Sets up the item initially
    /// </summary>
    internal virtual void Setup()
    {
        // loads in the data from the json
        LoadJson();

        maxLevel = jsonData.Count;

        // levels up the item initially
        LevelUp();
    }

    /// <summary>
    /// Loads in the json data from the json file
    /// </summary>
    protected void LoadJson()
    {
        string jsonString = File.ReadAllText(jsonPath);
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonString);
    }

    /// <summary>
    /// Sets up the variables from the jsonVariables data
    /// </summary>
    protected virtual void JsonSetup()
    {
        jsonVariables.Setup(ref itemName, nameof(itemName));
        jsonVariables.Setup(ref itemDescription, nameof(itemDescription));
    }

    /// <summary>
    /// Levels up the item
    /// </summary>
    protected virtual void LevelUp()
    {
        // increases the level
        level++;

        // loads in the jsonVariables from the jsonData
        jsonVariables = jsonData[level - 1];

        // sets up the variables based upon the json
        JsonSetup();

        jsonLoaded = true;
    }
}
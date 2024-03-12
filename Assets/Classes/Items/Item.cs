using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

// COMPLETE

/// <summary>
/// The base class for all items
/// </summary>
internal abstract class Item
{
    /// <summary>
    /// The name of the item
    /// </summary>
    protected string itemName;

    /// <summary>
    /// The description of the item
    /// </summary>
    protected string itemDescription;

    /// <summary>
    /// The path to the json file for the item
    /// </summary>
    protected string jsonPath;

    /// <summary>
    /// The json data for the item
    /// </summary>
    protected List<Dictionary<string, object>> jsonData;

    /// <summary>
    /// The current variables for this level of the item
    /// </summary>
    protected Dictionary<string, object> jsonVariables;

    /// <summary>
    /// Whether the json has been loaded or not
    /// </summary>
    protected bool jsonLoaded = false;

    /// <summary>
    /// The game setup
    /// </summary>
    protected IGameSetup gameSetup;

    /// <summary>
    /// The current level of the item
    /// </summary>
    protected int level = 0;

    /// <summary>
    /// The maximum level of the item
    /// </summary>
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
    /// <param name="gameSetup">The game setup</param>
    internal virtual void Setup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;

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

        if (level > maxLevel)
        {
            throw new Exception("Item tried to level up when it was already max level");
        }

        // loads in the jsonVariables from the jsonData
        jsonVariables = jsonData[level - 1];

        // sets up the variables based upon the json
        JsonSetup();

        jsonLoaded = true;
    }
}
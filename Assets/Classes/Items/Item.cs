using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Item
{
    protected string itemName;
    protected string itemDescription;

    protected string jsonPath;
    protected List<Dictionary<string, object>> jsonData;
    protected Dictionary<string, object> jsonVariables;

    protected int level = 0;
    protected int maxLevel;

    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }

    public bool Levelable { get => level < maxLevel; }

    public int Level { get => level; }

    internal virtual void Setup()
    {
        // loads in the data from the json
        LoadJson();

        maxLevel = jsonData.Count;

        // levels up the item initially
        LevelUp();
    }

    protected void LoadJson()
    {
        string jsonString = File.ReadAllText(jsonPath);
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonString);
    }

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
    }
}
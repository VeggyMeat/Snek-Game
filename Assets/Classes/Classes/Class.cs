using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Class: MonoBehaviour
{
    protected int enemiesKilled;

    internal BodyController body;

    protected string jsonPath;
    protected List<Dictionary<string, object>> jsonData;

    private string BodyJson
    {
        set { body.jsonFile = value; }
    }

    protected bool jsonLoaded;

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal virtual void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;
    }

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal virtual void ClassSetup()
    {
        jsonData = JsonToLevelData(jsonPath);
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy">The enemy's GameObject</param>
    internal virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        body.snake.IncreaseXP(enemy.GetComponent<EnemyController>().XPDrop);
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal void LevelUp()
    {
        JsonSetup(jsonData[body.level - 1]);
    }

    /// <summary>
    /// Takes a json file and turns it into a dictionary, calling InternalJsonSetup
    /// </summary>
    /// <param name="json">The json file</param>
    internal List<Dictionary<string, object>> JsonToLevelData(string json)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(json);
        string text = reader.ReadToEnd();
        reader.Close();

        List<Dictionary<string, object>> values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);

        return values;
    }

    /// <summary>
    /// Takes json data and turns it into a dictionary, calling InternalJsonSetup
    /// </summary>
    /// <param name="data">The data in a dictionary format</param>
    internal void JsonSetup(Dictionary<string, object> data)
    {
        InternalJsonSetup(data);
        
        if (jsonLoaded)
        {
            body.LoadFromJson();
        }

        jsonLoaded = true;
    }

    /// <summary>
    /// overwrites the variables in the json
    /// </summary>
    /// <param name="jsonData"></param>
    protected virtual void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "bodyJson":
                    BodyJson = jsonData["bodyJson"].ToString();

                    body.JsonToBodyData();
                    break;
            }
        }
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal virtual void Revived()
    {

    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal virtual void OnDeath()
    {

    }

    /// <summary>
    /// Called when the HealthBuff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal virtual void OnHealthBuffUpdate(float amount, bool multiplicative)
    {

    }

    /// <summary>
    /// Called when speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal virtual void OnSpeedBuffUpdate(float amount, bool multiplicative)
    {

    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal virtual void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {

    }
}

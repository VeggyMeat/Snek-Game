using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Class: MonoBehaviour
{
    internal int enemiesKilled;
    internal int level = 1;
    internal BodyController body;

    internal string jsonPath;

    private string BodyJson
    {
        get { return body.jsonFile; }
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
        JsonSetupFile(jsonPath);
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
    internal virtual void LevelUp()
    {

    }

    /// <summary>
    /// Takes a json file and turns it into a dictionary, calling InternalJsonSetup
    /// </summary>
    /// <param name="json">The json file</param>
    internal void JsonSetupFile(string json)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(json);
        string text = reader.ReadToEnd();
        reader.Close();

        Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);

        InternalJsonSetup(values);

        if (jsonLoaded)
        {
            body.LoadFromJson();
        }

        jsonLoaded = true;
    }

    /// <summary>
    /// Takes json data and turns it into a dictionary, calling InternalJsonSetup
    /// </summary>
    /// <param name="json">The json data in string format</param>
    internal void JsonSetupString(string json)
    {
        Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        InternalJsonSetup(values);
        
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

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public abstract class Class: MonoBehaviour
{
    /// <summary>
    /// Whether this is the primary class on a body
    /// </summary>
    protected bool primary = true;

    /// <summary>
    /// The number of enemies killed by this class
    /// </summary>
    protected int enemiesKilled = 0;

    /// <summary>
    /// The body this class is attatched to
    /// </summary>
    internal BodyController body;

    /// <summary>
    /// The path of the json file
    /// </summary>
    protected string jsonPath;

    /// <summary>
    /// List of Dictionaries containing the variable information for the class at each level
    /// </summary>
    protected List<Dictionary<string, object>> jsonData;

    /// <summary>
    /// The body's json file's path
    /// </summary>
    private string BodyJson
    {
        set 
        { 
            // sets the value, and then loads the body's variables from the json
            body.jsonFile = value;
            body.JsonToBodyData();
        }
    }

    /// <summary>
    /// Whether this class's variables have been loaded from a json yet
    /// </summary>
    protected bool jsonLoaded;

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal virtual void Setup()
    {

    }

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal virtual void ClassSetup()
    {
        // loads in the text from the file
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        // deserializes the json into a list of dictionaries containing the variables' contents for each level
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);
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

        TriggerManager.BodyKilledTrigger.CallTrigger(body.gameObject);
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal virtual void LevelUp()
    {
        JsonSetup(jsonData[body.Level - 1]);
    }

    /// <summary>
    /// Takes json data and turns it into a dictionary, calling InternalJsonSetup
    /// </summary>
    /// <param name="data">The data in a dictionary format</param>
    private void JsonSetup(Dictionary<string, object> data)
    {
        // overwrites the values in the classes json
        InternalJsonSetup(data);
        
        // loads the body's variables from the json
        if (jsonLoaded)
        {
            if (primary)
            {
                body.LoadFromJson();
            }
        }
        else
        {
            jsonLoaded = true;
        }
    }

    /// <summary>
    /// Overwrites the variables in the json
    /// </summary>
    /// <param name="jsonData"></param>
    protected virtual void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        if (jsonData.ContainsKey("bodyJson"))
        {
            BodyJson = jsonData["bodyJson"].ToString();
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

    /// <summary>
    /// Called when the body takes damage, before the damage is applied
    /// </summary>
    /// <param name="amount">The damage taken</param>
    /// <returns>Return the new damage value</returns>
    internal virtual int OnDamageTaken(int amount)
    {
        return amount;
    }
}

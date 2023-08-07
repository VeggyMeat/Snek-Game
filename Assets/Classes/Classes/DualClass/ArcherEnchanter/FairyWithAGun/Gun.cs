using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class Gun : MonoBehaviour
{
    private int level = 1;

    internal int Level
    {
        get
        {
            return level;
        }
    }

    private List<Dictionary<string, object>> jsonData;
    private Class attatched;

    private float timeDelay;
    private bool jsonLoaded = false;

    private GameObject bulletPrefab;
    private string bulletPath;
    private string bulletJson;
    private List<Dictionary<string, object>> bulletVariables;

    private float damageMultiplication;

    internal void Setup(Class attatched, float damageMultiplication, List<Dictionary<string, object>> variables)
    {
        Debug.Log(variables);

        // matches the jsonData to the variables
        jsonData = variables;

        // loads in the data from the variables
        LoadVariables();

        // Loads in the data from the json
        LoadJson();

        // loads in the bullet
        bulletPrefab = Resources.Load<GameObject>(bulletPath);

        // attatches the gun to the class
        this.attatched = attatched;

        // sets the damageMultiplication
        this.damageMultiplication = damageMultiplication;
    }

    internal void UpgradeGun()
    {
        // increases the level
        level++;

        // loads in the data from the json
        LoadJson();

        // loads in the data from the json into the variables
        LoadVariables();
    }

    private void LoadJson()
    {
        // loads in the text from the file
        StreamReader reader = new StreamReader(bulletJson);
        string text = reader.ReadToEnd();
        reader.Close();

        // deserializes the json into a list of dictionaries containing the variables' contents for each level
        bulletVariables = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);
    }

    private void LoadVariables()
    {
        Dictionary<string, object> data = jsonData[level];

        if (data.ContainsKey("timeDelay"))
        {
            timeDelay = float.Parse(data["timeDelay"].ToString());

            // if the json has already been loaded, it stops firing projectiles and starts again with the new time delay
            if (jsonLoaded)
            {
                StopFiringProjectiles();
            }

            StartFiringProjectiles();
        }
    }

    private void FireProjectile()
    {
        Projectile.Shoot(bulletPrefab, transform.position, UnityEngine.Random.Range(0, 2 * Mathf.PI), bulletVariables[level - 1], attatched, damageMultiplication);
    }

    private void StartFiringProjectiles()
    {
        InvokeRepeating(nameof(FireProjectile), timeDelay, timeDelay);
    }

    private void StopFiringProjectiles()
    {
        CancelInvoke(nameof(FireProjectile));
    }
}

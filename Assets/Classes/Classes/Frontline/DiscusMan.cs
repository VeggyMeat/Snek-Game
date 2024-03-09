using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The DiscusMan class, a subclass of the frontline class
/// </summary>
internal class DiscusMan : Frontline
{
    /// <summary>
    /// The max number of discuses that can be spawned
    /// </summary>
    private int maxDiscuses;

    /// <summary>
    /// The json for the discuses
    /// </summary>
    private string discusJson;

    /// <summary>
    /// The prefab for the discus gameObject
    /// </summary>
    private GameObject discusPrefab;

    /// <summary>
    /// The path to the discus prefab
    /// </summary>
    private string discusPath;

    /// <summary>
    /// The variables stored in the discusJson
    /// </summary>
    private JsonVariable discusVariable;


    /// <summary>
    /// The radius of the orbit of the discuses around the discus man
    /// </summary>
    private float orbitRadius;

    /// <summary>
    /// The angular speed at which the discuses orbit
    /// </summary>
    private float orbitSpeed;

    /// <summary>
    /// The time delay between discus spawns
    /// </summary>
    private float discusDelay;

    /// <summary>
    /// The current list of the discuses
    /// </summary>
    internal List<Discus> discuses = new List<Discus>();

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal override void LevelUp()
    {
        base.LevelUp();

        if (body.Level != 1)
        {
            // levels up the discusVariable
            discusVariable.IncreaseIndex();
        }

        // tell each discus that its leveled up
        foreach (Discus discus in discuses)
        {
            discus.OnLevelUp();
        }
    }

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/DiscusMan.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // sets up discusVariable
        discusVariable = new JsonVariable(discusJson);

        // grabs the discus's prefab
        discusPrefab = Resources.Load<GameObject>(discusPath);

        base.Setup();
    }

    // Called by unity every frame before doing any physics calculations 
    private void FixedUpdate()
    {
        // rotates the body
        transform.Rotate(Vector3.forward * orbitSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // kills all discuses
        KillAllDiscuses();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();
        
        // starts spawning discuses
        SpawnDiscus();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // sets up all the variables from the json
        jsonData.Setup(ref discusJson, nameof(discusJson));
        jsonData.Setup(ref discusDelay, nameof(discusDelay));
        jsonData.Setup(ref discusPath, nameof(discusPath));

        jsonData.Setup(ref orbitSpeed, nameof(orbitSpeed));

        if (jsonData.ContainsKey(nameof(maxDiscuses)))
        {
            // gets the new maxDiscuses value
            maxDiscuses = int.Parse(jsonData[nameof(maxDiscuses)].ToString());

            // if the number of max discuses increased then run spawnDiscus
            if (maxDiscuses > discuses.Count)
            {
                Invoke(nameof(SpawnDiscus), discusDelay);
            }
            
            // removes discuses until there are as many as the count
            while (maxDiscuses < discuses.Count)
            {
                RemoveDiscus();
            }
        }

        jsonData.SetupAction(ref orbitRadius, nameof(orbitRadius), null, SetDiscusesPositions, true);
    }

    /// <summary>
    /// Kills all the discuses in the list
    /// </summary>
    private void KillAllDiscuses()
    {
        // goes through each discus and kills it
        for (int i = discuses.Count - 1; i >= 0; i--)
        {
            Discus discus = discuses[i];

            discus.Die();
        }
    }

    /// <summary>
    /// Sets the position of all the discuses
    /// </summary>
    private void SetDiscusesPositions()
    {
        // sets all the discuses to their correct angle
        float angle = 0;
        float addedAngle = Mathf.PI * 2 / discuses.Count;

        // set the poisiton of all discuses using some trig
        foreach (Discus discusObj in discuses)
        {
            discusObj.transform.position = new Vector3(Mathf.Cos(angle) * orbitRadius, Mathf.Sin(angle) * orbitRadius, 0) + transform.position;

            angle += addedAngle;
        }
    }

    /// <summary>
    /// Spawns a new discus
    /// </summary>
    private void SpawnDiscus()
    {
        // spawns a new discus
        GameObject discus = Instantiate(discusPrefab, transform);
        Discus discusScript = discus.GetComponent<Discus>();
        discusScript.Setup(ref discusVariable, this);

        SetDiscusesPositions();

        // if more discuses can be spawned, then call this function again after the delay
        if (discuses.Count < maxDiscuses)
        {
            Invoke(nameof(SpawnDiscus), discusDelay);
        }
    }

    /// <summary>
    /// Removes a discus from the list
    /// </summary>
    private void RemoveDiscus()
    {
        // kills the first discus in the list
        discuses[0].Die();
    }
}

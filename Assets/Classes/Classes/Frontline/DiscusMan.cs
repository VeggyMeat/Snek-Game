using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class DiscusMan : Frontline
{
    private int maxDiscuses;
    private string discusJson;
    private GameObject discusPrefab;
    private string discusPath;
    private JsonVariable discusVariable;

    private float orbitRadius;
    private float orbitSpeed;

    private float discusDelay;

    internal List<Discus> discuses = new List<Discus>();

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

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Frontline/DiscusMan.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        // sets up discusVariable
        discusVariable = new JsonVariable(discusJson);

        // grabs the discus's prefab
        discusPrefab = Resources.Load<GameObject>(discusPath);

        base.Setup();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * orbitSpeed * Time.deltaTime);
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // kills all discuses
        KillAllDiscuses();
    }

    internal override void Revived()
    {
        base.Revived();
        
        // starts spawning discuses
        SpawnDiscus();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // sets up all the variables from the json
        jsonData.Setup(ref discusJson, nameof(discusJson));
        jsonData.Setup(ref discusDelay, nameof(discusDelay));
        jsonData.Setup(ref discusPath, nameof(discusPath));

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

        jsonData.SetupAction(ref orbitSpeed, nameof(orbitSpeed), null, SetAngularVelocity, true);
        jsonData.SetupAction(ref orbitRadius, nameof(orbitRadius), null, SetDiscusesPositions, true);
    }

    private void SetAngularVelocity()
    {
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = orbitSpeed;
    }

    private void KillAllDiscuses()
    {
        // goes through each discus and kills it
        for (int i = discuses.Count - 1; i >= 0; i--)
        {
            Discus discus = discuses[i];

            discus.Die();
        }
    }

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

    private void SpawnDiscus()
    {
        // spawns a new discus
        GameObject discus = Instantiate(discusPrefab, transform);
        Discus discusScript = discus.GetComponent<Discus>();
        discusScript.Setup(ref discusVariable, this);

        // sets the position of all discuses
        SetDiscusesPositions();

        // if more discuses can be spawned
        if (discuses.Count < maxDiscuses)
        {
            Invoke(nameof(SpawnDiscus), discusDelay);
        }
    }

    private void RemoveDiscus()
    {
        // kills the first discus in the list
        discuses[0].Die();
    }
}

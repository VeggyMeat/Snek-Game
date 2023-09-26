using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbologist : Enchanter
{
    private JsonVariable healingOrbVariables;

    private float healingOrbDelay;
    private string healingOrbPath;

    private bool summoning;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Herbologist.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        StartSummoning();
    }

    private void SummonHealingOrb()
    {

    }

    private void StartSummoning()
    {
        if (summoning)
        {
            return;
        }

        InvokeRepeating(nameof(SummonHealingOrb), healingOrbDelay, healingOrbDelay);

        summoning = true;
    }

    private void StopSummoning()
    {
        if (!summoning) 
        { 
            return; 
        }

        CancelInvoke();

        summoning = false;
    }

    internal override void Revived()
    {
        base.Revived();

        StartSummoning();
    }

    internal override void OnDeath()
    {
        StopSummoning();

        base.OnDeath();
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref healingOrbPath, nameof(healingOrbPath));

        if (jsonData.ContainsKey(nameof(healingOrbDelay)))
        {
            healingOrbDelay = int.Parse(jsonData[nameof(healingOrbDelay)].ToString());

            StopSummoning();
            StopSummoning();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Blacksmith : Enchanter
{
    private int defenceIncrease;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Blacksmith.json";

        base.ClassSetup();
    }

    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // increases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(defenceIncrease, false, null);
    }

    protected override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // decreases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(-defenceIncrease, false, null);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref defenceIncrease, nameof(defenceIncrease), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
    }
}

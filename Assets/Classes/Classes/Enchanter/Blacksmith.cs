using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blacksmith : Enchanter
{
    public int defenceIncrease;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Blacksmith.json";

        base.ClassSetup();
    }

    internal override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // increases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(defenceIncrease, false, null);
    }

    internal override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController body = player.GetComponent<BodyController>();

        // increases the body's defence by the defenceIncrease
        body.defenceBuff.AddBuff(-defenceIncrease, false, null);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceEnchanter : Enchanter
{
    private float damageIncrease;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineEnchanter/Prince/PrinceEnchanter.json";

        primary = false;

        base.ClassSetup();
    }

    protected override void AddBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // increases the body's damage by the damageIncrease multiplier
        playerBody.damageBuff.AddBuff(damageIncrease, true, null);
    }

    protected override void RemoveBuff(GameObject player)
    {
        // gets the BodyController
        BodyController playerBody = player.GetComponent<BodyController>();

        // decreases the body's damage by the damageIncrease multiplier
        playerBody.damageBuff.AddBuff(1/damageIncrease, true, null);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("damageIncrease"))
        {
            if (jsonLoaded)
            {
                UnbuffAllBodies();
            }

            damageIncrease = float.Parse(jsonData["damageIncrease"].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }
    }
}

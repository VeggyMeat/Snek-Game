using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : Item
{
    private float attackSpeedModifier;

    internal override void Setup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/Quiver.json";

        // sets up the variables based upon the json
        JsonSetup();

        base.Setup();
        
        // when a new body is added calls the AddArcherBuff function on it
        TriggerManager.BodySpawnTrigger.AddTrigger(AddArcherBuff);

        // buffs all the current archers
        BuffArchers();
    }

    // adds the buff to a body controller if its an archer
    private BodyController AddArcherBuff(BodyController body)
    {
        // if its an archer
        if (body.classNames.Contains("Archer"))
        {
            // add the buff
            body.attackSpeedBuff.AddBuff(attackSpeedModifier, true, null);
        }

        return body;
    }

    // currently un-used, will be used when levels added
    private void RemoveBuff(BodyController body)
    {
        body.attackSpeedBuff.AddBuff(1 / attackSpeedModifier, true, null);
    }

    // goes through and buffs each archer
    private void BuffArchers()
    {
        // gets the head of the snake
        BodyController body = ItemManager.headController.head;

        while (body is not null)
        {
            // tries to buff the body if its an archer
            AddArcherBuff(body);

            // gets the next body in the snake
            body = body.next;
        }
    }

    protected override void JsonSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Items/Quiver.json";

        base.JsonSetup();

        foreach (string item in jsonData.Keys)
        {
            switch (item)
            {
                case "attackSpeedModifier":
                    attackSpeedModifier = float.Parse(jsonData[item].ToString());
                    break;
            }
        }
    }
}

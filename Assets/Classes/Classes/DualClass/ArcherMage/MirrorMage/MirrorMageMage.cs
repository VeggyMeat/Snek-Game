using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class MirrorMageMage : Mage
{
    private float speedBuff;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MirrorMage/MirrorMageMage.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        BuffAllBodies();

        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
    }

    private BodyController BuffBody(BodyController bodyCon)
    {
        if (bodyCon != body)
        {
            // makes the body slower
            bodyCon.attackSpeedBuff.AddBuff(speedBuff, true, null);
        }

        return bodyCon;
    }

    private void UnBuffBody(BodyController bodyCon)
    {
        if (bodyCon != body)
        {
            // removes the slowness on the body
            bodyCon.attackSpeedBuff.AddBuff(1 / speedBuff, true, null);
        }
    }

    private void BuffAllBodies()
    {
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            BuffBody(currentBody);

            currentBody = currentBody.next;
        }
    }

    private void UnBuffAllBodies() 
    {
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            UnBuffBody(currentBody);

            currentBody = currentBody.next;
        }
    }

    internal override void Revived()
    {
        base.Revived();

        BuffAllBodies();

        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        UnBuffAllBodies();

        TriggerManager.BodySpawnTrigger.RemoveTrigger(BuffBody);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);
        
        if (jsonData.ContainsKey(nameof(speedBuff)))
        {
            if (jsonLoaded)
            {
                UnBuffAllBodies();
            }

            speedBuff = float.Parse(jsonData[nameof(speedBuff)].ToString());

            if (jsonLoaded)
            {
                BuffAllBodies();
            }
        }
    }
}

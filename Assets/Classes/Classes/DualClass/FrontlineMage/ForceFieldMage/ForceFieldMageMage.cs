using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldMageMage : Mage
{
    private float buffHealth;
    private float buffTime;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/ForceFieldMage/ForceFieldMageMage.json";

        primary = false;

        base.ClassSetup();
    }

    internal void BuffBody()
    {
        // get a random body from the snake
        List<BodyController> bodyControllers = new List<BodyController>();

        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            bodyControllers.Add(currentBody);
            currentBody = currentBody.next;
        }

        BodyController selectedBody = bodyControllers.RandomItem();

        // buffs that body
        selectedBody.healthBuff.AddBuff(buffHealth, false, buffTime);
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref buffHealth, nameof(buffHealth));
        jsonData.Setup(ref buffTime, nameof(buffTime));
    }
}

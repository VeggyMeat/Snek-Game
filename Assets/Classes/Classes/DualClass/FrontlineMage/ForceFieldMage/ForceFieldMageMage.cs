using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The force field mage mage class, a subclass of the mage class
/// </summary>
internal class ForceFieldMageMage : Mage
{
    /// <summary>
    /// The amount of health to give the buffed bodies
    /// </summary>
    private float buffHealth;

    /// <summary>
    /// How long the buff lasts for
    /// </summary>
    private float buffTime;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/ForceFieldMage/ForceFieldMageMage.json";

        primary = false;

        base.ClassSetup();
    }

    /// <summary>
    /// Buffs a random body in the snake
    /// </summary>
    internal void BuffBody()
    {
        // get a random body from the snake
        List<BodyController> bodyControllers = new List<BodyController>();

        // goes through each body in the linked list
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            bodyControllers.Add(currentBody);
            currentBody = currentBody.next;
        }

        // grabs a random item from the list
        BodyController selectedBody = bodyControllers.RandomItem();

        // buffs that body
        selectedBody.healthBuff.AddBuff(buffHealth, false, buffTime);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref buffHealth, nameof(buffHealth));
        jsonData.Setup(ref buffTime, nameof(buffTime));
    }
}

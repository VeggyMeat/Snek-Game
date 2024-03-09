using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The mirror mage mage class, a subclass of the mage class
/// </summary>
internal class MirrorMageMage : Mage
{
    /// <summary>
    /// The value to increase the speed buff by
    /// </summary>
    private float speedBuff;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/ArcherMage/MirrorMage/MirrorMageMage.json";

        base.ClassSetup();
    }


    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        BuffAllBodies();

        // adds the buff to all bodies that spawn
        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
    }

    /// <summary>
    /// Buffs the body to attack slower
    /// </summary>
    /// <param name="bodyCon">The body to buff</param>
    /// <returns>The body to buff</returns>
    private BodyController BuffBody(BodyController bodyCon)
    {
        if (bodyCon != body)
        {
            // makes the body slower
            bodyCon.attackSpeedBuff.AddBuff(speedBuff, true, null);
        }

        return bodyCon;
    }

    /// <summary>
    /// Unbuffs the body 
    /// </summary>
    /// <param name="bodyCon">The body to remove the buff from</param>
    private void UnBuffBody(BodyController bodyCon)
    {
        if (bodyCon != body)
        {
            // removes the slowness on the body
            bodyCon.attackSpeedBuff.AddBuff(1 / speedBuff, true, null);
        }
    }

    /// <summary>
    /// Buffs all current bodies in the snake
    /// </summary>
    private void BuffAllBodies()
    {
        // goes through each body in the linked list
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            BuffBody(currentBody);

            currentBody = currentBody.next;
        }
    }

    /// <summary>
    /// Removes the buff from all current bodies in the snake
    /// </summary>
    private void UnBuffAllBodies() 
    {
        // goes through each body in the linked list
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            UnBuffBody(currentBody);

            currentBody = currentBody.next;
        }
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        BuffAllBodies();

        // adds back the trigger to buff newly added bodies
        TriggerManager.BodySpawnTrigger.AddTrigger(BuffBody);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        UnBuffAllBodies();

        // removes the trigger to stop buffing new bodies
        TriggerManager.BodySpawnTrigger.RemoveTrigger(BuffBody);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref speedBuff, nameof(speedBuff), UnBuffAllBodies, BuffAllBodies, jsonLoaded);
    }
}

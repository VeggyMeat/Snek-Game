using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The arcane flower enchanter class, a subclass of the enchanter class
/// </summary>
internal class ArcaneFlowerEnchanter : Enchanter
{
    /// <summary>
    /// The multiplicative damage modifier for the bodies
    /// </summary>
    private float damageModifier;

    /// <summary>
    /// The multiplicative healing modifier for the snake
    /// </summary>
    private float healingModifier;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/ArcaneFlower/ArcaneFlowerEnchanter.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // sets the snake's healing modifier
        body.snake.healingModifier *= healingModifier;
    }

    /// <summary>
    /// Adds the damage modifier to the body
    /// </summary>
    /// <param name="bodyObject">The body to buff</param>
    protected override void AddBuff(GameObject bodyObject)
    {
        // grabs the body controller
        BodyController bodyController = bodyObject.GetComponent<BodyController>();

        // adds the damage modifier to the body
        bodyController.damageBuff.AddBuff(damageModifier, true, null);
    }

    /// <summary>
    /// Removes the damage modifier from the body
    /// </summary>
    /// <param name="bodyObject">The body to remove the buff from</param>
    protected override void RemoveBuff(GameObject bodyObject)
    {
        // grabs the body controller
        BodyController bodyController = bodyObject.GetComponent<BodyController>();

        // removes the damage modifier from the body
        bodyController.damageBuff.AddBuff(1 / damageModifier, true, null);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        RemoveSnakeBuff();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.LevelUp();

        BuffSnake();
    }

    /// <summary>
    /// Buffs the snake by adding the healing modifier
    /// </summary>
    private void BuffSnake()
    {
        body.snake.healingModifier *= healingModifier;
    }

    /// <summary>
    /// Removes the snake's healing modifier
    /// </summary>
    private void RemoveSnakeBuff()
    {
        body.snake.healingModifier /= healingModifier;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref damageModifier, nameof(healingModifier), UnbuffAllBodies, BuffAllBodies, jsonLoaded);
        jsonData.SetupAction(ref healingModifier, nameof(healingModifier), BuffSnake, RemoveSnakeBuff, jsonLoaded);
    }
}

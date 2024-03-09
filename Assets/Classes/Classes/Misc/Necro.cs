using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The necromancer class, a miscellanious class
/// </summary>
internal class Necro : Class
{
    /// <summary>
    /// The maximum number of zombies that can be summoned
    /// </summary>
    private int maxSummoned;

    /// <summary>
    /// The path for the zombie prefab
    /// </summary>
    private string zombiePath;

    /// <summary>
    /// The json for the zombie
    /// </summary>
    private string zombieJson;

    /// <summary>
    /// The number of zombies currently summoned
    /// </summary>
    internal int zombieNumber = 0;

    /// <summary>
    /// The variables for the necro zombies
    /// </summary>
    private JsonVariable necroZombieVariables;

    /// <summary>
    /// The zombie prefab
    /// </summary>
    private GameObject zombie;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Misc/Necro.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        // indicates that this is a misc class
        body.classNames.Add("Misc");

        // adds this to the enemy death trigger list
        TriggerManager.EnemyDeadTrigger.AddTrigger(EnemyKilledTrigger);

        // gets the zombie asset ready
        zombie = Resources.Load<GameObject>(zombiePath);

        // sets up the zombie variables
        necroZombieVariables = new JsonVariable(zombieJson);

        base.Setup();
    }

    /// <summary>
    /// Called when an enemy is killed
    /// </summary>
    /// <param name="enemy">The enemy that is killed</param>
    /// <returns>The enemy that is killed</returns>
    private GameObject EnemyKilledTrigger(GameObject enemy)
    {
        // if there is capacity for another zombie spawn it
        if (zombieNumber < maxSummoned)
        {
            SummonZombie(enemy.transform);
        }

        return enemy;
    }

    /// <summary>
    /// Spawns a new zombie at the given position
    /// </summary>
    /// <param name="position">The position to spawn the zombie</param>
    private void SummonZombie(Transform position)
    {
        // spawns a zombie at the position
        GameObject summonedZombie = Instantiate(zombie, new Vector3(position.position.x, position.position.y, 0f), Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        
        // sets up the zombie
        NecromancerZombieController controller = summonedZombie.GetComponent<NecromancerZombieController>();
        controller.Setup(necroZombieVariables.Variables, this, body.DamageMultiplier);

        // adds the zombie to the list of controlled zombies
        zombieNumber++;
    }

    /// <summary>
    /// Called when a zombie dies
    /// </summary>
    internal void ZombieDeath()
    {
        zombieNumber--;
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // adds this back to the list for enemy death triggers
        TriggerManager.EnemyDeadTrigger.AddTrigger(EnemyKilledTrigger);
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes this from the list of enemy death triggers
        TriggerManager.EnemyDeadTrigger.RemoveTrigger(EnemyKilledTrigger);
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxSummoned, nameof(maxSummoned));
        jsonData.Setup(ref zombieJson, nameof(zombieJson));
        jsonData.Setup(ref zombiePath, nameof(zombiePath));
    }

    /// <summary>
    /// Called by the body when it levels up
    /// </summary>
    internal override void LevelUp()
    {        
        base.LevelUp();

        // levels up the necro zombies
        if (!jsonLoaded)
        {
            necroZombieVariables.IncreaseIndex();
        }
    }
}

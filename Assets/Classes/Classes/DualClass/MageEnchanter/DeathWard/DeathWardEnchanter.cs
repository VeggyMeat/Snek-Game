using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The death ward enchanter class, a subclass of the enchanter class
/// </summary>
internal class DeathWardEnchanter : Enchanter
{
    /// <summary>
    /// The max Health increase of the death ward when an enemy dies
    /// </summary>
    private int maxHealthIncrease;

    /// <summary>
    /// The percentage of the body's health that the death ward will revive it with
    /// </summary>
    private float reviveHealthPercentage;

    /// <summary>
    /// The amount of max health to remove from itself when it revives a body
    /// </summary>
    private int selfDamage;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/MageEnchanter/DeathWard/DeathWardEnchanter.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // adds the triggers
        TriggerManager.EnemyDeadTrigger.AddTrigger(OnEnemyDeath);
        TriggerManager.BodyDeadTrigger.AddTrigger(OnBodyDeath);
    }

    /// <summary>
    /// Called when an enemy dies, increases the max health of the death ward
    /// </summary>
    /// <param name="enemy">The enemy that died</param>
    /// <returns>The enemy that died</returns>
    private GameObject OnEnemyDeath(GameObject enemy)
    {
        body.healthBuff.AddBuff(maxHealthIncrease, false, 0);

        return enemy;
    }

    /// <summary>
    /// Called when a body dies, revives the body
    /// </summary>
    /// <param name="bodyController">The body that died</param>
    /// <returns>The body that died</returns>
    private BodyController OnBodyDeath(BodyController bodyController)
    {
        // if it is this object, ignore
        if (bodyController == body)
        {
            return bodyController;
        }

        // gets the HP that will be moved to the body
        int health = (int)(bodyController.MaxHealth * reviveHealthPercentage);

        // revives the body that died
        bodyController.Revived();

        // sets its hp back to 0
        bodyController.health = 0;

        // heals it to the right level
        bodyController.ChangeHealth(health);

        if (health < body.MaxHealth)
        {
            // reduces the maxHP of the death ward
            body.healthBuff.AddBuff(-health, false, 0);
        }
        else
        {
            // kills self soon after
            Invoke(nameof(body.KillBody), 0.05f);
        }

        return bodyController;
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // removes the triggers
        TriggerManager.EnemyDeadTrigger.RemoveTrigger(OnEnemyDeath);
        TriggerManager.BodyDeadTrigger.RemoveTrigger(OnBodyDeath);

        // destroys the body permananetly
        body.DestroySelf();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref maxHealthIncrease, nameof(maxHealthIncrease));
        jsonData.Setup(ref reviveHealthPercentage, nameof(reviveHealthPercentage));
    }
}

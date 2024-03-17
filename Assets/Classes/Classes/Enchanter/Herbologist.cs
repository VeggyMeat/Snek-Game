using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The herbologist class, a subclass of the enchanter class
/// </summary>
internal class Herbologist : Enchanter, IGroundTriggerManager
{
    /// <summary>
    /// The amount of health the healing orb heals
    /// </summary>
    private int healingOrbHealAmount;

    /// <summary>
    /// The delay inbetween spawning healing orbs
    /// </summary>
    private float healingOrbDelay;

    /// <summary>
    /// The path to the healing orb prefab
    /// </summary>
    private string healingOrbPath;

    /// <summary>
    /// The radius in which the healing orb can spawn of the body
    /// </summary>
    private float healingOrbRadius;

    /// <summary>
    /// The radius within the body that the healing orb will despawn
    /// </summary>
    private float healingOrbDespawnRange;

    /// <summary>
    /// How long the healing orb will last before despawning
    /// </summary>
    private float healingOrbLifeSpan;


    /// <summary>
    /// Whether the healing orbs are currently being spawned or not
    /// </summary>
    private bool summoning;

    /// <summary>
    /// The prefab for the healing orb
    /// </summary>
    private GameObject healingOrbPrefab;

    /// <summary>
    /// The position the item (healing orb) judges its distance from
    /// </summary>
    public Vector2 DespawnPosition
    {
        get
        {
            return body.snake.Head.transform.position;
        }
    }

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Herbologist.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called when the item (healing orb) collides with something
    /// </summary>
    /// <returns>Whether the item (healing orb) should destroy itself or not</returns>
    public bool OnCollision(Collider2D collision)
    {
        // if its not player tagged ignore it
        if (collision.gameObject.tag != "Player")
        {
            return false;
        }

        // finds all the bodies that arent at full hp
        List<BodyController> bodies = new List<BodyController>();

        // gets the head of the snake
        BodyController currentBody = body.snake.Head;

        // goes through all the bodies in the snake
        while (currentBody is not null)
        {
            // if the body is not at max HP add it to the list
            if (currentBody.MaxHealth > currentBody.health)
            {
                // and if its alive
                if (!currentBody.IsDead)
                {
                    bodies.Add(currentBody);
                }
            }

            // get the next body
            currentBody = currentBody.next;
        }

        // if there are bodies to heal
        if (bodies.Count > 0)
        {
            // gets a random body from the list
            BodyController healBody = bodies.RandomItem();

            // heal that body
            healBody.ChangeHealth(healingOrbHealAmount);
        }

        return true;
    }

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // gets the healingOrb gameObject
        healingOrbPrefab = Resources.Load<GameObject>(healingOrbPath);

        // starts summoning orbs
        StartSummoning();
    }

    /// <summary>
    /// Summons a new healing orb
    /// </summary>
    private void SummonHealingOrb()
    {
        // gets a random position within the healingOrbRadius of the player's transform
        Vector2 position = Random.insideUnitCircle * healingOrbRadius + (Vector2)body.snake.Head.transform.position;

        // creates a new healing orb
        GameObject newHealingOrb = Instantiate(healingOrbPrefab, position, Quaternion.identity);

        // sets up the healing orb
        newHealingOrb.GetComponent<GroundTrigger>().Setup(this, true, healingOrbDespawnRange, healingOrbLifeSpan);
    }

    /// <summary>
    /// Starts summoning healing orbs
    /// </summary>
    private void StartSummoning()
    {
        // if already summoning, ignore
        if (summoning)
        {
            return;
        }

        // start summoning and note that it is summoning
        InvokeRepeating(nameof(SummonHealingOrb), healingOrbDelay / body.attackSpeedBuff.Value, healingOrbDelay / body.attackSpeedBuff.Value);

        summoning = true;
    }

    /// <summary>
    /// Stops summoning healing orbs
    /// </summary>
    private void StopSummoning()
    {
        // if not summoning, ignore
        if (!summoning) 
        { 
            return; 
        }

        // stop summoning and note that it is not summoning
        CancelInvoke();

        summoning = false;
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // continues summoning orbs
        StartSummoning();
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        // stops summoning orbs
        StopSummoning();

        base.OnDeath();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // not allowed to change after initial setting
        jsonData.Setup(ref healingOrbPath, nameof(healingOrbPath));

        jsonData.Setup(ref healingOrbDespawnRange, nameof(healingOrbDespawnRange));
        jsonData.Setup(ref healingOrbLifeSpan, nameof(healingOrbLifeSpan));
        jsonData.Setup(ref healingOrbRadius, nameof(healingOrbRadius));
        jsonData.Setup(ref healingOrbHealAmount, nameof(healingOrbHealAmount));

        // updates the healing orb delay value, resetting the summoning if it changes
        jsonData.SetupAction(ref healingOrbDelay, nameof(healingOrbDelay), StopSummoning, StartSummoning, jsonLoaded);
    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // restarts the summoning of the healing orbs
        StopSummoning();
        StartSummoning();
    }
}

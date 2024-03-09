using System.Collections.Generic;

// COMPLETE

/// <summary>
/// The healer class, a subclass of the enchanter class
/// </summary>
internal class Healer : Enchanter
{
    /// <summary>
    /// Thedelay between each heal
    /// </summary>
    private int timeDelay;

    /// <summary>
    /// The amount of health to heal each time
    /// </summary>
    private int healthIncrease;

    /// <summary>
    /// Whether the class is currently healing or not
    /// </summary>
    private bool healing = false;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Healer.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // starts healing bodies
        StartHealing();
    }

    /// <summary>
    /// Heals a random ally that does not have maximum health
    /// </summary>
    private void HealRandomAlly()
    {
        // creates a list of healable bodies
        List<BodyController> healable = new List<BodyController>();

        // goes through each body in the snake and checks if its healable
        BodyController currentBody = body.snake.Head;
        while (currentBody is not null)
        {
            if (currentBody.health < currentBody.MaxHealth)
            {
                if (!currentBody.IsDead)
                {
                    healable.Add(currentBody);
                }
            }

            // gets the next body
            currentBody = currentBody.next;
        }

        // if something is healable, heal a random body that is healable
        if (healable.Count != 0)
        {
            HealAlly(healable.RandomItem());
        }
    }

    /// <summary>
    /// Heals a body
    /// </summary>
    /// <param name="healBody"></param>
    private void HealAlly(BodyController healBody)
    {
        // increase the body's health by healthIncrease
        healBody.ChangeHealth(healthIncrease);
    }

    /// <summary>
    /// Starts repeatedly calling the HealRandomAlly method
    /// </summary>
    private void StartHealing()
    {
        // if already healing, ignore
        if (healing)
        {
            return;
        }

        // regularly calls HealRandomAlly
        InvokeRepeating(nameof(HealRandomAlly), timeDelay / body.attackSpeedBuff.Value, timeDelay / body.attackSpeedBuff.Value);

        healing = true;
    }

    /// <summary>
    /// Stops repeatedly calling the HealRandomAlly method
    /// </summary>
    private void StopHealing()
    {
        // if not healing already, ignore
        if (!healing)
        {
            return;
        }

        // stops regularly calling HealRandomAlly
        CancelInvoke(nameof(HealRandomAlly));

        healing = false;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), StopHealing, StartHealing, jsonLoaded);
        jsonData.Setup(ref healthIncrease, nameof(healthIncrease));
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // stops regularly healing bodies
        StopHealing();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // continues regularly healing bodies
        StartHealing();
    }

    /// <summary>
    /// Called when the attack speed buff is changed
    /// </summary>
    /// <param name="amount">The amount changed (either multiplication or amount)</param>
    /// <param name="multiplicative">Whether the 'amount' is added or multiplied</param>
    internal override void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        base.OnAttackSpeedBuffUpdate(amount, multiplicative);

        // restarts the healing process
        StopHealing();
        StartHealing();
    }
}

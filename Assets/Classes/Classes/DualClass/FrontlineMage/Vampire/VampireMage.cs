using System.Collections.Generic;

/// <summary>
/// The vampire mage class, a subclass of the mage class
/// </summary>
internal class VampireMage : Mage
{
    /// <summary>
    /// The amount of health to heal the snake by
    /// </summary>
    private int healSnakeAmount;

    /// <summary>
    /// The amount to heal the vampire by when it sucks life
    /// </summary>
    private int healVampireAmount;

    /// <summary>
    /// The amount of damage to do to adjacent bodies when sucking life
    /// </summary>
    private int damageBodiesAmount;

    /// <summary>
    /// The delay between sucking health when below the limit
    /// </summary>
    private float suckHealthDelay;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/DualClass/FrontlineMage/Vampire/VampireMage.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        StartSuckingLife();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        jsonData.Setup(ref healSnakeAmount, nameof(healSnakeAmount));
        jsonData.Setup(ref healVampireAmount, nameof(healVampireAmount));
        jsonData.Setup(ref damageBodiesAmount, nameof(damageBodiesAmount));
        jsonData.SetupAction(ref suckHealthDelay, nameof(suckHealthDelay), StopSuckingLife, StartSuckingLife, jsonLoaded);
    }

    /// <summary>
    /// Called when the body takes damage, before the damage is applied
    /// </summary>
    /// <param name="amount">The damage taken</param>
    /// <returns>Returns the new damage value</returns>
    internal override int OnDamageTaken(int amount)
    {
        // heal the entire body a certain amount of hp
        BodyController healBody = body.snake.Head;

        while (healBody is not null)
        {
            // if the body is not the vampire
            if (healBody.Name != "Vampire")
            {
                // heal it
                if (!healBody.IsDead)
                {
                    healBody.ChangeHealth(healSnakeAmount);
                }
            }

            // get the next body in the snake
            healBody = healBody.next;
        }

        return base.OnDamageTaken(amount);
    }

    /// <summary>
    /// Called regularly to try suck life from adjacent bodies in the snake
    /// </summary>
    private void SuckLife()
    {
        // if the body is less than half health, such health from adjacent bodies
        if (body.health < body.MaxHealth / 2)
        {
            // if there is a snake after
            if (body.next is not null)
            {
                if (!body.IsDead)
                {
                    // deal damage to it
                    body.next.ChangeHealth(-damageBodiesAmount);

                    // heal the amount of health
                    body.ChangeHealth(healVampireAmount);
                }
            }

            // if there is a body before this one
            if (body.prev is not null)
            {
                if (!body.IsDead)
                {
                    // deal damage to it
                    body.prev.ChangeHealth(-damageBodiesAmount);

                    // heal the amount of health
                    body.ChangeHealth(healVampireAmount);
                }
            }
        }
    }

    /// <summary>
    /// Starts regularly calling the suck life method
    /// </summary>
    private void StartSuckingLife()
    {
        InvokeRepeating(nameof(SuckLife), suckHealthDelay, suckHealthDelay);
    }

    /// <summary>
    /// Stops regularly calling the suck life method
    /// </summary>
    private void StopSuckingLife()
    {
        CancelInvoke(nameof(SuckLife));
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        StartSuckingLife();
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        StopSuckingLife();
    }
}

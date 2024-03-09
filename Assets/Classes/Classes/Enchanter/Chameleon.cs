using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The chameleon class, a subclass of the enchanter class
/// </summary>
internal class Chameleon : Enchanter
{
    /// <summary>
    /// The list of classes that the chameleon can change into
    /// </summary>
    readonly List<string> Classes = new List<string>()
    {
        "Mage", "Frontline", "Enchanter", "Archer"
    };

    /// <summary>
    /// The list of different buffs that the chameleon can give
    /// </summary>
    readonly List<string> Buffs = new List<string>()
    {
        "Attack Speed", "Health", "Defence", "Attack Damage"
    };

    /// <summary>
    /// The multiplier for the attack speed buff
    /// </summary>
    private float attackSpeedMultiplier;

    /// <summary>
    /// The multiplier for the maxHealth of the bodies
    /// </summary>
    private float healthMultiplier;

    /// <summary>
    /// The multiplier for the defence of the bodies
    /// </summary>
    private float defenceMultiplier;

    /// <summary>
    /// The multiplier for the damage of the bodies
    /// </summary>
    private float damageMultiplier;

    
    /// <summary>
    /// The current buff the chameleon is applying
    /// </summary>
    private string currentBuff;

    /// <summary>
    /// The current class the chameleon is applying the buff to
    /// </summary>
    private string currentClass;


    /// <summary>
    /// The delay in time between buffing
    /// </summary>
    private float timeDelay;

    /// <summary>
    /// The length of time the buff lasts
    /// </summary>
    private float buffLength;

    /// <summary>
    /// Whether the chameleon keeps changing what it is buffing
    /// </summary>
    private bool changingType = false;

    /// <summary>
    /// Called before the body is set up, to set up the jsons
    /// </summary>
    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Chameleon.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Called by the body after it has been set up
    /// </summary>
    internal override void Setup()
    {
        base.Setup();

        // starts changing types
        StartChangingType();
    }

    /// <summary>
    /// Adds the buff to the body
    /// </summary>
    /// <param name="snakeObj">The GameObject of the body to buff</param>
    protected override void AddBuff(GameObject snakeObj)
    {
        BodyController snakeBody = snakeObj.GetComponent<BodyController>();

        // if its the current class
        if (snakeBody.classNames.Contains(currentClass))
        {
            // add the stat corresponding to the currentBuff
            switch (currentBuff)
            {
                // gives the buff to the respective effect for buffLength seconds
                case "Attack Speed":
                    snakeBody.attackSpeedBuff.AddBuff(attackSpeedMultiplier, true, buffLength);
                    break;
                case "Health":
                    snakeBody.healthBuff.AddBuff(healthMultiplier, true, buffLength);
                    break;
                case "Attack Damage":
                    snakeBody.damageBuff.AddBuff(damageMultiplier, true, buffLength);
                    break;
                case "Defence":
                    snakeBody.defenceBuff.AddBuff(defenceMultiplier, true, buffLength);
                    break;
            }
        }
    }

    /// <summary>
    /// Starts changing the type of the chameleon
    /// </summary>
    private void StartChangingType()
    {
        // if already changing type, ignore
        if (changingType)
        {
            return;
        }

        // start changing type, and note that it is changing type
        InvokeRepeating(nameof(ChangeChameleonType), timeDelay, timeDelay);

        changingType = true;
    }

    /// <summary>
    /// Stops changing the type of the chameleon
    /// </summary>
    private void StopChangingType()
    {
        // if not changing type already, ignore
        if (!changingType)
        {
            return;
        }

        // stop changing type, and note that it is not changing type
        CancelInvoke(nameof(ChangeChameleonType));

        changingType = false;
    }

    /// <summary>
    /// Changes the chameleon's type to a random class and buff
    /// </summary>
    private void ChangeChameleonType()
    {
        // gets the next set of buffs
        currentBuff = Buffs.RandomItem();
        currentClass = Classes.RandomItem();

        // adds the new buffs
        BuffAllBodies();
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        // stops changing type
        StopChangingType();
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        // resumes changing type
        StartChangingType();
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // sets up the variables
        jsonData.Setup(ref buffLength, nameof(buffLength));
        jsonData.Setup(ref damageMultiplier, nameof(damageMultiplier));
        jsonData.Setup(ref attackSpeedMultiplier, nameof(attackSpeedMultiplier));
        jsonData.Setup(ref healthMultiplier, nameof(healthMultiplier));
        jsonData.Setup(ref defenceMultiplier, nameof(defenceMultiplier));

        jsonData.SetupAction(ref timeDelay, nameof(timeDelay), StopChangingType, StartChangingType, jsonLoaded);
    }
}

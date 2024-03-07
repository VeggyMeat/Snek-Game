using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chamelion : Enchanter
{
    readonly List<string> Classes = new List<string>()
    {
        "Mage", "Frontline", "Enchanter", "Archer"
    };

    readonly List<string> Buffs = new List<string>()
    {
        "Attack Speed", "Health", "Defence", "Attack Damage"
    };

    private float attackSpeedMultiplier;
    private float healthMultiplier;
    private float defenceMultiplier;
    private float damageMultiplier;

    private string currentBuff;
    private string currentClass;

    private float timeDelay;
    private float buffLength;

    private bool changingType = false;

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Chameleon.json";

        base.ClassSetup();
    }

    internal override void Setup()
    {
        base.Setup();

        // starts changing types
        StartChangingType();
    }

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

    // irrespective of attack speed buff
    private void StartChangingType()
    {
        // if already changing type, ignore
        if (changingType)
        {
            return;
        }

        // start changing type, and note that it is changing type
        InvokeRepeating(nameof(ChangeChamelionType), timeDelay, timeDelay);

        changingType = true;
    }

    private void StopChangingType()
    {
        // if not changing type already, ignore
        if (!changingType)
        {
            return;
        }

        // stop changing type, and note that it is not changing type
        CancelInvoke(nameof(ChangeChamelionType));

        changingType = false;
    }

    private void ChangeChamelionType()
    {
        // gets the next set of buffs
        currentBuff = Buffs.RandomItem();
        currentClass = Classes.RandomItem();

        // adds the new buffs
        BuffAllBodies();
    }

    internal override void OnDeath()
    {
        base.OnDeath();

        // stops changing type
        StopChangingType();
    }

    internal override void Revived()
    {
        base.Revived();

        // resumes changing type
        StartChangingType();
    }

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

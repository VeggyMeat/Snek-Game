using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chamelion : Enchanter
{
    readonly List<string> Classes = new List<string>()
    {
        "Frontline", "Enchanter", "Archer", "Mage"
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

    internal override void ClassSetup()
    {
        jsonPath = "Assets/Resources/Jsons/Classes/Enchanter/Chamelion.json";

        base.ClassSetup();
    }

    /// <summary>
    /// Runs the BuffBody function on every body in the snake
    /// </summary>
    private void BuffAllBodies()
    {
        BodyController snakeBody = body.snake.head;

        while (snakeBody is not null)
        {
            // buff the body
            BuffBody(snakeBody);

            // grabs the next one
            snakeBody = snakeBody.next;
        }
    }

    private void BuffBody(BodyController snakeBody)
    {
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
        InvokeRepeating(nameof(ChangeChamelionType), timeDelay, timeDelay);
    }

    private void StopChangingType()
    {
        CancelInvoke(nameof(ChangeChamelionType));
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

        if (jsonData.ContainsKey(nameof(timeDelay)))
        {
            // if the json has already been loaded, then stop the current changing type
            if (jsonLoaded)
            {
                StopChangingType();
            }

            timeDelay = float.Parse(jsonData[nameof(timeDelay)].ToString());

            // starts chaning type
            StartChangingType();
        }
    }
}

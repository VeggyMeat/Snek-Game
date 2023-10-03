using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public abstract class Enchanter : Class
{
    protected bool buffsAllBodies = false;

    internal override void Setup()
    {
        body.classNames.Add("Enchanter");

        base.Setup();
    }

    /// <summary>
    /// Buffs all currently alive bodies
    /// </summary>
    /// <param name="first">whether to buff itself or not</param>
    protected virtual void BuffAllBodies(bool first = false)
    {
        // gets the head
        BodyController bodyBuffed = body.snake.head;

        while (bodyBuffed is not null)
        {
            // makes sure the body doesn't get buffed twice initially
            if (first)
            {
                if (bodyBuffed.Name == body.Name)
                {
                    // gets the next body
                    bodyBuffed = bodyBuffed.next;

                    continue;
                }
            }

            // adds the buff to the body
            AddBuff(bodyBuffed.gameObject);

            // gets the next body
            bodyBuffed = bodyBuffed.next;
        }
    }

    /// <summary>
    /// Unbuffs all currently alive bodies
    /// </summary>
    protected virtual void UnbuffAllBodies()
    {
        // gets the head
        BodyController bodyBuffed = body.snake.head;

        while (bodyBuffed is not null)
        {
            // remove the buff from the body
            RemoveBuff(bodyBuffed.gameObject);

            // gets the next body
            bodyBuffed = bodyBuffed.next;
        }
    }

    /// <summary>
    /// Adds the buff to a GameObject
    /// </summary>
    /// <param name="thing">The GameObject</param>
    /// <exception cref="NotImplementedException"></exception>
    internal virtual void AddBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the buff from a GameObject
    /// </summary>
    /// <param name="thing">The GameObject</param>
    /// <exception cref="NotImplementedException"></exception>
    internal virtual void RemoveBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    internal override void OnDeath()
    {
        base.OnDeath();

        if (buffsAllBodies)
        {
            // removes buffs from all the bodies
            UnbuffAllBodies();
        }
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    internal override void Revived()
    {
        base.Revived();

        if (buffsAllBodies)
        {
            // buffs all the bodies
            BuffAllBodies();
        }
    }

    /// <summary>
    /// Called when a new body is added, if the buffAllBodies is true
    /// </summary>
    /// <param name="newBody">The new body</param>
    /// <returns></returns>
    internal BodyController NewBodyTrigger(BodyController newBody)
    {
        AddBuff(newBody.gameObject);

        return newBody;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        if (jsonData.ContainsKey("buffsAllBodies"))
        {
            bool oldBuffsAllBodies = buffsAllBodies;
            buffsAllBodies = bool.Parse(jsonData["buffsAllBodies"].ToString());

            // if it used to buff them, but now it doesnt
            if (oldBuffsAllBodies && !buffsAllBodies) 
            { 
                // removes the trigger
                TriggerManager.BodySpawnTrigger.RemoveTrigger(NewBodyTrigger);

                // unbuffs all the bodies
                UnbuffAllBodies();
            }
            // if it is just now buffing them
            else if (!oldBuffsAllBodies && buffsAllBodies)
            {
                // adds the trigger
                TriggerManager.BodySpawnTrigger.AddTrigger(NewBodyTrigger);

                // buffs all the bodies
                BuffAllBodies(true);
            }
        }
    }
}

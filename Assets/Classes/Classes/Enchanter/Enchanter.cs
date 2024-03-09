using System;
using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The base class for all enchanter classes
/// </summary>
internal abstract class Enchanter : Class
{
    /// <summary>
    /// Whether the enchanter should buff all bodies
    /// </summary>
    protected bool buffsAllBodies = false;

    /// <summary>
    /// Called when the body is setup
    /// </summary>
    internal override void Setup()
    {
        // adds the enchanter class name to the body's classes for identification
        body.classNames.Add(nameof(Enchanter));

        base.Setup();

        // if it buffs all bodies
        if (buffsAllBodies) 
        {
            // buff all bodies
            BuffAllBodies(true);

            // adds a trigger to buff the new body when a new body is spawned
            TriggerManager.BodySpawnTrigger.AddTrigger(NewBodyTrigger);
        }
    }

    /// <summary>
    /// Buffs all currently alive bodies
    /// </summary>
    /// <param name="first">Whether to buff itself or not</param>
    private void BuffAllBodies(bool first)
    {
        // gets the head
        BodyController bodyBuffed = body.snake.Head;

        // goes through each buff and buffs it
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
    /// Buffs all currently alive bodies
    /// </summary>
    protected void BuffAllBodies()
    {
        BuffAllBodies(false);
    }

    /// <summary>
    /// Unbuffs all currently alive bodies
    /// </summary>
    protected virtual void UnbuffAllBodies()
    {
        // gets the head
        BodyController bodyBuffed = body.snake.Head;

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
    /// <exception cref="NotImplementedException">Called when the child class does not override</exception>
    protected virtual void AddBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the buff from a GameObject
    /// </summary>
    /// <param name="thing">The GameObject</param>
    /// <exception cref="NotImplementedException">Called when the child class does not override</exception>
    protected virtual void RemoveBuff(GameObject thing)
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

            // removes the trigger
            TriggerManager.BodySpawnTrigger.RemoveTrigger(NewBodyTrigger);
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

            // adds the trigger back
            TriggerManager.BodySpawnTrigger.AddTrigger(NewBodyTrigger);
        }
    }

    /// <summary>
    /// Called when a new body is added, if the buffAllBodies is true
    /// </summary>
    /// <param name="newBody">The new body</param>
    /// <returns>The new body</returns>
    private BodyController NewBodyTrigger(BodyController newBody)
    {
        // buffs the body
        AddBuff(newBody.gameObject);

        return newBody;
    }

    /// <summary>
    /// Overwrites the class's variables based on the data from the json
    /// </summary>
    /// <param name="jsonData">The jsonData to load data off of</param>
    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // cannot be changed after initialisation
        jsonData.Setup(ref buffsAllBodies, nameof(buffsAllBodies));
    }
}

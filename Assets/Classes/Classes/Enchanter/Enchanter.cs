using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using static UnityEditor.Progress;

public abstract class Enchanter : Class
{
    protected bool buffsAllBodies = false;

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
    /// <param name="first">whether to buff itself or not</param>
    protected virtual void BuffAllBodies(bool first)
    {
        // gets the head
        BodyController bodyBuffed = body.snake.Head;

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
    protected virtual void BuffAllBodies()
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
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void AddBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the buff from a GameObject
    /// </summary>
    /// <param name="thing">The GameObject</param>
    /// <exception cref="NotImplementedException"></exception>
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
    /// <returns></returns>
    internal BodyController NewBodyTrigger(BodyController newBody)
    {
        // buffs the body
        AddBuff(newBody.gameObject);

        return newBody;
    }

    protected override void InternalJsonSetup(Dictionary<string, object> jsonData)
    {
        base.InternalJsonSetup(jsonData);

        // cant be changed after initialisation
        jsonData.Setup(ref buffsAllBodies, nameof(buffsAllBodies));
    }
}

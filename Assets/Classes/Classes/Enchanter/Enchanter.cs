using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Enchanter : Class
{
    protected bool buffsAllBodies;

    internal override void Setup()
    {
        body.classNames.Add("Enchnatment");

        base.Setup();

        // buffs all of the bodies and adds the trigger if buffsAllBodies is true
        if (buffsAllBodies)
        {
            BuffAllBodies(true);

            TriggerManager.BodySpawnTrigger.AddTrigger(NewBodyTrigger);
        }
    }

    internal virtual void BuffAllBodies(bool first = false)
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

    internal virtual void UnbuffAllBodies()
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

    internal virtual void AddBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    internal virtual void RemoveBuff(GameObject thing)
    {
        throw new NotImplementedException();
    }

    // called when the body dies
    internal override void OnDeath()
    {
        base.OnDeath();

        if (buffsAllBodies)
        {
            // removes buffs from all the bodies
            UnbuffAllBodies();
        }
    }

    // calls when the body is revived
    internal override void Revived()
    {
        base.Revived();

        if (buffsAllBodies)
        {
            // buffs all the bodies
            BuffAllBodies();
        }
    }

    // called when a new body is added, if the buffAllBodies is true
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
            if (jsonData["buffsAllBodies"].ToString() == "True")
            {
                buffsAllBodies = true;

                if (jsonLoaded)
                {
                    BuffAllBodies();
                }
            }
            else
            {
                buffsAllBodies = false;

                if (jsonLoaded)
                {
                    UnbuffAllBodies();
                }
            }
        }
    }
}

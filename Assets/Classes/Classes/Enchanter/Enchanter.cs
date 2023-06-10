using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enchanter : Class
{
    public bool buffsAllBodies;

    internal override void Setup()
    {
        className = "Enchnatment";

        base.Setup();

        // buffs all of the bodies and adds the trigger if buffsAllBodies is true
        if (buffsAllBodies)
        {
            BuffAllBodies();

            TriggerManager.BodySpawnTrigger.AddTrigger(NewBodyTrigger);
        }
    }

    internal virtual void BuffAllBodies()
    {
        // gets the head
        BodyController body = snake.head;

        while (body is not null)
        {
            // adds the buff to the body
            AddBuff(body.gameObject);

            // gets the next body
            body = body.next;
        }
    }

    internal virtual void UnbuffAllBodies()
    {
        // gets the head
        BodyController body = snake.head;

        while (body is not null)
        {
            // remove the buff from the body
            RemoveBuff(body.gameObject);

            // gets the next body
            body = body.next;
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
    internal BodyController NewBodyTrigger(BodyController body)
    {
        AddBuff(body.gameObject);

        return body;
    }
}

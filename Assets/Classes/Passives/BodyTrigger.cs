using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger<T1>
{
    // a delegate type describing the signature of the trigger function
    public delegate T1 Trigger(T1 change);

    // a list of all the trigger functions
    private List<Trigger> triggerCalls = new List<Trigger>();

    // adds a new trigger function to the list
    public void AddTrigger(Trigger triggerFunction, bool beginning=false)
    {
        // if it needs to be at the beginning, adds it there
        if (beginning)
        {
            triggerCalls.Insert(0, triggerFunction);
            return;
        }

        // otherwise adds it to the end
        triggerCalls.Add(triggerFunction);
    }

    // removes a trigger function from the list
    public void RemoveTrigger(Trigger triggerFunction)
    {
        triggerCalls.Remove(triggerFunction);
    }

    // calls the trigger functions with that value
    public void CallTrigger(T1 value)
    {
        // goes through each trigger function and calls it with the same parameter
        foreach (Trigger trigger in triggerCalls)
        {
            trigger(value);
        }
    }

    // calls the trigger, but returns the values from the things triggered
    public T1 CallTriggerReturn(T1 value)
    {
        // goes through each trigger function and calls it with the same parameter, overiding the value
        foreach (Trigger trigger in triggerCalls)
        {
            value = trigger(value);
        }

        return value;
    }
}
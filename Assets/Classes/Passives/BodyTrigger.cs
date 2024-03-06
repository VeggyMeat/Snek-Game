using System;
using System.Collections.Generic;

// COMPLETE

/// <summary>
/// A class that allows for triggering of functions based on a change in some condition
/// </summary>
/// <typeparam name="T1">The type of value passed by the functions</typeparam>
public class BodyTrigger<T1>
{
    /// <summary>
    /// A delegate type describing the signature of the trigger function
    /// </summary>
    /// <param name="change">Some input value</param>
    /// <returns>The same value as the input (or changed to change it)</returns>
    public delegate T1 Trigger(T1 change);

    /// <summary>
    /// The list of all the functions to call when the trigger is called
    /// </summary>
    private readonly List<Trigger> triggerCalls = new List<Trigger>();

    /// <summary>
    /// Adds a new trigger function to the list
    /// </summary>
    /// <param name="triggerFunction">The function that will be called</param>
    public void AddTrigger(Trigger triggerFunction)
    {
        triggerCalls.Add(triggerFunction);
    }

    /// <summary>
    /// Removes a trigger function from the list
    /// </summary>
    /// <param name="triggerFunction">The function to be removed</param>
    /// <exception cref="Exception">Throws an exception if the trigger function is not in the list of triggers</exception>
    public void RemoveTrigger(Trigger triggerFunction)
    {
        // if the trigger function is not in the list, throw an exception
        if (!triggerCalls.Contains(triggerFunction))
        {
            throw new Exception("The trigger function was not found in the list of triggers");
        }

        // removes the item from the list
        triggerCalls.Remove(triggerFunction);    
    }

    /// <summary>
    /// Calls the trigger functions with a perticular value
    /// </summary>
    /// <param name="value">The value to be passed</param>
    public void CallTrigger(T1 value)
    {
        // goes through each trigger function and calls it with the same parameter
        foreach (Trigger trigger in triggerCalls)
        {
            trigger(value);
        }
    }

    /// <summary>
    /// Calls the trigger, but returns the values from the things triggered
    /// </summary>
    /// <param name="value">The value to be passed</param>
    /// <returns>The value returned from the functions</returns>
    public T1 CallTriggerReturn(T1 value)
    {
        // goes through each trigger function and calls it with the same parameter, overriding the value each time
        foreach (Trigger trigger in triggerCalls)
        {
            value = trigger(value);
        }

        return value;
    }
}
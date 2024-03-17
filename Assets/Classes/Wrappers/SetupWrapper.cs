using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// COMPLETE

/// <summary>
/// Handles extra setup operations on variables
/// </summary>
public static class SetupWrapper
{
    /// <summary>
    /// Sets up a variable from a dictionary
    /// </summary>
    /// <typeparam name="T">The type of the variable</typeparam>
    /// <param name="dict">The dictionary with the variable's data</param>
    /// <param name="variable">The variable</param>
    /// <param name="name">The variable's name in the dictionary</param>
    public static void Setup<T>(this Dictionary<string, object> dict, ref T variable, string name) where T : IConvertible
    {
        if (dict.ContainsKey(name))
        {
            variable = (T)Convert.ChangeType(dict[name], typeof(T));
        }
    }

    /// <summary>
    /// Sets up a variable from a dictionary
    /// </summary>
    /// <param name="dict">The dictionary with the variable's data</param>
    /// <param name="variable">The variables</param>
    /// <param name="name">The variable's name in the dictionary</param>
    /// <exception cref="Exception">Called if the data in the dictionary does not match a colour with the number of items</exception>
    public static void Setup(this Dictionary<string, object> dict, ref Color variable, string name)
    {
        if (dict.ContainsKey(name))
        {
            string text = dict[name].ToString();

            // get rid of open and close brackets
            text = text.Replace("[", "");
            text = text.Replace("]", "");

            // get rid of all whitespace
            text = text.Replace(" ", "");

            // split by ',' seperator
            string[] split = text.Split(',');

            // if the array is not 3 or 4 items long
            if (split.Count() == 3)
            {
                variable = new Color(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            }
            else if (split.Count() == 4)
            {
                variable = new Color(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
            }
            else
            {
                throw new Exception("Not a valid length for a colour");
            }

        }
    }

    /// <summary>
    /// Sets up a variable from a dictionary, with an action before and after if a condition is met
    /// </summary>
    /// <typeparam name="T">The type of the variable</typeparam>
    /// <param name="dict">The dictionary with the variable's data</param>
    /// <param name="variable">The variable</param>
    /// <param name="name">The variable's name in the dictionary</param>
    /// <param name="actionBefore">The action to do before loading in the data if 'doAction' is true</param>
    /// <param name="actionAfter">The action to do after loading in the data if 'doAction' is true</param>
    /// <param name="doAction">Whether to do the previous two actions or not</param>
    public static void SetupAction<T>(this Dictionary<string, object> dict, ref T variable, string name, Action actionBefore, Action actionAfter, bool doAction)
    {
        if (dict.ContainsKey(name))
        {
            if (actionBefore is not null)
            {
                if (doAction)
                {
                    actionBefore();
                }
            }

            variable = (T)Convert.ChangeType(dict[name], typeof(T));

            if (actionAfter is not null)
            {
                if (doAction)
                {
                    actionAfter();
                }
            }
        }
    }
}

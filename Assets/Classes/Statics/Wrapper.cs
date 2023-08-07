using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Wrapper
{
    public static void Setup<T>(this Dictionary<string, object> dict, ref T variable, string name) where T : IConvertible
    {
        // if its in the dictionary
        if (dict.ContainsKey(name))
        {
            // then set the variable to the value in the dictionary (type cast it)
            variable = (T)Convert.ChangeType(dict[name], typeof(T));
        }
    }

    /// <summary>
    /// Returns a random item from the list
    /// </summary>
    /// <typeparam name="T">The type of class in the list</typeparam>
    /// <param name="ls">The list the item gets chosen from</param>
    /// <returns></returns>
    public static T RandomItem<T>(this List<T> ls)
    {
        return ls[UnityEngine.Random.Range(0, ls.Count - 1)];
    }
}

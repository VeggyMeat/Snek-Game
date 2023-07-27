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
}

using System.Collections.Generic;

// COMPLETE

/// <summary>
/// Handles extra random operations
/// </summary>
public static class RandomWrapper
{
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

    /// <summary>
    /// Shuffles the list
    /// </summary>
    /// <typeparam name="T">The type of the list to shuffle</typeparam>
    /// <param name="ls">The list of items</param>
    public static void Shuffle<T>(this List<T> ls)
    {
        List<T> newList = new List<T>();

        // keeps adding random items from the list to the new list until the old list is empty
        while (ls.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, ls.Count);
            newList.Add(ls[index]);
            ls.RemoveAt(index);
        }

        ls.AddRange(newList);
    }
}

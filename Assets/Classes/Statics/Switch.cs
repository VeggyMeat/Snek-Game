using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Switch
{
    public static void Item<T>(ref T item1, ref T item2)
    {
        (item2, item1) = (item1, item2);
    }
}

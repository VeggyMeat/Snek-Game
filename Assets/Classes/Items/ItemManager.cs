using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    internal static List<Item> items = new List<Item>();
    internal static HeadController headController;

    internal static void Setup(HeadController headController)
    {
        ItemManager.headController = headController;
    }

    internal static void AddItem(Item item)
    {
        item.Setup();
        items.Add(item);
    }
}

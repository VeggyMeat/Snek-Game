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

    internal static void AddItem(string itemName)
    {
        Item item;

        switch (itemName)
        {
            case "Quiver":
                item = new Quiver();
                break;
            case "LuckyFlask":
                item = new LuckyFlask();
                break;
            case "SteeringWheel":
                item = new SteeringWheel();
                break;
            default:
                // crashes if its been called with an item that doesn't exist
                throw new System.Exception();
        }

        item.Setup();
        items.Add(item);
    }
}

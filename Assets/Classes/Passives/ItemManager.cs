using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    internal List<Item> items = new List<Item>();

    internal void AddItem(Item item)
    {
        item.Setup();
        items.Add(item);
    }

    public void BodyAdded(Class body)
    {

    }
}

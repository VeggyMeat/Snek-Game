using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ShopManagerTests
{
    private GameObject ShopManager
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/ShopManager.prefab");
        }
    }

    [Test]
    public void TestRemoveBody()
    {
        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        int bodiesCount = shopManager.Bodies.Count;
        List<string> oldBodies = new List<string>(shopManager.Bodies);

        shopManager.RemoveBody(oldBodies[1]);

        Assert.AreEqual(bodiesCount - 1, shopManager.Bodies.Count);
        Assert.AreEqual(oldBodies[0], shopManager.Bodies[0]);
        Assert.AreEqual(oldBodies[2], shopManager.Bodies[1]);
    }

    [Test]
    public void TestRemoveItem()
    {
        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        int itemsCount = shopManager.Items.Count;
        List<string> oldItems = new List<string>(shopManager.Items);

        shopManager.RemoveItem(oldItems[1]);

        Assert.AreEqual(itemsCount - 1, shopManager.Items.Count);
        Assert.AreEqual(oldItems[0], shopManager.Items[0]);
        Assert.AreEqual(oldItems[2], shopManager.Items[1]);
    }
}

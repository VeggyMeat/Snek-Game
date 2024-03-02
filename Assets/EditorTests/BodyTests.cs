using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class BodyTests
{
    private GameObject HeadController
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/Player.prefab");
        }
    }

    /*[Test]
    public void TestHeadCreation()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        headController.ShopManager = Object.Instantiate(ShopManager).GetComponent<ShopManager>();

        Assert.IsTrue(headController != null);
    }

    [Test]
    public void TestBodyCreations()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        headController.ShopManager = shopManager;

        List<string> bodies = new List<string>(shopManager.bodies);

        foreach (string body in bodies)
        {
            headController.AddBody(body);
        }
    }

    [Test]
    public void TestHeadPosition()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        headController.ShopManager = shopManager;

        // creates a body attatched to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.head.transform.position = randomLocation;

        // checks if the head is at the position
        Assert.AreEqual(randomLocation, headController.HeadPos);

        // adds another body to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // checks if the head is at the position
        Assert.AreEqual(randomLocation, headController.HeadPos);
    }

    [Test]
    public void TestTailPosition()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        headController.ShopManager = shopManager;

        // creates a body attatched to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation1 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.head.transform.position = randomLocation1;

        // checks if the tail is at the position
        Assert.AreEqual(randomLocation1, headController.TailPos);

        // adds another body to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation2 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the new body to that position
        headController.head.next.transform.position = randomLocation2;

        // checks if the tail is at the position
        Assert.AreEqual(randomLocation2, headController.TailPos);
    }

    [Test]
    public void TestSnakeLength()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        headController.ShopManager = shopManager;

        for (int i = 0; i < 10; i++)
        {
            headController.AddBody(shopManager.bodies.RandomItem());

            Assert.AreEqual(i + 1, headController.Length);
        }
    }

    [Test]
    public void TestDeathCheck()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        HeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        ShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        headController.ShopManager = shopManager;

        Assert.AreEqual(false, headController.DeathCheck);

        headController.AddBody(shopManager.bodies.RandomItem());

        Assert.AreEqual(false, headController.DeathCheck);

        headController.head.ChangeHealth(-1000);

        Assert.AreEqual(true, headController.DeathCheck);
    }*/
}

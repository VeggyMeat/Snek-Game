using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using NSubstitute;
using System.Text;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BodyTests
{
    private GameObject HeadController
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/Player.prefab");
        }
    }

    private GameObject ShopManager
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/ShopManager.prefab");
        }
    }

    [Test]
    public void TestHeadCreation()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        IShopManager shopManager = Substitute.For<IShopManager>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        Assert.IsTrue(headController is not null);
    }

    [Test]
    public void TestBodyCreations()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        List<string> bodies = new List<string>(shopManager.Bodies);

        foreach (string body in bodies)
        {
            try
            {
                headController.AddBody(body);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add body {body} to the snake", e);
            }
        }
    }

    [Test]
    public void TestHeadPosition()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        headController.AddBody(shopManager.Bodies.RandomItem());

        Vector2 randomLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        headController.Head.transform.position = randomLocation;

        Assert.AreEqual(randomLocation, headController.HeadPos);

        headController.AddBody(shopManager.Bodies.RandomItem());

        Assert.AreEqual(randomLocation, headController.HeadPos);
    }

    [Test]
    public void TestTailPosition()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        // creates a body attatched to the snake
        headController.AddBody(shopManager.Bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation1 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.Head.transform.position = randomLocation1;

        // checks if the tail is at the position
        Assert.AreEqual(randomLocation1, headController.TailPos);

        // adds another body to the snake
        headController.AddBody(shopManager.Bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation2 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the new body to that position
        headController.Head.next.transform.position = randomLocation2;

        // checks if the tail is at the position
        Assert.AreEqual(randomLocation2, headController.TailPos);
    }

    [Test]
    public void TestSnakeLength()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        for (int i = 0; i < 10; i++)
        {
            headController.AddBody(shopManager.Bodies.RandomItem());

            Assert.AreEqual(i + 1, headController.Length);
        }
    }

    [Test]
    public void TestCurrentBodies()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        List<string> bodyList = new List<string>(shopManager.Bodies);

        foreach (string body in bodyList)
        {
            headController.AddBody(body);

            string name = headController.CurrentBodies[headController.CurrentBodies.Count - 1];

            Assert.AreEqual(body, name);
        }
    }

    [Test]
    public void TestAliveBodies()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);
        gameSetup.DeathScreenController.Returns(Substitute.For<IDeathScreen>());

        headController.SetGameSetup(gameSetup);

        const int bodyCount = 10;

        for (int i = 0; i < bodyCount; i++)
        {
            headController.AddBody("BowMan");

            Assert.AreEqual(i + 1, headController.AliveBodies);
        }

        headController.Head.ChangeHealth(-1000);

        Assert.AreEqual(bodyCount - 1, headController.AliveBodies);
    }

    [Test]
    public void TestBodiesNames()
    {
        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        List<string> bodyList = new List<string>(shopManager.Bodies);

        foreach (string body in bodyList)
        {
            GameObject headObject = Object.Instantiate(HeadController);
            IHeadController headController = headObject.GetComponent<HeadController>();

            GameObject newShopManagerObject = Object.Instantiate(ShopManager);
            IShopManager newShopManager = newShopManagerObject.GetComponent<ShopManager>();

            IGameSetup gameSetup = Substitute.For<IGameSetup>();
            gameSetup.ShopManager.Returns(newShopManager);

            headController.SetGameSetup(gameSetup);

            headController.AddBody(body);

            Assert.AreEqual(body, headController.Head.Name);
        }
    }

    [Test]
    public void TestRearrange()
    {
        GameObject headObject = Object.Instantiate(HeadController);
        IHeadController headController = headObject.GetComponent<HeadController>();

        GameObject shopManagerObject = Object.Instantiate(ShopManager);
        IShopManager shopManager = shopManagerObject.GetComponent<ShopManager>();

        IGameSetup gameSetup = Substitute.For<IGameSetup>();
        gameSetup.ShopManager.Returns(shopManager);

        headController.SetGameSetup(gameSetup);

        List<string> bodies = new List<string>();

        for (int i = 0; i < 10; i++)
        {
            string body = shopManager.Bodies.RandomItem();
            headController.AddBody(body);
            bodies.Add(body);
        }

        // shuffle bodies
        bodies.Shuffle();

        List<BodyController> bodyControllers = new List<BodyController>();

        foreach(string body in bodies)
        {
            BodyController bodyController = headController.Head;
            try
            {
                while (bodyController.Name != body)
                {
                    bodyController = bodyController.next;
                }
                bodyControllers.Add(bodyController);
            }
            catch
            {
                Assert.Fail("Body was not found inside of list");
            }
        }

        headController.Rearrange(bodyControllers);

        BodyController currentBody = headController.Head;
        int index = 0;
        while (currentBody is not null)
        {
            Assert.IsTrue(currentBody == bodyControllers[index]);
            index++;
            currentBody = currentBody.next;
        }
    }
}

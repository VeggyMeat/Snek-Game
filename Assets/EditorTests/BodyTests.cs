using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using NSubstitute;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BodyTests
{
    /// <summary>
    /// The head controller prefab used in the game
    /// </summary>
    private GameObject HeadController
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/Player.prefab");
        }
    }

    /// <summary>
    /// The shop manager prefab used in the game
    /// </summary>
    private GameObject ShopManager
    {
        get
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Scene/ShopManager.prefab");
        }
    }

    /// <summary>
    /// Tests that creating a head controller works and does not crash
    /// </summary>
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

    /// <summary>
    /// Tests that telling the head to add every body in the snake works, making sure all body classes do not crash
    /// </summary>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Tests that the head position property is correct
    /// </summary>
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

    /// <summary>
    /// Tests that the tail position property is correct
    /// </summary>
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

        headController.AddBody(shopManager.Bodies.RandomItem());

        Vector2 randomLocation1 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        headController.Head.transform.position = randomLocation1;

        Assert.AreEqual(randomLocation1, headController.TailPos);

        headController.AddBody(shopManager.Bodies.RandomItem());

        Vector2 randomLocation2 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        headController.Head.next.transform.position = randomLocation2;

        Assert.AreEqual(randomLocation2, headController.TailPos);
    }

    /// <summary>
    /// Tests that the length property is correct
    /// </summary>
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

    /// <summary>
    /// Tests that the current bodies property is correct
    /// </summary>
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

    /// <summary>
    /// Tests that the alive bodies property is correct
    /// </summary>
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

    /// <summary>
    /// Tests that every body class's name property is correct in their json files
    /// </summary>
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

    /// <summary>
    /// Tests that the rearranging method for the snake works
    /// </summary>
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

    /// <summary>
    /// Tests that leveling every body class up to max level does not crash anything
    /// </summary>
    [Test]
    public void TestAllLevelling()
    {
        // all bodies go to level 5, other than ClockworkMagician which does not level
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

            if (headController.Head.Name == "ClockworkMagician")
            {
                try
                {
                    // should throw an exception
                    headController.Head.LevelUp();
                    Assert.Fail("ClockworkMagician should not level up and should throw an exception");
                }
                catch
                {
                    // expected
                }
            }
            else
            {
                headController.AddBody(body);
                try
                {
                    for (int i = 1; i < 5; i++)
                    {
                        Assert.AreEqual(i, headController.Head.Level);
                        headController.Head.LevelUp();
                    }
                    Assert.AreEqual(5, headController.Head.Level);
                }
                catch (Exception e)
                {
                    Assert.Fail($"Failed to level up body {body} \n {e}");
                }
            }
        }
    }

    /// <summary>
    /// Tests that adding every body to the same snake, and levelling all of them up to max level does not crash anything
    /// </summary>
    [Test]
    public void TestLargeSnakeLevelling()
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
            headController.AddBody(body);
        }

        for (int i = 1; i < 5; i++)
        {
            BodyController currentBody = headController.Head;
            while (currentBody is not null)
            {
                try
                {
                    if (currentBody.Name != "ClockworkMagician")
                    {
                        currentBody.LevelUp();
                    }
                    currentBody = currentBody.next;
                }
                catch (Exception e)
                {
                    Assert.Fail($"Failed to level up body {currentBody.Name} to level {i + 1} \n {e}");
                }
            }
        }
    }
}

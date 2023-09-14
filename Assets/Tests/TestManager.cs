using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestManager: MonoBehaviour
{
    private List<Func<string>> tests;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemySummonerPrefab;
    [SerializeField] GameObject shopManagerPrefab;
    [SerializeField] GameObject cameraPrefab;
    [SerializeField] GameObject uIPrefab;
    [SerializeField] GameObject uIControllerPrefab;
    [SerializeField] GameObject optionChoicesPrefab;
    [SerializeField] GameObject selectOptionsPrefab;
    [SerializeField] bool runTests = false;

    void Start()
    {
        // stops time
        Time.timeScale = 0;

        // only runs the tests if told to
        if (!runTests)
        {
            return;
        }

        // gets all the tests into a list
        tests = tests = new List<Func<string>>()
        {
            HeadControllerTestCreation,
            HeadControllerAddBodyTest,
            HeadControllerHeadPosTest,
            HeadControllerTailPosTest,
            HeadControllerLengthTest
        };

        // clears all objects in the scene ready for the tests
        ClearScene();

        // goes through each test
        foreach (Func<string> test in tests)
        {
            Debug.Log(test());

            // clears all created object during the last test ready for the next test
            ClearScene();
        }
    }

    private void ClearScene()
    {
        // destroys every object other than the TestManager & EventSystem
        foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
        {
            if (gameObject.tag != "Testing Immune")
            {
                Destroy(gameObject);
            }
        }
    }

    private HeadController PlayerSetup()
    {
        // create the head, grab the headController
        GameObject head = Instantiate(playerPrefab);
        return head.GetComponent<HeadController>();
    }

    private ShopManager ShopManagerSetup(HeadController head)
    {
        // create the ShopManager, grab the ShopManager
        GameObject shopManagerObj = Instantiate(shopManagerPrefab);

        ShopManager shopManager = shopManagerObj.GetComponent<ShopManager>();

        head.shopManager = shopManager;
        shopManager.head = head;

        return shopManager;
    }


    /* ------------------------------------ TESTS RUN ON HEADCONTROLLER ------------------------------------
    * Notes: 
    * AddBody tested through game play and other tests
    * GetXPPercentage tested through game play
    * LevelUP tested through game play
    * IncreaseXP tested through game play
    */


    /// <summary>
    /// Tests that creating a HeadController does not have a problem
    /// </summary>
    /// <returns></returns>
    private string HeadControllerTestCreation()
    {
        HeadController headController = PlayerSetup();

        return "Passed HeadControllerTestCreation";
    }

    private string HeadControllerAddBodyTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // creates a body attatched to the snake
        headController.AddBody("BowMan");

        return "Passed HeadControllerAddBodyTest";

    }

    /// <summary>
    /// Tests that the HeadController.HeadPos() function works
    /// </summary>
    /// <returns></returns>
    private string HeadControllerHeadPosTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // creates a body attatched to the snake
        headController.AddBody("BowMan");

        // creates a random location
        Vector2 randomLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.head.transform.position = randomLocation;

        // checks if HeadPos is equal to the position of the head
        if (headController.HeadPos() != randomLocation)
        {
            return "Failed HeadControllerHeadPosTest, position did not match that of the HeadController's head";
        }

        headController.AddBody("BowMan");

        // checks if HeadPos is equal to the position of the head
        if (headController.HeadPos() != randomLocation)
        {
            return "Failed HeadControllerHeadPosTest, position did not match that of the HeadController's head when adding more bodies";
        }

        return "Passed HeadControllerHeadPosTest";
    }

    private string HeadControllerTailPosTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // creates a body attatched to the snake
        headController.AddBody("BowMan");

        // creates a random location
        Vector2 randomLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.head.transform.position = randomLocation;

        if (headController.TailPos() != randomLocation)
        {
            return $"Failed HeadControllerTailPosTest, with one body tail returned {headController.TailPos()} expected {randomLocation}";
        }

        // creates a body attatched to the snake
        headController.AddBody("BowMan");

        // creates a random location
        Vector2 randomLocation2 = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the second body to that position
        headController.head.next.transform.position = randomLocation2;

        if (headController.TailPos() != randomLocation2)
        {
            return $"Failed HeadControllerTailPosTest, with two bodies tail returned {headController.TailPos()} expected {randomLocation2}";
        }

        return "Passed HeadControllerTailPosTest";
    }

    /// <summary>
    /// Tests that the HeadController.Length() function works
    /// </summary>
    /// <returns></returns>
    private string HeadControllerLengthTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // if the headController's length function doesnt return 0, raise an error
        if (headController.Length() != 0)
        {
            return $"Failed HeadControllerLengthTest, On creation of snake, HeadController.Length() returned {headController.Length()} expected 0";
        }

        // does a length test 5 times
        for (int i = 1; i < 6; i++)
        {
            // creates a body attatched to the snake
            headController.AddBody("BowMan");

            // if the Length() returns something other than the number of BowMans added
            if (headController.Length() != i)
            {
                return $"Failed HeadControllerLengthTest, HeadController.Length() returned {headController.Length()} expected {i}";
            }
        }

        return "Passed HeadControllerLengthTest";
    }


    /* ------------------------------------ TESTS RUN ON BODYCONTROLLER ------------------------------------
    * Notes: 
    */


    /// <summary>
    /// Tests the BodyController.ChangeHealth() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerHealthTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // add a bowman to the snake
        headController.AddBody("BowMan");

        // get the starting health of the bow man
        int startingHealth = headController.head.health;

        // deal 20 damage to the bowman
        if (headController.head.ChangeHealth(-20))
        {
            return $"Failed BodyControllerHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman's health is not exactly 20 less than starting health, return saying test failed
        if (headController.head.health + 20 != startingHealth)
        {
            return $"Failed BodyControllerHealthTest, dealt 20 damage from {startingHealth} ending with {headController.head.health} expected {startingHealth - 20}";
        }

        // heal the bowman by 50
        if (headController.head.ChangeHealth(50))
        {
            return $"Failed BodyControllerHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman isnt at max hp, return saying test failed
        if (headController.head.health != headController.head.MaxHealth)
        {
            return $"Failed BodyControllerHealthTest, overhealing body from {startingHealth - 20} by 50 made it {headController.head.health} rather than expected max health value of {headController.head.MaxHealth}";
        }

        // get the amount of damage about to deal
        int damageDealt = headController.head.health;

        // deal damage to the bowman for its entire hp
        if (headController.head.ChangeHealth(-damageDealt))
        {
            return $"Failed BodyControllerHealthTest, on fatal damage, said the body was still alive";
        }

        // if its not dead, then return saying test failed
        if (!headController.head.IsDead)
        {
            return $"Failed BodyControllerHealthTest, Dealt {damageDealt} damage from {damageDealt} health to the body leaving it at, {headController.head.health} health but did not die";
        }

        return "Passed BodyControllerHealthTest";
    }

    private string BodyControllerAddBodyTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        foreach ()
    }
}

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

        // attatches the head to the shopManager and vice versa
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
        headController.AddBody(shopManager.bodies.RandomItem());

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
        headController.AddBody(shopManager.bodies.RandomItem());

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

    /// <summary>
    /// Tests the HeadController.TailPos() function
    /// </summary>
    /// <returns></returns>
    private string HeadControllerTailPosTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // creates a body attatched to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // creates a random location
        Vector2 randomLocation = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        // sets the head to that position
        headController.head.transform.position = randomLocation;

        if (headController.TailPos() != randomLocation)
        {
            return $"Failed HeadControllerTailPosTest, with one body tail returned {headController.TailPos()} expected {randomLocation}";
        }

        // creates a body attatched to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

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
            headController.AddBody(shopManager.bodies.RandomItem());

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
    * Length() tested through HeadControllerLengthTest
    * BodySetup() tested through BodyControllerAddBodyTest
    * TailPos() tested through the HeadControllerTailPosTest
    * HealthChangeCheck() tested through BodyControllerChangeHealthTest
    * OnDeath() tested through game play
    * Revied() tested through game play
    * Move() tested through game play
    * Buff Updates tested through other tests
    * JsonToBodyData() tested through BodyControllerAddBodyTest
    * LoadFromJson() tested through BodyControllerAddBodyTest and BodyControllerLevelUpTest
    */

    /// <summary>
    /// Tests the BodyController.AddBody() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerAddBodyTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        int length = 0;

        // adds every body from the shopManager to the snake
        foreach (string body in shopManager.bodies)
        {
            headController.AddBody(body);

            length++;
        }

        if (length != headController.Length())
        {
            return $"Failed BodyControllerAddBodyTest, Added {length} bodies but only {headController.Length()} were successfully added";
        }

        return "Passed BodyControllerAddBodyTest";
    }


    /// <summary>
    /// Tests the BodyController.ChangeHealth() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerChangeHealthTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // add a bowman to the snake
        headController.AddBody(shopManager.bodies.RandomItem());

        // get the starting health of the bow man
        int startingHealth = headController.head.health;

        // deal 20 damage to the bowman
        if (headController.head.ChangeHealth(-20))
        {
            return $"Failed BodyControllerChangeHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman's health is not exactly 20 less than starting health, return saying test failed
        if (headController.head.health + 20 != startingHealth)
        {
            return $"Failed BodyControllerChangeHealthTest, dealt 20 damage from {startingHealth} ending with {headController.head.health} expected {startingHealth - 20}";
        }

        // heal the bowman by 50
        if (headController.head.ChangeHealth(50))
        {
            return $"Failed BodyControllerChangeHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman isnt at max hp, return saying test failed
        if (headController.head.health != headController.head.MaxHealth)
        {
            return $"Failed BodyControllerChangeHealthTest, overhealing body from {startingHealth - 20} by 50 made it {headController.head.health} rather than expected max health value of {headController.head.MaxHealth}";
        }

        // get the amount of damage about to deal
        int damageDealt = headController.head.health;

        // deal damage to the bowman for its entire hp
        if (headController.head.ChangeHealth(-damageDealt))
        {
            return $"Failed BodyControllerChangeHealthTest, on fatal damage, said the body was still alive";
        }

        // if its not dead, then return saying test failed
        if (!headController.head.IsDead)
        {
            return $"Failed BodyControllerChangeHealthTest, Dealt {damageDealt} damage from {damageDealt} health to the body leaving it at, {headController.head.health} health but did not die";
        }

        return "Passed BodyControllerChangeHealthTest";
    }

    /// <summary>
    /// Tests the BodyController.IsHead() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerIsHeadTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // adds a body
        headController.AddBody(shopManager.bodies.RandomItem());

        // checks that its the head
        if (!headController.head.IsHead())
        {
            return "Failed BodyControllerIsHeadTest, returned false when it should have returned true on 1st body";
        }

        // adds another body
        headController.AddBody(shopManager.bodies.RandomItem());

        // checks the first body is still the head
        if (!headController.head.IsHead())
        {
            return "Failed BodyControllerIsHeadTest, returned false when it should have returned true after adding 2nd body";
        }

        // checks that the second body is not the head
        if (headController.head.next.IsHead())
        {
            return "Failed BodyControllerIsHeadTest, returned true when it should have returned false on 2nd body";
        }

        return "Passed BodyControllerIsHeadTest";
    }

    /// <summary>
    /// Tests the BodyController.Position() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerPositionTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // adds a body
        headController.AddBody(shopManager.bodies.RandomItem());
        
        // checks its position index is 0
        if (headController.head.Position() != 0)
        {
            return $"Failed BodyControllerPositionTest, returned {headController.head.Position()} when it should have returned 0 on 1st body";
        } 

        // adds another body
        headController.AddBody(shopManager.bodies.RandomItem());

        // checks the first body's position is still 0
        if (headController.head.Position() != 0)
        {
            return $"Failed BodyControllerPositionTest, returned {headController.head.Position()} when it should have returned 0 on 1st body with 2 bodies";
        }

        // checks the new body's position is 1
        if (headController.head.next.Position() != 1)
        {
              return $"Failed BodyControllerPositionTest, returned {headController.head.next.Position()} when it should have returned 1 on 2nd body with 2 bodies";
        }

        return "Passed BodyControllerPositionTest";
    }

    /// <summary>
    /// Tests the BodyController.DestroySelf() function
    /// </summary>
    /// <returns></returns>
    private string BodyControllerDestroySelfTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);

        // adds a body
        headController.AddBody(shopManager.bodies.RandomItem());

        // destroys that body
        headController.head.DestroySelf();

        if (headController.Length() != 0)
        {
            return "Failed BodyControllerDestroySelfTest, did not remove body from snake with 1 body";
        }

        // adds 3 new bodies
        headController.AddBody(shopManager.bodies.RandomItem());
        headController.AddBody(shopManager.bodies.RandomItem());
        headController.AddBody(shopManager.bodies.RandomItem());

        // destroys the second body
        BodyController thirdBody = headController.head.next.next;
        headController.head.next.DestroySelf();

        if (thirdBody != headController.head.next)
        {
            return "Failed BodyControllerDestroySelfTest, did not remove 2nd body from snake with 3 bodies";
        }

        // destroy the tail
        headController.head.next.DestroySelf();

        if (headController.Length() != 1)
        {
              return "Failed BodyControllerDestroySelfTest, did not remove 2nd body from snake with 2 bodies";
        }

        return "Passed BodyControllerDestroySelfTest";
    }

    /*
    private string BodyControllerLevelUpTest()
    {
        HeadController headController = PlayerSetup();
        ShopManager shopManager = ShopManagerSetup(headController);
    }
    */

    // other tests:
    // TriggerManager
    // EnemySummonerController
    // EnemyController
    // Buff
}

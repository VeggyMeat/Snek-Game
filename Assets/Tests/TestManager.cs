using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager: MonoBehaviour
{
    private List<Func<string>> tests;

    [SerializeField] GameObject headControllerPrefab;
    [SerializeField] bool runTests = false;

    void Start()
    {
        // only runs the tests if told to
        if (!runTests)
        {
            return;
        }

        // gets all the tests into a list
        tests = tests = new List<Func<string>>()
        {

        };

        // goes through each test
        foreach (Func<string> test in tests)
        {
            // runs the test, writing to console the output
            try
            {
                Debug.Log(test());
            }
            // in case of any error, print the error and the test that caused it
            catch (Exception e)
            {
                Debug.Log($"test {nameof(test)} threw an error: \n{e}");
            }
        }
    }

    /// <summary>
    /// Tests the BodyController.ChangeHealth() function
    /// </summary>
    /// <returns></returns>
    private string BodyHealthTest()
    {
        // create the head and grab the headController
        GameObject head = Instantiate(headControllerPrefab);
        HeadController headController = head.GetComponent<HeadController>();

        // add a bowman to the snake
        headController.AddBody("BowMan");

        // get the starting health of the bow man
        int startingHealth = headController.head.health;

        // deal 20 damage to the bowman
        if (headController.head.ChangeHealth(-20))
        {
            return $"Failed BodyHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman's health is not exactly 20 less than starting health, return saying test failed
        if (headController.head.health + 20 != startingHealth)
        {
            return $"Failed BodyHealthTest, dealt 20 damage from {startingHealth} ending with {headController.head.health} expected {startingHealth - 20}";
        }

        // heal the bowman by 50
        if (headController.head.ChangeHealth(50))
        {
            return $"Failed BodyHealthTest, returned saying the body was not alive when it still was";
        }

        // if the bowman isnt at max hp, return saying test failed
        if (headController.head.health != headController.head.MaxHealth)
        {
            return $"Failed BodyHealthTest, overhealing body from {startingHealth - 20} by 50 made it {headController.head.health} rather than expected max health value of {headController.head.MaxHealth}";
        }

        // get the amount of damage about to deal
        int damageDealt = headController.head.health;

        // deal damage to the bowman for its entire hp
        if (headController.head.ChangeHealth(-damageDealt))
        {
            return $"Failed BodyHealthTest, on fatal damage, said the body was still alive";
        }

        // if its not dead, then return saying test failed
        if (!headController.head.IsDead)
        {
            return $"Failed BodyHealthTest, Dealt {damageDealt} damage from {damageDealt} health to the body leaving it at, {headController.head.health} health but did not die";
        }

        return "Passed HealthTest";
    }


    /*
     * ------------------------------------ TESTS RUN ON HEADCONTROLLER ------------------------------------
     * Notes: 
     * Excludes functions: (AddBody, GetXPPercentage)
    */


    /// <summary>
    /// tests that creating the headController does not have a problem
    /// </summary>
    /// <returns></returns>
    private string HeadControllerTestCreation()
    {
        try
        {
            // create the head, grab the headController
            GameObject head = Instantiate(headControllerPrefab);
            HeadController headController = head.GetComponent<HeadController>();
        }
        catch (Exception e)
        {
            return $"Failed HeadTestCreation, error \"{e}\"";
        }

        return "HeadTestCreation passed";
    }

    /// <summary>
    /// Tests that the HeadController.HeadPos() function works
    /// </summary>
    /// <returns></returns>
    private string HeadPosTest()
    {
        // creates the head controller
        GameObject head = Instantiate(headControllerPrefab);
        HeadController headController = head.GetComponent<HeadController>();

        // creates a body attatched to the snake
        headController.AddBody("BowMan");

        // checks if HeadPos is equal to the position of the head
        if (headController.HeadPos() != (Vector2)headController.head.transform.position)
        {
            return "Failed HeadPosTest, position did not match that of the headController's head";
        }

        headController.AddBody("BowMan");

        // checks if HeadPos is equal to the position of the head
        if (headController.HeadPos() != (Vector2)headController.head.transform.position)
        {
            return "Failed HeadPosTest, position did not match that of the headController's head when adding more bodies";
        }

        return "Passed HeadPosTest";
    }

    /// <summary>
    /// Tests that the HeadController.Length() function works
    /// </summary>
    /// <returns></returns>
    private string HeadLengthTest()
    {
        // creates the head controller
        GameObject head = Instantiate(headControllerPrefab);
        HeadController headController = head.GetComponent<HeadController>();

        // if the headController's length function doesnt return 0, raise an error
        if (headController.Length() != 0)
        {
            return $"Failed HeadLengthTest, On creation of snake Length() returned {headController.Length()} expected 0";
        }

        // does a length test 5 times
        for (int i = 1; i < 6; i++)
        {
            // creates a body attatched to the snake
            headController.AddBody("BowMan");

            // if the Length() returns something other than the number of BowMans added
            if (headController.Length() != i)
            {
                return $"Failed HeadLengthTest, HeadController.Length() returned {headController.Length()} expected {i}";
            }
        }

        return "Passed HeadLengthTest";
    }
}

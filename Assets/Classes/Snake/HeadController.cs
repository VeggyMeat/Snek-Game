using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    // movement related
    public double turningRate = 2;

    public GameObject circle;
    public GameObject triggerController;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    internal BodyController head;

    internal double angle;
    internal Vector2 velocityVector;
    internal float totalMass = 0;
    internal float velocity = 0;

    internal TriggerController triggerControllerScript;

    private int XP = 0;
    private int XPLevelUp;
    private bool pressed = false;

    void Start()
    {
        // grabs the trigger controller script
        triggerControllerScript = triggerController.GetComponent<TriggerController>();

        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0f, 0f);
    }

    private void Update()
    {
        // temp way to add a body when space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            if (!pressed)
            {
                pressed = true;
                AddBody(circle);
            }
        }
        else
        {
            pressed = false;
        }
    }

    private void FixedUpdate()
    {
        // incomplete

        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        // turning mechanism for the snake updating angle based on left or right arrow keys pressed
        if (rightPress && leftPress)
        {

        }
        else if (rightPress)
        {
            angle += turningRate * Time.deltaTime;

            velocityVector = new Vector2((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)));
        }

        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;


            velocityVector = new Vector2((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)));
        }

        if (head) 
        {

            // moves the whole snake
            head.Move();

            // updates the voids position
            transform.position = HeadPos();
        }
    }

    internal void IncreaseXP(int amount)
    {
        XP += amount;

        if (XP >= XPLevelUp)
        {
            LevelUp();
        }
    }

    internal void LevelUp()
    {
        if (XP < XPLevelUp)
        {
            XP = XPLevelUp;
        }

        XPLevelUp += XPIncreaseLevel;

        // bring to the level up scene
    }

    // returns the position of the head if it exists
    internal Vector2 HeadPos()
    {
        if (head is null)
        {
            throw new Exception();
        }

        return head.transform.position;
    }

    // adds a new body to the snake
    internal void AddBody(GameObject obj)
    {
        if (head is null)
        {
            // creates the body and sets it up and places it as the head of the snake
            GameObject body = Instantiate(obj);

            // randomly choses one of the options
            int choice = UnityEngine.Random.Range(6, 7);

            if (choice == 0)
            {
                // makes the body a bowman (TEMPORARY)
                body.AddComponent<BowMan>();
            }
            else if (choice == 1)
            {
                // makes the body a necro (TEMPORARY)
                body.AddComponent<Necro>();
            }
            else if (choice == 2)
            {
                // makes the body a swordsman (TEMPORARY)
                body.AddComponent<Swordsman>();
            }
            else if (choice == 3)
            {
                // make the body a fire mage (TEMPORARY)
                body.AddComponent<FireMage>();
            }
            else if (choice == 4)
            {
                body.AddComponent<Samurai>();
            }
            else if (choice == 5)
            {
                body.AddComponent<Gambler>();
            }
            else if (choice == 6)
            {
                body.AddComponent<Engineer>();
            }

            head = body.GetComponent<BodyController>();
            head.BodySetup(this, null, triggerControllerScript);
        }
        else
        {
            // makes the head add a new body behind it
            head.AddBody(obj, this);
        }
    }

    // returns the total number of bodies in the snake
    internal int Length()
    {
        if (head is null)
        {
            return 0;
        }
        else
        {
            return head.Length();
        }
    }
}

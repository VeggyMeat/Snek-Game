using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class HeadController : MonoBehaviour
{
    // movement related
    public double turningRate = 2;

    public GameObject circle;
    public GameObject triggerController;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    public List<string> bodies;

    internal BodyController head;

    internal double angle;
    internal Vector2 velocityVector;
    internal float totalMass = 0;
    internal float velocity = 0;

    internal TriggerController triggerControllerScript;

    private int XP = 0;
    private int XPLevelUp;
    private bool pressed = false;

    public GameObject shopManagerObj;

    private ShopManager shopManager;

    void Start()
    {
        // grabs the trigger controller script
        triggerControllerScript = triggerController.GetComponent<TriggerController>();

        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0f, 0f);

        // sets up the shop manager
        shopManager = shopManagerObj.GetComponent<ShopManager>();
        shopManager.Setup(this);
    }

    private void Update()
    {
        // temp way to add a body when space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            if (!pressed)
            {
                pressed = true;

                // adds a random body
                AddBody(bodies[UnityEngine.Random.Range(0, bodies.Count)]);
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
        XP = 0;

        XPLevelUp += XPIncreaseLevel;

        // bring to the level up scene
        shopManager.MakeShop();
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
    internal void AddBody(string bodyClass)
    {
        // creates the body and sets it up and places it as the head of the snake
        GameObject body = Instantiate(circle);

        switch(bodyClass)
        {
            case "BowMan":
                body.AddComponent<BowMan>();
                break;
            case "Gambler":
                body.AddComponent<Gambler>();
                break;
            case "Samurai":
                body.AddComponent<Samurai>();
                break;
            case "Swordsman":
                body.AddComponent<Swordsman>();
                break;
            case "ClockworkMagician":
                body.AddComponent<ClockworkMagician>();
                break;
            case "FireMage":
                body.AddComponent<FireMage>();
                break;
            case "Spectre":
                body.AddComponent<Spectre>();
                break;
            case "Necro":
                body.AddComponent<Necro>();
                break;
            case "Engineer":
                body.AddComponent<Engineer>();
                break;
        }
        if (head is null)
        {
            head = body.GetComponent<BodyController>();
            head.BodySetup(this, null, triggerControllerScript);
        }
        else
        {
            // makes the head add a new body behind it
            head.AddBody(body, this);
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

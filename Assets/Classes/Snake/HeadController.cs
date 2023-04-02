using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    // movement related
    public double turningRate = 2;
    public double velocity = 3;
    public double baseLerpSpeed = 5;

    public GameObject circle;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    internal BodyController? head;

    internal double angle;
    internal Vector2 velocityVector;
    internal float totalMass = 0;
    internal int XP = 0;

    private int XPLevelUp;
    private bool pressed = false;

    void Start()
    {
        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0f, 0f);

        // temporary code adding a BowMan body to the snake
        AddBody(circle);
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

        // moves the whole snake
        head.Move();

        // updates the voids position
        transform.position = HeadPos();
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
            head = body.GetComponent<BodyController>();
            head.Setup(this, null);
            head.gameObject.AddComponent<BowMan>();
            head.gameObject.GetComponent<BowMan>().Setup();
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

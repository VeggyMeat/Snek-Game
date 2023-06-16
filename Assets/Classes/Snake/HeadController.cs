using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class HeadController: MonoBehaviour
{
    // movement related
    public double turningRate = 2;
    public int frameDelay;

    public GameObject circle;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    public List<string> bodies;

    internal BodyController head;

    internal double angle;
    internal Vector2 velocityVector;
    internal float velocity = 0;

    private int XP = 0;
    private int Level = 0;
    private int XPLevelUp;

    public ShopManager shopManager;

    void Start()
    {
        // sets up the item manager
        ItemManager.Setup(this);

        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0f, 0f);

        // sets up the shop manager
        shopManager.Setup(this);

        // adds a body initially
        shopManager.AddBodyShop();
        shopManager.AddBodyShop();
        shopManager.AddBodyShop();
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
        Level++;

        // level up trigger
        TriggerManager.BodyLevelUpTrigger.CallTrigger(Level);

        // bring to the level up scene
        shopManager.AddBodyShop();
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

    internal Vector2 TailPos()
    {
        if (head is null)
        {
            return transform.position;
        }

        return head.TailPos();
    }

    // adds a new body to the snake
    internal void AddBody(string bodyClass)
    {
        // creates the body and sets it up and places it as the head of the snake
        GameObject body = Instantiate(circle, (Vector3)TailPos() + new Vector3 (0, 0, 2), Quaternion.identity);
        BodyController bodyContr = body.AddComponent<BodyController>();

        switch(bodyClass)
        {
            case "BowMan":
                bodyContr.classes.Add(body.AddComponent<BowMan>());
                break;
            case "Gambler":
                bodyContr.classes.Add(body.AddComponent<Gambler>());
                break;
            case "Samurai":
                bodyContr.classes.Add(body.AddComponent<Samurai>());
                break;
            case "Swordsman":
                bodyContr.classes.Add(body.AddComponent<Swordsman>());
                break;
            case "ClockworkMagician":
                bodyContr.classes.Add(body.AddComponent<ClockworkMagician>());
                break;
            case "FireMage":
                bodyContr.classes.Add(body.AddComponent<FireMage>());
                break;
            case "Spectre":
                bodyContr.classes.Add(body.AddComponent<Spectre>());
                break;
            case "Necro":
                bodyContr.classes.Add(body.AddComponent<Necro>());
                break;
            case "Engineer":
                bodyContr.classes.Add(body.AddComponent<Engineer>());
                break;
            case "Bomb":
                bodyContr.classes.Add(body.AddComponent<Bomb>());
                break;
            case "SorcererProdigy":
                bodyContr.classes.Add(body.AddComponent<SorcererProdigy>());
                break;
            case "CeramicAutomaton":
                bodyContr.classes.Add(body.AddComponent<CeramicAutomaton>());
                break;
            case "Sniper":
                bodyContr.classes.Add(body.AddComponent<Sniper>());
                break;
            case "Blacksmith":
                bodyContr.classes.Add(body.AddComponent<Blacksmith>());
                break;
        }
        if (head is null)
        {
            head = body.GetComponent<BodyController>();
            head.BodySetup(this, null);
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

    public float GetXPPercentage()
    {
        return (float)XP / XPLevelUp;
    }
}

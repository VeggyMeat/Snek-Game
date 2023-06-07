using Mono.Cecil;
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
    public int frameDelay;

    public GameObject circle;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    public List<string> bodies;

    internal BodyController head;

    internal double angle;
    internal Vector2 velocityVector;
    internal float totalMass = 0;
    internal float velocity = 0;

    private int XP = 0;
    private int Level = 0;
    private int XPLevelUp;

    public ShopManager shopManager;

    void Start()
    {
        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0f, 0f);

        // sets up the shop manager
        shopManager.Setup(this);

        // adds a body initially
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
            case "Bomb":
                body.AddComponent<Bomb>();
                break;
            case "SorcererProdigy":
                body.AddComponent<SorcererProdigy>();
                break;
            case "CeramicAutomaton":
                body.AddComponent<CeramicAutomaton>();
                break;
            case "Sniper":
                body.AddComponent<Sniper>();
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

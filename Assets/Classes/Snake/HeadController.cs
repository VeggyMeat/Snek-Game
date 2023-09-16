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
    public int frameDelay = 4;

    public GameObject circle;

    // xp related
    public int BaseXPLevelRequirement = 50;
    public int XPIncreaseLevel = 25;

    [SerializeField] internal float bodyHealthBarScaleX = 1;
    [SerializeField] internal float bodyHealthBarScaleY = 1;
    [SerializeField] internal GameObject healthBarPrefab;

    /// <summary>
    /// The BodyController of the head of the snake
    /// </summary>
    internal BodyController head;

    internal double angle = 0f;
    internal Vector2 velocityVector;
    internal float velocity = 0;

    private int XP = 0;
    private int Level = 0;
    private int XPLevelUp;

    public ShopManager shopManager;
    public EnemySummonerController enemySummonerController;

    private List<string> currentBodies = new List<string>();

    public List<string> CurrentBodies
    {
        get { return currentBodies; }
    }

    void Start()
    {
        // sets up the item manager
        ItemManager.Setup(this);

        XPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0, velocity);
        
        // calls an initial body selection
        shopManager.OnLevelUp();
    }

    private void FixedUpdate()
    {
        // gets information on the key presses
        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        // turning mechanism for the snake updating angle based on left or right arrow keys pressed
        if (rightPress && leftPress)
        {

        }
        else if (rightPress)
        {
            angle += turningRate * Time.deltaTime;
        }
        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;    
        }
        velocityVector = new Vector2((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)));

        if (head) 
        {

            // moves the whole snake
            head.Move();

            // updates the voids position
            transform.position = HeadPos();
        }
    }

    /// <summary>
    /// Increases the XP of the snake and levels up if necessary
    /// </summary>
    /// <param name="amount">Amount of XP gained</param>
    internal void IncreaseXP(int amount)
    {
        // increases the xp
        XP += amount;

        // levels it up if it has enough
        if (XP >= XPLevelUp)
        {
            LevelUp();
        }
    }
    
    /// <summary>
    /// Called when the snake levels up, sets up the selection screen
    /// </summary>
    private void LevelUp()
    {
        // resets xp
        XP = 0;

        // increases level, increases next xp requirement
        XPLevelUp += XPIncreaseLevel;
        Level++;

        // level up trigger
        TriggerManager.BodyLevelUpTrigger.CallTrigger(Level);

        // bring to the level up scene
        shopManager.OnLevelUp();
    }

    /// <summary>
    /// Returns the position of the head if it exists
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception">Throws an exception if the head is null</exception>
    internal Vector2 HeadPos()
    {
        if (head is null)
        {
            throw new Exception();
        }

        return head.transform.position;
    }

    /// <summary>
    /// Returns the position of the tail
    /// </summary>
    /// <returns></returns>
    internal Vector2 TailPos()
    {
        if (head is null)
        {
            return transform.position;
        }

        return head.TailPos();
    }

    /// <summary>
    /// Adds a new body to the snake
    /// </summary>
    /// <param name="bodyClass">The name of the class of the new body</param>
    internal void AddBody(string bodyClass)
    {
        // creates the body and sets it up and places it as the head of the snake or at the end
        GameObject body = Instantiate(circle, (Vector3)TailPos() + new Vector3 (0, 0, 2), Quaternion.identity);
        BodyController bodyContr = body.AddComponent<BodyController>();

        // adds the new body to the list of bodies
        currentBodies.Add(bodyClass);

        // adds the respective class to the body
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
            case "Healer":
                bodyContr.classes.Add(body.AddComponent<Healer>());
                break;
            case "Prince":
                bodyContr.classes.Add(body.AddComponent<PrinceFrontline>());
                bodyContr.classes.Add(body.AddComponent<PrinceEnchanter>());
                break;
            case "FairyWithAGun":
                bodyContr.classes.Add(body.AddComponent<FairyWithAGunEnchanter>());
                bodyContr.classes.Add(body.AddComponent<FairyWithAGunArcher>());
                break;
            case "Shieldman":
                bodyContr.classes.Add(body.AddComponent<ShieldmanEnchanter>());
                bodyContr.classes.Add(body.AddComponent<ShieldmanFrontline>());
                break;
            case "EnragedBerzerker":
                bodyContr.classes.Add(body.AddComponent<EnragedBerzerker>());
                break;
            case "Chamelion":
                bodyContr.classes.Add(body.AddComponent<Chamelion>());
                break;
            case "DiscusMan":
                bodyContr.classes.Add(body.AddComponent<DiscusMan>());
                break;
            case "RamHead":
                bodyContr.classes.Add(body.AddComponent<RamHead>());
                break;
            case "Vampire":
                bodyContr.classes.Add(body.AddComponent<VampireMage>());
                bodyContr.classes.Add(body.AddComponent<VampireFrontline>());
                break;
            case "Shotgunner":
                bodyContr.classes.Add(body.AddComponent<Shotgunner>());
                break;
        }

        // removes that body from the list of available bodies
        if (shopManager.remove)
        {
            shopManager.bodies.Remove(bodyClass);
        }

        if (head is null)
        {
            // gets the BodyController and sets it up as the head
            head = body.GetComponent<BodyController>();
            head.BodySetup(this, null);
        }
        else
        {
            // makes the head add a new body behind it
            head.AddBody(body, this);
        }
    }

    /// <summary>
    /// Returns the length of the snake
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Returns the percentage of XP to the next level
    /// </summary>
    /// <returns></returns>
    public float GetXPPercentage()
    {
        return (float)XP / XPLevelUp;
    }
}

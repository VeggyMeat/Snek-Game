using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadController: MonoBehaviour, IHeadController
{
    // movement related
    [SerializeField] private double turningRate = 2;
    [SerializeField] private int frameDelay = 4;

    public Transform Transform
    {
        get
        {
            return transform;
        }
    }

    public double TurningRate
    {
        get
        {
            return turningRate;
        }
        set
        {
            turningRate = value;
        }
    }

    public int FrameDelay
    {
        get
        {
            return frameDelay;
        }
    }

    [SerializeField] private GameObject bodyObject;

    // xp related
    [SerializeField] private int BaseXPLevelRequirement = 50;
    [SerializeField] private int XPIncreaseLevel = 25;

    [SerializeField] internal float bodyHealthBarScaleX = 1;
    [SerializeField] internal float bodyHealthBarScaleY = 1;
    [SerializeField] internal GameObject healthBarPrefab;

    private IGameSetup gameSetup;

    /// <summary>
    /// The BodyController of the head of the snake
    /// </summary>
    private BodyController head;

    public BodyController Head
    {
        get
        {
            return head;
        }
    }

    public void SetHead(BodyController bodyController)
    {
        head = bodyController;
    }

    private double angle = 0f;

    private Vector2 velocityVector;

    internal Vector2 VelocityVector
    {
        get
        {
            return velocityVector;
        }
    }

    private float velocity = 0;

    internal float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    private int xP = 0;
    private int level = 0;
    private int xPLevelUp;

    private bool turning = false;

    private int score = 0;

    public bool Turning
    {
        get
        {
            return turning;
        }
    }

    internal float healingModifier = 1;

    private List<string> currentBodies = new List<string>();

    /// <summary>
    /// The current bodies inside the snake
    /// </summary>
    public List<string> CurrentBodies
    {
        get 
        { 
            return currentBodies; 
        }
    }

    /// <summary>
    /// Whether the body is dead or not
    /// </summary>
    private bool DeathCheck
    {
        get
        {
            if (AliveBodies == 0 && Length > 0)
            {
                OnDeath();

                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// The position of the head if it exists
    /// </summary>
    /// <exception cref="Exception">Throws an exception if the head is null</exception>
    public Vector2 HeadPos
    {
        get
        {
            if (head is null)
            {
                throw new Exception();
            }

            return head.transform.position;
        }
    }

    /// <summary>
    /// The position of the tail
    /// </summary>
    public Vector2 TailPos
    {
        get
        {
            if (head is null)
            {
                return transform.position;
            }

            return head.TailPos();
        }
    }

    /// <summary>
    /// Returns the length of the snake
    /// </summary>
    public int Length
    {
        get
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

    /// <summary>
    /// Returns the number of alive bodies
    /// </summary>
    public int AliveBodies
    {
        get 
        {
            if (head is null)
            {
                return 0;
            }
            else
            {
                return head.AliveBodies();
            }
        }
    }

    /// <summary>
    /// The percentage of the way to the next level
    /// </summary>
    public float XPPercentage
    {
        get
        {
            return (float)xP / xPLevelUp;
        }
    }

    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    private void Start()
    {
        // sets up the databse handler
        DatabaseHandler.Setup();

        // sets up the item manager
        ItemManager.Setup(gameSetup);

        // sets up the AOEEffect system
        AOEEffect.Setup();

        // sets up the time manager
        TimeManager.Setup();

        xPLevelUp = BaseXPLevelRequirement;
        velocityVector = new Vector2(0, velocity);
        
        // calls an initial body selection
        gameSetup.ShopManager.OnLevelUp();
    }

    private void FixedUpdate()
    {
        if (DeathCheck)
        {
            return;
        }

        UpdateTurning();

        if (head) 
        {
            // moves the whole snake
            head.Move();

            // updates the voids position
            transform.position = HeadPos;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
    }

    private void UpdateTurning()
    {
        // gets information on the key presses
        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        // turning mechanism for the snake updating angle based on left or right arrow keys pressed
        if (rightPress && leftPress)
        {
            if (turning)
            {
                TriggerManager.StopTurningTrigger.CallTrigger(0);
            }

            turning = false;
        }
        else if (rightPress)
        {
            angle += turningRate * Time.deltaTime;

            if (!turning)
            {
                TriggerManager.StartTurningTrigger.CallTrigger(0);
            }

            turning = true;
        }
        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;

            if (!turning)
            {
                TriggerManager.StartTurningTrigger.CallTrigger(0);
            }

            turning = true;
        }
        else
        {
            if (turning)
            {
                TriggerManager.StopTurningTrigger.CallTrigger(0);
            }

            turning = false;
        }

        velocityVector = new Vector2((float)(velocity * Math.Sin(angle) / AliveBodies), (float)(velocity * Math.Cos(angle) / AliveBodies));
    }

    public List<string> FinishRun(string name)
    {
        Run run = new Run
        {
            PlayerName = name,
            Score = score,
            Time = (int)TimeManager.GetElapsedTimeSince(TimeManager.StartTime).TotalSeconds,
            Date = DateTime.Now
        };

        DatabaseHandler.AddRun(run);

        return new List<string> { name, score.ToString(), ((int)TimeManager.GetElapsedTimeSince(TimeManager.StartTime).TotalSeconds).ToString(), DateTime.Now.ToString() };
    }

    private void OnDeath()
    {
        gameSetup.ShopManager.PauseTime();

        gameSetup.DeathScreenController.OnDeath();
    }

    /// <summary>
    /// Increases the XP of the snake and levels up if necessary
    /// </summary>
    /// <param name="amount">Amount of XP gained</param>
    public void IncreaseXP(int amount)
    {
        // increases the xp
        xP += amount;

        // levels it up if it has enough
        if (xP >= xPLevelUp)
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
        xP = 0;

        // increases level, increases next xp requirement
        xPLevelUp += XPIncreaseLevel;
        level++;

        // level up trigger
        TriggerManager.BodyLevelUpTrigger.CallTrigger(level);

        // bring to the level up scene
        gameSetup.ShopManager.OnLevelUp();
    }

    /// <summary>
    /// Adds a new body to the snake
    /// </summary>
    /// <param name="bodyClass">The name of the class of the new body</param>
    public void AddBody(string bodyClass)
    {
        // creates the body and sets it up and places it as the head of the snake or at the end
        GameObject body = Instantiate(bodyObject, (Vector3)TailPos + new Vector3 (0, 0, 2), Quaternion.identity);
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
            case "Ninja":
                bodyContr.classes.Add(body.AddComponent<Ninja>());
                break;
            case "MagicArcher":
                bodyContr.classes.Add(body.AddComponent<MagicArcherArcher>());
                bodyContr.classes.Add(body.AddComponent<MagicArcherMage>());
                break;
            case "TheRightHandMan":
                bodyContr.classes.Add(body.AddComponent<TheRightHandMan>());
                break;
            case "Furnace":
                bodyContr.classes.Add(body.AddComponent<Furnace>());
                break;
            case "Pyromaniac":
                bodyContr.classes.Add(body.AddComponent<Pyromaniac>());
                break;
            case "Herbologist":
                bodyContr.classes.Add(body.AddComponent<Herbologist>());
                break;
            case "DeathWard":
                bodyContr.classes.Add(body.AddComponent<DeathWardEnchanter>());
                bodyContr.classes.Add(body.AddComponent<DeathWardMage>());
                break;
            case "SpikeLauncher":
                bodyContr.classes.Add(body.AddComponent<SpikeLauncherArcher>());
                bodyContr.classes.Add(body.AddComponent<SpikeLauncherFrontline>());
                break;
            case "ForceFieldMage":
                bodyContr.classes.Add(body.AddComponent<ForceFieldMageFrontline>());
                bodyContr.classes.Add(body.AddComponent<ForceFieldMageMage>());
                break;
            case "MirrorMage":
                bodyContr.classes.Add(body.AddComponent<MirrorMageMage>());
                bodyContr.classes.Add(body.AddComponent<MirrorMageArcher>());
                break;
            case "ArcaneFlower":
                bodyContr.classes.Add(body.AddComponent<ArcaneFlowerEnchanter>());
                bodyContr.classes.Add(body.AddComponent<ArcaneFlowerMage>());
                break;
            case "Swashbuckler":
                bodyContr.classes.Add(body.AddComponent<SwashbucklerArcher>());
                bodyContr.classes.Add(body.AddComponent<SwashbucklerFrontline>());
                break;
            case "Cannoneer":
                bodyContr.classes.Add(body.AddComponent<CannoneerEnchanter>());
                bodyContr.classes.Add(body.AddComponent<CannoneerArcher>());
                break;
        }

        // removes that body from the list of available bodies
        if (gameSetup.ShopManager.Remove)
        {
            gameSetup.ShopManager.RemoveBody(bodyClass);
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

    public void Rearrange(List<BodyController> order)
    {
        // makes sure there is the right number of objects in the list
        if (order.Count != currentBodies.Count)
        {
            throw new Exception("list passed does not contain the right number of objects");
        }

        // if the snake is empty, return
        if (head is null)
        {
            return;
        }

        // gets the current order of bodies
        List<(Vector2, Vector2, Vector2, Queue<Vector2>)> previousOrder = new List<(Vector2, Vector2, Vector2, Queue<Vector2>)>();

        BodyController body = head;
        while (body is not null)
        {
            previousOrder.Add((new Vector2 (body.transform.position.x, body.transform.position.y),
                               new Vector2 (body.lastMoved.x, body.lastMoved.y),
                               new Vector2 (body.lastPosition.x, body.lastPosition.y),
                               new Queue<Vector2> (body.PositionFollow)));

            body = body.next;
        }

        head = order[0];
        order[0].prev = null;
        order[0].next = null;
        for (int i = 1; i < order.Count; i++)
        {
            order[i-1].next = order[i];
            order[i].prev = order[i - 1];
            order[i].next = null;
        }

        for (int i = 0; i < order.Count; i++)
        {
            var info = previousOrder[i];

            order[i].transform.position = info.Item1;
            order[i].lastMoved = info.Item2;
            order[i].lastPosition = info.Item3;
            order[i].PositionFollow = info.Item4;
        }
    }
}

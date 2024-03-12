using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// COMPLETE

/// <summary>
/// The controller placed on an empty object that controls the snake
/// It has an attached link list of all the bodies in the snake
/// Manages some general game flow, calling other scripts when necessary to handle the player input
/// Stores general information about the players run
/// </summary>
public class HeadController: MonoBehaviour, IHeadController
{
    /// <summary>
    /// The transform of the head controller, which follows the snake's Head every frame
    /// </summary>
    public Transform Transform
    {
        get
        {
            return transform;
        }
    }

    /// <summary>
    /// The speed at which the snake turns
    /// </summary>
    private double turningRate = 2;

    /// <summary>
    /// The speed at which the snake turns
    /// </summary>
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

    /// <summary>
    /// The number of frames behind each body part is from the previous one
    /// </summary>
    private const int frameDelay = 7;

    /// <summary>
    /// The number of frames behind each body part is from the previous one
    /// </summary>
    public int FrameDelay
    {
        get
        {
            return frameDelay;
        }
    }

    /// <summary>
    /// The prefab of the body object, that is used to instantiate new bodies
    /// 
    /// Does not change, set in the Inspector in Unity
    /// </summary>
    [SerializeField] private GameObject bodyObject;

    /// <summary>
    /// The amount of XP required to level up to the first level
    /// </summary>
    private const int BaseXPLevelRequirement = 60;

    /// <summary>
    /// The amount of XP added to the requirement for each level after each level
    /// </summary>
    private const int XPIncreaseLevel = 35;

    /// <summary>
    /// The X value scaling of the health bar for the bodies
    /// </summary>
    internal readonly float bodyHealthBarScaleX = 0.2f;

    /// <summary>
    /// The Y value scaling of the health bar for the bodies
    /// </summary>
    internal readonly float bodyHealthBarScaleY = 0.2f;

    /// <summary>
    /// The prefab of the health bar, that is used to instantiate new health bars
    /// </summary>
    [SerializeField] internal GameObject healthBarPrefab;

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// The head of the snake
    /// </summary>
    private BodyController head;

    /// <summary>
    /// The head of the snake
    /// </summary>
    public BodyController Head
    {
        get
        {
            return head;
        }
    }

    /// <summary>
    /// Sets the head of the snake to a perticular body
    /// </summary>
    /// <param name="bodyController">The new body to become the head</param>
    internal void SetHead(BodyController bodyController)
    {
        head = bodyController;
    }

    /// <summary>
    /// The angle that the snake is facing
    /// </summary>
    private double angle = 0f;

    /// <summary>
    /// The current velocity scaled and added each frame to the snake
    /// </summary>
    private Vector2 velocityVector;

    /// <summary>
    /// The current velocity scaled and added each frame to the snake
    /// </summary>
    internal Vector2 VelocityVector
    {
        get
        {
            return velocityVector;
        }
    }

    /// <summary>
    /// The total velocities of the snake's bodies
    /// </summary>
    internal float Velocity
    {
        get
        {
            if (head)
            {
                return head.Velocity / Bodies;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// The amount of XP the snake has
    /// </summary>
    private int xP = 0;

    /// <summary>
    /// The current level of the snake
    /// </summary>
    private int level = 0;

    /// <summary>
    /// The current amount of XP required to level up
    /// </summary>
    private int xPLevelUp;

    /// <summary>
    /// Whether the snake is currently turning
    /// </summary>
    private bool turning = false;

    /// <summary>
    /// The current score of the run
    /// </summary>
    private int score = 0;

    /// <summary>
    /// Whether the snake is currently turning
    /// </summary>
    public bool Turning
    {
        get
        {
            return turning;
        }
    }

    /// <summary>
    /// The modifier applied to all the healing of the snake
    /// </summary>
    internal float healingModifier = 1;

    /// <summary>
    /// The list of all the bodies in the snake
    /// </summary>
    private List<string> currentBodies = new List<string>();

    /// <summary>
    /// The list of all the bodies in the snake
    /// </summary>
    public List<string> CurrentBodies
    {
        get 
        { 
            return currentBodies; 
        }
    }

    /// <summary>
    /// The position of the head of the snake
    /// </summary>
    /// <exception cref="Exception">Throws an exception if the head is null</exception>
    public Vector2 HeadPos
    {
        get
        {
            // if there is no head, throw an exception
            if (head is null)
            {
                throw new Exception("HeadPos cannot be returned, there is no head");
            }

            return head.transform.position;
        }
    }

    /// <summary>
    /// The position of the tail of the snake (the last object in the linked list)
    /// </summary>
    /// <exception cref="Exception">Throws an exception if there are no bodies in the snake</exception>"
    public Vector2 TailPos
    {
        get
        {
            if (Length == 0)
            {
                throw new Exception("TailPos cannot be returned, there are no bodies in the snake");
            }

            return head.TailPos();
        }
    }

    /// <summary>
    /// The total number of bodies in the snake
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
    /// The total number of bodies in the snake that are alive (not dead)
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
    /// The total number of bodies in the snake
    /// </summary>
    public int Bodies
    {
        get
        {
            if (head is null)
            {
                return 0;
            }
            else
            {
                return head.Bodies();
            }
        }
    }

    /// <summary>
    /// The percentage completion of the current level of the snake (0-1)
    /// </summary>
    public float XPPercentage
    {
        get
        {
            return (float)xP / xPLevelUp;
        }
    }

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup"></param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// Whether the body is dead or not
    /// </summary>
    private void DeathCheck()
    {
        // if there are no bodies alive, and at least one body has already been added, the snake is dead, so call OnDeath()
        if (AliveBodies == 0 && Length > 0)
        {
            OnDeath();
        }
    }

    // Called by unity before the first frame
    private void Start()
    {
        // sets the current amount of XP required to level up
        xPLevelUp = BaseXPLevelRequirement;

        // sets the snake's initial velocity
        velocityVector = new Vector2(0, Velocity);
        

        // calls an initial body selection for the player
        gameSetup.ShopManager.OnLevelUp();
    }

    // Called by unity every frame before doing any physics calculations
    private void FixedUpdate()
    {
        DeathCheck();

        UpdateTurning();

        if (head && AliveBodies > 0) 
        {
            head.Move();

            // updates the snake's position, to match the head's
            transform.position = HeadPos;
        }
    }

    /// <summary>
    /// Updates the velocity vector of the snake and the snake's angle based on the player's keypresses
    /// </summary>
    private void UpdateTurning()
    {
        // gets information on the key presses, whether the left or right arrow keys are pressed down
        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        // turning mechanism for the snake updating angle based on left or right arrow keys pressed
        // calls the turning trigger if the snake starts turning this frame
        // calls the stop turning trigger if the snake stops turning this frame
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

        // sets the new velocity vector based on the new angle
        velocityVector = new Vector2((float)(Velocity * Math.Sin(angle)), (float)(Velocity * Math.Cos(angle)));
    }

    /// <summary>
    /// Adds the run to the database, with the information from the snake
    /// </summary>
    /// <param name="name">The player's name for this run</param>
    /// <returns>The list of statistics stored in the database for this run (name, score, time, date)</returns>
    public List<string> FinishRun(string name)
    {
        // creates a new run with all the relevant information
        Run run = new Run
        {
            PlayerName = name,
            Score = score,
            Time = (int)TimeManager.GetElapsedTimeSince(TimeManager.StartTime).TotalSeconds,
            Date = DateTime.Now
        };

        // adds the run to the database
        DatabaseHandler.AddRun(run);

        // returns the statistics of the run
        return new List<string> { name, score.ToString(), ((int)TimeManager.GetElapsedTimeSince(TimeManager.StartTime).TotalSeconds).ToString(), DateTime.Now.ToString() };
    }

    /// <summary>
    /// Ends the game after the player dies
    /// </summary>
    private void OnDeath()
    {
        // pauses the game time from passing
        gameSetup.ShopManager.PauseTime();

        // puts the death screen up for the player
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

        // levels it up if it has enough xp
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
        // removes the xp required to level up
        xP -= xPLevelUp;

        // increases level, increases next xp requirement
        xPLevelUp += XPIncreaseLevel;
        level++;

        // level up trigger
        TriggerManager.BodyLevelUpTrigger.CallTrigger(level);

        // bring the player to the level up screen
        gameSetup.ShopManager.OnLevelUp();
    }

    /// <summary>
    /// Adds a new body to the snake
    /// </summary>
    /// <param name="bodyClass">The name of the class of the new body</param>
    public void AddBody(string bodyClass)
    {
        // creates the body and sets it up and places it as the head of the snake or at the end
        GameObject body;
        if (head is null)
        {
            body = Instantiate(bodyObject, Transform.position + new Vector3(0, 0, 2), Quaternion.identity);
        }
        else
        {
            body = Instantiate(bodyObject, (Vector3)TailPos + new Vector3(0, 0, 2), Quaternion.identity);
        }

        // adds the BodyController to the body
        BodyController bodyContr = body.AddComponent<BodyController>();

        // adds the new body to the list of bodies
        currentBodies.Add(bodyClass);

        // adds the respective class(es) to the body
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
            case "Chameleon":
                bodyContr.classes.Add(body.AddComponent<Chameleon>());
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

        // removes that body from the list of available bodies if required
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
            // if the head already exists, makes the head try to create the body
            head.AddBody(body, this);
        }
    }

    /// <summary>
    /// Rearranges the bodies of the snake to the order given
    /// </summary>
    /// <param name="order">The new order of the bodies, the first item being the head, the last being the tail</param>
    /// <exception cref="Exception">Throws an exception if the objects don't match</exception>
    public void Rearrange(List<BodyController> order)
    {
        // makes sure there is the right number of objects in the list
        if (order.Count != currentBodies.Count)
        {
            throw new Exception("list passed does not contain the right number of objects");
        }

        // makes sure the list has the same bodies as the snake
        List<string> orderNames = new List<string>();
        foreach (BodyController bodyController in order)
        {
            orderNames.Add(bodyController.Name);
        }

        // checks if the list of names has the same names as the list of body (but potentially in a different order)
        if (!orderNames.All(currentBodies.Contains))
        {
            throw new Exception("list passed does not contain the same bodies as the snake");
        }

        // if the snake is empty, return
        if (head is null)
        {
            return;
        }

        // gets the current order of bodies
        List<(Vector2, Vector2, Vector2, Queue<Vector2>)> previousOrder = new List<(Vector2, Vector2, Vector2, Queue<Vector2>)>();

        // goes through every body in the snake
        BodyController body = head;
        while (body is not null)
        {
            // grabs all the relevant positional information from the body
            previousOrder.Add((new Vector2 (body.transform.position.x, body.transform.position.y),
                               new Vector2 (body.lastMoved.x, body.lastMoved.y),
                               new Vector2 (body.lastPosition.x, body.lastPosition.y),
                               new Queue<Vector2> (body.PositionFollow)));

            body = body.next;
        }


        // goes through every body in the snake and sets the new order
        // sets the new head as the first body
        head = order[0];

        // resets the new head's previous and next bodies
        order[0].prev = null;
        order[0].next = null;

        // goes through all the other bodies to do the same thing
        for (int i = 1; i < order.Count; i++)
        {
            // sets the previous body's next body to this one
            order[i-1].next = order[i];

            // sets this body's previous body to the previous one
            order[i].prev = order[i - 1];

            // resets the next body of this body
            order[i].next = null;
        }

        // goes through all the new bodies and gives them the positional information from the old body that was in that position
        for (int i = 0; i < order.Count; i++)
        {
            (Vector2, Vector2, Vector2, Queue<Vector2>) info = previousOrder[i];

            order[i].transform.position = info.Item1;
            order[i].lastMoved = info.Item2;
            order[i].lastPosition = info.Item3;
            order[i].PositionFollow = info.Item4;
        }
    }
}

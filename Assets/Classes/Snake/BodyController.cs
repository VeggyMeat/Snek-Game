using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    private int defence;
    private int maxHealth;
    private float velocityContribution;

    protected new string name;

    /// <summary>
    /// The name of the class
    /// </summary>
    public string Name
    {
        get
        {
            return name;
        }
    }

    private int level = 0;

    /// <summary>
    /// The body's current level (starts from 1)
    /// </summary>
    public int Level
    {
        get
        {
            return level;
        }
    }

    internal int maxLevel;

    internal bool levelable = true;

    /// <summary>
    /// Defence value of the body, any incoming damage gets reduced by this
    /// </summary>
    public int Defence
    { 
        get
        {
            return (int) defenceBuff.Value;
        }
        set
        {
            defence = value;
            defenceBuff.updateOriginalValue(value);
        }
    }

    /// <summary>
    /// The maximum and starting health value of the body
    /// </summary>
    public int MaxHealth
    {
        get
        {
            return (int) healthBuff.Value;
        }
        set
        {
            maxHealth = value;
            healthBuff.updateOriginalValue(value);
            HealthChangeCheck();
        }
    }

    /// <summary>
    /// The contribution to the snake's velocity given by the body (bigger = faster)
    /// </summary>
    public float VelocityContribution
    {
        get
        {
            return speedBuff.Value;
        }
        set
        {
            float oldValue = VelocityContribution;
            velocityContribution = value;
            speedBuff.updateOriginalValue(value);
            UpdateVelocityContribution(oldValue);
        }
    }

    /// <summary>
    /// Multiplier for all body damage
    /// </summary>
    public float DamageMultiplier
    {
        get
        {
            return damageBuff.Value;
        }
    }

    /// <summary>
    /// The list of all the names of the classes that are attatched to this body
    /// </summary>
    internal List<string> classNames = new List<string>();

    private int contactDamage;
    private int contactForce;

    /// <summary>
    /// The damage dealt on contact with an enemy
    /// </summary>
    public int ContactDamage
    {
        get
        {
            return (int) (contactDamage * DamageMultiplier);
        }
        set
        {
            contactDamage = value;
        }
    }

    /// <summary>
    /// The force applied to the enemy on contact
    /// </summary>
    public int ContactForce
    {
        get
        {
            return contactForce;
        }
        set
        {
            contactForce = value;
        }
    }

    /// <summary>
    /// Buff dealing with maxHealth and health
    /// </summary>
    internal Buff healthBuff;

    /// <summary>
    /// Buff dealing with the velocity contribution of the body
    /// </summary>
    internal Buff speedBuff;
    
    /// <summary>
    /// Buff dealing with the damage multiplier of the body
    /// </summary>
    internal Buff damageBuff;

    /// <summary>
    /// Buff dealing with the body's defence
    /// </summary>
    internal Buff defenceBuff;
    
    /// <summary>
    /// Body dealing with the attack speed multiplier of the body
    /// </summary>
    internal Buff attackSpeedBuff;

    /// <summary>
    /// The color of the body
    /// </summary>
    private Color color;

    // rgb colours of the body
    private float r;
    private float g;
    private float b;

    /// <summary>
    /// The length of time the body is dead for before automatically being revived
    /// </summary>
    private float timeDead = 30f;

    /// <summary>
    /// The rigidbody of the body
    /// </summary>
    internal Rigidbody2D selfRigid;
    
    /// <summary>
    /// The sprite renderer of the body
    /// </summary>
    internal SpriteRenderer spriteRenderer;


    /// <summary>
    /// The next snake body in the chain
    /// </summary>
    internal BodyController next;

    /// <summary>
    /// The previous snake body in the chain
    /// </summary>
    internal BodyController prev;

    /// <summary>
    /// The HeadController of the snake
    /// </summary>
    internal HeadController snake;

    /// <summary>
    /// The current health value of the body
    /// </summary>
    internal int health;


    /// <summary>
    /// The previous velocity vector of the body last FixedUpdate
    /// </summary>
    internal Vector2 lastMoved;

    /// <summary>
    /// The last position this body was in, last FixedUpdate
    /// </summary>
    internal Vector2 lastPosition;

    private bool isDead = false;

    /// <summary>
    /// Whether the body is dead or not
    /// </summary>
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    /// <summary>
    /// All of the classes attatched to this body
    /// </summary>
    internal List<Class> classes = new List<Class>();

    /// <summary>
    /// The path of the body's json file
    /// </summary>
    internal string jsonFile;

    /// <summary>
    /// All of the variables' data for each level
    /// </summary>
    private List<Dictionary<string, object>> jsonData;

    /// <summary>
    /// Whether the json data has been loaded in before or not
    /// </summary>
    private bool jsonLoaded = false;

    /// <summary>
    /// A queue of positions that the previous body was in frames ago
    /// </summary>
    private Queue<Vector2> positionFollow = new Queue<Vector2>();

    // sets up variables
    private void Setup()
    {
        // sets up the classes bit on the body
        foreach(Class c in classes)
        {
            c.body = this;
            c.ClassSetup();
        }

        // loads in the data from json
        LevelUp();

        // updates the total mass of the snake
        snake.velocity += velocityContribution;
        health = maxHealth;

        // sets the color of the object
        ResetColour();

        // sets up the buffs
        healthBuff = gameObject.AddComponent<Buff>();
        healthBuff.Setup(HealthBuffUpdate, maxHealth);

        speedBuff = gameObject.AddComponent<Buff>();
        speedBuff.Setup(SpeedBuffUpdate, velocityContribution);

        damageBuff = gameObject.AddComponent<Buff>();
        damageBuff.Setup(null, 1f);

        defenceBuff = gameObject.AddComponent<Buff>();
        defenceBuff.Setup(null, defence);

        attackSpeedBuff = gameObject.AddComponent<Buff>();
        attackSpeedBuff.Setup(AttackSpeedBuffUpdate, 1f);

        // sets up the classes
        foreach (Class c in classes)
        {
            c.body = this;
            c.Setup();
        }

        // calls the trigger saying a new body was added
        TriggerManager.BodySpawnTrigger.CallTrigger(this);
    }

    // function called when a new body is created

    /// <summary>
    /// Sets up the body and its classes
    /// </summary>
    /// <param name="snake">Snake that the body belongs to</param>
    /// <param name="prev">The previous body in the snake (or null if none)</param>
    internal void BodySetup(HeadController snake, BodyController prev)
    {
        // sets up the starting variables for the body
        this.snake = snake;
        this.prev = prev;

        // sets the position and grabs the rigid body
        selfRigid = gameObject.GetComponent<Rigidbody2D>();

        // grabs the spriteRenderer
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Setup();
    }

    void FixedUpdate()
    {
        // updates the last moved vector for projectiles shot
        lastMoved = (selfRigid.position - lastPosition) / Time.deltaTime;
        lastPosition = selfRigid.position;
    }

    /// <summary>
    /// Returns whether the body is the head of the snake
    /// </summary>
    /// <returns>If its the head or not</returns>
    internal bool IsHead()
    {
        return prev is null;
    }

    /// <summary>
    /// Adds a new body to the end of the snake
    /// </summary>
    /// <param name="body">The snake body to be added</param>
    /// <param name="snake">The snake the new body will belong to</param>
    internal void AddBody(GameObject body, HeadController snake)
    {
        if (next is null)
        {
            // sets up the body
            next = body.GetComponent<BodyController>();
            next.BodySetup(snake, this);
        }
        else
        {
            // makes the next body add a new body behind it
            next.AddBody(body, snake);
        }
    }

    /// <summary>
    /// Returns the position of the body in the snake
    /// </summary>
    /// <returns></returns>
    internal int Position()
    {
        // if its the head, return 0
        if (prev is null)
        {
            return 0;
        }

        // else return one plus the position of the previous body
        return prev.Position() + 1;
    }

    /// <summary>
    /// Returns the length of body parts from it
    /// </summary>
    /// <returns></returns>
    internal int Length()
    {
        // if its the tail returns one
        if (next is null)
        {
            return 1;
        }
        // otherwise return one plus the length from the next body
        else
        {
            return 1 + next.Length();
        }
    }

    /// <summary>
    /// Gives the position of the tail of the snake
    /// </summary>
    /// <returns></returns>
    internal Vector2 TailPos()
    {
        // if its the tail, return its position
        if (next is null)
        {
            return transform.position;
        }

        // otherwise passes it down the chain
        return next.TailPos();
    }

    /// <summary>
    /// checks if body is dead or has too much HP (returns whether the body is still alive)
    /// </summary>
    /// <returns>If the body survives or not</returns>
    private bool HealthChangeCheck()
    {
        // if the health is bigger than MaxHealth, reduce it down to MaxHealth
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }

        // if its less than 0, kill the body
        else if (health <= 0)
        {
            OnDeath();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Changes the health of the body when damage is dealt or healing applied
    /// </summary>
    /// <param name="quantity">The change in health requested</param>
    /// <returns>If the body survives or not</returns>
    internal virtual bool ChangeHealth(int quantity)
    {
        if (quantity > 0)
        {
            // increase health trigger (ASSUMES NO HEALING WILL MAKE YOU TAKE DAMAGE)
            quantity = TriggerManager.BodyGainedHealthTrigger.CallTriggerReturn(quantity);

            // increases the health by the amount healed
            health += quantity;
        }
        else if (quantity < 0)
        {
            // calls each class for taking damage
            foreach (Class clas in classes)
            {
                clas.OnDamageTaken(quantity);
            }

            // reduce the damage taken by the defence
            quantity += Defence;

            // if the body ignores damage from defence, return it survived (it cant heal from blocking too much)
            if (quantity >= 0)
            {
                return true;
            }

            // lost health trigger (ASSUMES IT WILL NOT MAKE BODY HEAL)
            quantity = TriggerManager.BodyLostHealthTrigger.CallTriggerReturn(quantity);

            // reduces the health by the final damage
            health += quantity;
        }

        return HealthChangeCheck();
    }

    /// <summary>
    /// Called when the body dies
    /// </summary>
    private void OnDeath()
    {
        // sets the body to be dead
        isDead = true;

        // reverts the original additions from the body
        snake.velocity -= VelocityContribution;

        // changes the tag so enemies wont interact with it
        gameObject.tag = "Dead";

        // makes the body slightly transparent to indicate death
        Color oldColor = spriteRenderer.color;
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.4f);

        // makes the body revive in timeDead seconds
        Invoke(nameof(Revived), timeDead);

        // calls the OnDeath for all classes attatched
        foreach (Class c in classes)
        {
            c.OnDeath();
        }

        // body died trigger called
        TriggerManager.BodyDeadTrigger.CallTrigger(gameObject);
    }

    /// <summary>
    /// Called when the body is revived
    /// </summary>
    private void Revived()
    {
        // sets the body to be alive again
        isDead = false;

        // updates the total mass of the snake
        snake.velocity += VelocityContribution;
        health = MaxHealth;

        // changes the tag back so that enemies can deal damage
        gameObject.tag = "Player";

        // returns the body back to normal color
        Color oldColor = spriteRenderer.color;
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);

        // calls the Revived for all classes attatched
        foreach (Class c in classes)
        {
            c.Revived();
        }

        // stops it from being revived again if its a premature revive (not implemented yet)
        CancelInvoke(nameof(Revived));
    }

    /// <summary>
    /// Removes the body from the snake (and destroys it)
    /// </summary>
    internal void DestroySelf()
    {
        // removes itself from the linkedList, filling the gap
        if (next is not null)
        {
            next.prev = prev;
        }

        if (prev is not null)
        {
            prev.next = next;
        }
        else
        {
            // if it is the head, makes sure the snake's head is updated on the snake script
            snake.head = next;
        }

        // reverts the original additions from the body
        snake.velocity -= VelocityContribution;

        // destroys this body
        Destroy(gameObject);
    }

    /// <summary>
    /// Moves the body of this body, passes along to the next
    /// </summary>
    /// <param name="place"></param>
    internal void Move(Vector2 place = new Vector2())
    {
        // if head
        if (IsHead())
        {
            // get the vector its moved in the last frame
            Vector2 movement = snake.velocityVector * Time.deltaTime / snake.Length();

            // add it to the list for the next snake to follow, if there is one
            if (next is not null)
            {
                positionFollow.Enqueue(movement + selfRigid.position);
            }

            // move the body to the new position
            selfRigid.MovePosition(movement + selfRigid.position);

            // if there is a body following it
            if (next is not null)
            {
                // if it is ready to move, move it, and remove the spot from the queue
                if (positionFollow.Count > snake.frameDelay)
                {
                    next.Move(positionFollow.Dequeue());
                }
            }
        }
        // if not
        else
        {
            // moves the body to the new place
            selfRigid.MovePosition(place);

            // if there is a body following it
            if (next is not null)
            {
                // add the new position to the end of the queue
                positionFollow.Enqueue(place);

                // if it is ready to move, move it, and remove the spot from the queue
                if (positionFollow.Count > snake.frameDelay)
                {
                    next.Move(positionFollow.Dequeue());
                }
            }
        }
    }

    /// <summary>
    /// Updates the snake's velocity when a speed buff is added or removed
    /// </summary>
    /// <param name="prev"></param>
    private void UpdateVelocityContribution(float prev)
    {
        // removes the previous velocity amount, and adds the new amount
        snake.velocity -= prev;
        snake.velocity += VelocityContribution;
    }

    /// <summary>
    /// Called by Buff Manager when healthBuff is changed
    /// </summary>
    /// <param name="amount">the value of the change by the health buff</param>
    /// <param name="multiplicative">Whether its a value added to max health (false) or a scaler (true)</param>
    private void HealthBuffUpdate(float amount, bool multiplicative)
    {
        // if its a multiplying one, multiply the health by that much
        if (multiplicative)
        {
            health = (int)(health * amount);
        }
        // if its an additive one, increase (/decrease) the health by the amount
        else
        {
            health += (int)amount;
        }

        // call a health change check to see if body has died
        HealthChangeCheck();

        // passes on the update to the classes
        foreach(Class c in classes)
        {
            c.OnHealthBuffUpdate(amount, multiplicative);
        }
    }

    /// <summary>
    /// Called by Buff Manager when speedBuff is changed
    /// </summary>
    /// <param name="amount">the value of the change by the speed buff</param>
    /// <param name="multiplicative">Whether its a value added to speed (false) or a scaler (true)</param>
    private void SpeedBuffUpdate(float amount, bool multiplicative)
    {
        float prev;

        // if its multiplicative, the previous value was the divided amount
        if (multiplicative)
        {
            prev = speedBuff.Value / amount;
        }
        // if additive then it is minus the amount
        else
        {
            prev = speedBuff.Value - amount;
        }

        // call an update to the velocity contribution
        UpdateVelocityContribution(prev);

        // passes on the update to the classes
        foreach (Class c in classes)
        {
            c.OnSpeedBuffUpdate(amount, multiplicative);
        }
    }

    /// <summary>
    /// Called by Buff Manager when attackSpeedBuff is changed
    /// </summary>
    /// <param name="amount">the value of the change by the attack speed buff</param>
    /// <param name="multiplicative">Whether its a value added to attack speed (false) or a scaler (true)</param>
    private void AttackSpeedBuffUpdate(float amount, bool multiplicative)
    {
        // passes on the update to all the classes
        foreach (Class c in classes)
        {
            c.OnAttackSpeedBuffUpdate(amount, multiplicative);
        }
    }

    /// <summary>
    /// Sets the snake's colour to the values 'r', 'g' and 'b'
    /// </summary>
    internal void ResetColour()
    {
        // sets the new colour to the values
        color = new Color(r, g, b);

        // displays that colour to the renderer
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Takes in a json file and converts it to the data for all the levels of the body
    /// </summary>
    /// <param name="json">The path of the json file</param>
    /// <returns></returns>
    internal void JsonToBodyData()
    {
        // gets the text from the file
        StreamReader reader = new StreamReader(jsonFile);
        string text = reader.ReadToEnd();
        reader.Close();

        // sets the jsonData to the deserialized json in the appropriate format
        jsonData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(text);

        // gets the max number of levels from the json
        maxLevel = jsonData.Count;
    }

    /// <summary>
    /// Reloads the snake's variables from the json file
    /// </summary>
    internal void LoadFromJson()
    {
        // gets the values that need to be set for this level
        Dictionary<string, object> values = jsonData[level - 1];

        // if the json has been loaded already
        if (jsonLoaded)
        {
            // sets the values for each term
            if (values.ContainsKey(nameof(defence)))
            {
                Defence = (int)Convert.ChangeType(values[nameof(defence)], typeof(int));
            }
            if (values.ContainsKey(nameof(maxHealth)))
            {
                MaxHealth = (int)Convert.ChangeType(values[nameof(maxHealth)], typeof(int));
            }
            if (values.ContainsKey(nameof(velocityContribution)))
            {
                VelocityContribution = (float)Convert.ChangeType(values[nameof(velocityContribution)], typeof(float));
            }
        }
        else
        {
            // sets the values for each term
            values.Setup(ref defence, nameof(defence));
            values.Setup(ref maxHealth, nameof(maxHealth));
            values.Setup(ref velocityContribution, nameof(velocityContribution));

            // indicates that the json has been loaded
            jsonLoaded = true;
        }

        // sets up the other variables
        values.Setup(ref contactDamage, nameof(contactDamage));
        values.Setup(ref contactForce, nameof(contactForce));
        values.Setup(ref r, nameof(r));
        values.Setup(ref g, nameof(g));
        values.Setup(ref b, nameof(b));
        values.Setup(ref maxLevel, nameof(maxLevel));
        values.Setup(ref name, nameof(name));

        // sets the colour of the body based on the r, g, b values
        ResetColour();
    }

    /// <summary>
    /// Called when the body levels up
    /// </summary>
    internal virtual void LevelUp()
    {
        // increases the level
        level++;

        // levels up each of the attatched classes
        foreach (Class friendly in classes)
        {
            friendly.LevelUp();
        }

        // reloads the body's values from the json
        LoadFromJson();

        // if its at max level, indicates it cant level further
        if (level == maxLevel)
        {
            levelable = false;
        }

        // calls the level up trigger
        TriggerManager.BodyLevelUpTrigger.CallTrigger(level);
    }
}
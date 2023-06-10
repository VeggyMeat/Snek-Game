using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // all defined by the classes that inherit the body controller

    // unbuffed versions, just to inherit from jsons (has to be public because of that, very dangerous)
    public int defence;
    public int maxHealth;
    public float velocityContribution;

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

    public float DamageMultiplier
    {
        get
        {
            return damageBuff.Value;
        }
    }

    internal string className;

    public int contactDamage;
    public int contactForce;

    internal Buff healthBuff;
    internal Buff speedBuff;
    internal Buff damageBuff;
    internal Buff defenceBuff;
    internal Buff attackSpeedBuff;

    internal Color color;

    public float r;
    public float g;
    public float b;

    internal float timeDead = 30f;

    internal Rigidbody2D selfRigid;
    internal SpriteRenderer spriteRenderer;

    internal BodyController next;
    internal BodyController prev;
    internal HeadController snake;

    internal int health;

    internal Vector2 lastMoved;
    internal Vector2 lastPosition;

    internal bool isDead = false;

    private Queue<Vector2> positionFollow = new Queue<Vector2>();

    // sets up variables
    internal virtual void Setup()
    {
        // updates the total mass of the snake
        snake.velocity += velocityContribution;
        health = maxHealth;

        // sets the color of the object
        color = new Color(r, g, b);
        spriteRenderer.color = color;

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

        // calls the trigger saying a new body was added
        TriggerManager.BodySpawnTrigger.CallTrigger(this);
    }

    // function called when a new body is created
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

    // returns whether the body is the head of the snake
    internal bool IsHead()
    {
        return prev is null;
    }

    // adds a new body to the snake
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

    // returns the position of the body in the snake (un-used)
    internal int Position()
    {
        if (prev is null)
        {
            return 0;
        }

        return prev.Position() + 1;
    }

    // used to calculate the total length of the snake, returns the length of body parts from it
    internal int Length()
    {
        if (next is null)
        {
            return 1;
        }
        else
        {
            return 1 + next.Length();
        }
    }

    internal Vector2 TailPos()
    {
        if (next is null)
        {
            return transform.position;
        }

        return next.TailPos();
    }

    // checks if body is dead or has too much HP (returns whether the body is still alive)
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

    // returns whether the body survives or not
    internal virtual bool ChangeHealth(int quantity)
    {
        if (quantity > 0)
        {
            // increase health trigger (ASSUMES NO HEALING WILL MAKE YOU TAKE DAMAGE)
            quantity = TriggerManager.BodyGainedHealthTrigger.CallTriggerReturn(quantity);

            health += quantity;
        }
        else if (quantity < 0)
        {
            // reduce the damage taken by the defence
            quantity += Defence;

            // if the body ignores damage from defence, return it survived
            if (quantity > 0)
            {
                return true;
            }

            // lost health trigger (ASSUMES IT WILL NOT MAKE BODY HEAL)
            quantity = TriggerManager.BodyLostHealthTrigger.CallTriggerReturn(quantity);

            health += quantity;
        }

        return HealthChangeCheck();
    }

    internal virtual void OnDeath()
    {
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

        // body died trigger called
        TriggerManager.BodyDeadTrigger.CallTrigger(gameObject);
    }

    internal virtual void Revived()
    {
        isDead = false;

        // updates the total mass of the snake
        snake.velocity += VelocityContribution;
        health = MaxHealth;

        // changes the tag back so that enemies can deal damage
        gameObject.tag = "Player";

        // returns the body back to normal color
        Color oldColor = spriteRenderer.color;
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);

        // stops it from being revived again if its a premature revive (not implemented yet)
        CancelInvoke(nameof(Revived));
    }

    // currently un-used, would be used if body needs to be removed
    internal void DestroySelf()
    {
        // removes itself from the linkedList
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

    // moves the body
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

    private void UpdateVelocityContribution(float prev)
    {
        snake.velocity -= prev;
        snake.velocity += VelocityContribution;
    }

    // functions that get called by the respective buffs
    internal virtual void HealthBuffUpdate(float amount, bool multiplicative)
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
    }

    internal virtual void SpeedBuffUpdate(float amount, bool multiplicative)
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
    }

    internal virtual void AttackSpeedBuffUpdate(float amount, bool multiplicative)
    {

    }
}
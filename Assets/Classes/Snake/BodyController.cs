using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    // all defined by the classes that inherit the body controller
    public int defence;
    public int maxHealth;
    public int contactDamage;
    public int contactForce;
    public float velocityContribution;

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
        // updates the total mass of the snake (partially unused right now)
        snake.totalMass += selfRigid.mass;
        snake.velocity += velocityContribution;
        health = maxHealth;

        // sets the color of the object
        color = new Color(r, g, b);
        spriteRenderer.color = color;
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

    // returns the position of the body in the snake
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

    // returns whether the body survives or not
    internal bool ChangeHealth(int quantity)
    {
        if (quantity > 0)
        {
            health += quantity;

            // increase health trigger

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else if (quantity < 0)
        {
            // reduce the damage taken by the defence
            quantity += defence;

            // if the body ignores damage from defence, return it survived
            if (quantity > 0)
            {
                return true;
            }

            health += quantity;

            // lost health trigger

            if (health <= 0)
            {
                health = 0;
                OnDeath();

                return false;
            }
        }

        return true;
    }

    internal virtual void OnDeath()
    {
        isDead = true;

        // reverts the original additions from the body
        snake.totalMass -= selfRigid.mass;
        snake.velocity -= velocityContribution;

        // changes the tag so enemies wont interact with it
        gameObject.tag = "Dead";

        // makes the body slightly transparent to indicate death
        Color oldColor = spriteRenderer.color;
        GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.4f);

        // makes the body revive in timeDead seconds
        Invoke(nameof(Revived), timeDead);

        // body died trigger called
        TriggerController.bodyDied(gameObject);
    }

    internal virtual void Revived()
    {
        isDead = false;

        // updates the total mass of the snake (partially unused right now)
        snake.totalMass += selfRigid.mass;
        snake.velocity += velocityContribution;
        health = maxHealth;

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
        snake.totalMass -= selfRigid.mass;
        snake.velocity -= velocityContribution;

        // destroys this body
        Destroy(gameObject);
    }

    // moves the body
    internal void Move(Vector2 place = new Vector2())
    {
        // if head
        if (prev is null)
        {
            Debug.Log(Length());
            // get the vector its moved in the last frame
            Vector2 movement = snake.velocityVector * Time.deltaTime / snake.Length();

            // add it to the list for the next snake to follow
            if (next is not null)
            {
                positionFollow.Enqueue(movement + selfRigid.position);
            }

            selfRigid.MovePosition(movement + selfRigid.position);

            if (next is not null)
            {
                if (positionFollow.Count > snake.frameDelay)
                {
                    next.Move(positionFollow.Dequeue());
                }
            }
        }
        // if not
        else
        {
            selfRigid.MovePosition(place);

            if (next is not null)
            {
                positionFollow.Enqueue(place);

                if (positionFollow.Count > snake.frameDelay)
                {
                    next.Move(positionFollow.Dequeue());
                }
            }
        }
    }
}

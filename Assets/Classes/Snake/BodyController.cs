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

    internal TriggerController triggerController;

    internal bool isDead = false;

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
    internal void BodySetup(HeadController snake, BodyController prev, TriggerController controller)
    {
        // sets up the starting variables for the body
        this.snake = snake;
        this.prev = prev;
        triggerController = controller;

        // sets the position and grabs the rigid body
        selfRigid = gameObject.GetComponent<Rigidbody2D>();
        selfRigid.position = new Vector2(0, 0);

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
    internal void AddBody(GameObject obj, HeadController snake)
    {
        if (next is null)
        {
            // creates the body and sets it up and places it as the head of the snake
            GameObject body = Instantiate(obj);

            // randomly choses one of the options
            int choice = UnityEngine.Random.Range(5, 6);

            if (choice == 0)
            {
                // makes the body a bowman (TEMPORARY)
                body.AddComponent<BowMan>();
            }
            else if (choice == 1)
            {
                // makes the body a necro (TEMPORARY)
                body.AddComponent<Necro>();
            }
            else if (choice == 2)
            {
                // makes the body a swordsman (TEMPORARY)
                body.AddComponent<Swordsman>();
            }
            else if (choice == 3)
            {
                // make the body a fire mage (TEMPORARY)
                body.AddComponent<FireMage>();
            }
            else if (choice == 4)
            {
                body.AddComponent<Samurai>();
            }
            else if (choice == 5)
            {
                body.AddComponent<Gambler>();
            }

            next = body.GetComponent<BodyController>();
            next.BodySetup(snake, this, triggerController);
        }
        else
        {
            // makes the next body add a new body behind it
            next.AddBody(obj, snake);
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
        triggerController.bodyDied(gameObject);
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
    internal void Move(float totalMass=0.1f, List<Rigidbody2D> objects = null, List<Vector2> positions = null, float prevRadius = 0f)
    {
        // updates the total mass seen so far
        if (!isDead)
        {
            totalMass += selfRigid.mass;
        }

        // gets the radius of the current circle
        float radius = transform.localScale.x / 2;

        // if it is the head, create the lists to pass on to the next instance, and move the head based on the angle facing and speed
        if (IsHead())
        {
            objects = new List<Rigidbody2D>() { selfRigid };

            positions = new List<Vector2>() { snake.velocityVector * Time.deltaTime / totalMass + selfRigid.position };
        }
        // if not the head
        else
        {
            // calculates the distance the objects should be apart
            float targetDistance = radius + prevRadius;

            // calculates the current distance apart as a vector
            Vector2 diff = positions[positions.Count - 1] - selfRigid.position;

            // gets the magnitude of that
            float distance = diff.magnitude;

            // figures out the error between actual distance and desired distance
            float error = targetDistance - distance;

            // normalizes the difference vector
            Vector2 diffNormalized = diff.normalized;

            // calculates the weighting in which side should be shifted more
            float weight = selfRigid.mass / totalMass;

            // updates the position changes of each of the objects in the list, to be moved
            for (int i = 0; i < positions.Count; i++)
            {
                // changes the position based off of the weight, error and the Vector between the two objects
                positions[i] += diffNormalized * error * weight;
            }

            // adds this objects position to the list, updated based on the inverse weighting, error and the Vector between the two objects
            positions.Add(selfRigid.position - (1 - weight) * error * diffNormalized);

            // adds this object to the list
            objects.Add(selfRigid);
        }

        if (next is not null)
        {
            // makes the next body move, passing on the lists
            next.Move(totalMass, objects, positions, radius);
        }
        else
        {
            // actually updates the positions based on the list of where they should be
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].MovePosition(positions[i]);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public int defence = 0;
    public int maxHealth = 100;
    public int contactDamage = 25;
    public int contactForce = 2000;
    public double velocityContribution = 5;

    internal Rigidbody2D selfRigid;

    internal BodyController? next;
    internal BodyController? prev;
    internal HeadController snake;
    internal int health;

    internal Vector2 lastMoved;
    internal Vector2 lastPosition;

    // kinda like start, but called on creation, rather than before first update
    public void Setup(HeadController snake, BodyController? prev)
    {
        // sets up the starting variables for the body
        health = maxHealth;
        this.snake = snake;
        selfRigid = gameObject.GetComponent<Rigidbody2D>();
        selfRigid.position = new Vector2(0, 0);
        this.prev = prev;

        // updates the total mass of the snake (unused right now)
        snake.totalMass += selfRigid.mass;
        snake.velocity += velocityContribution;
    }

    void Start()
    {

    }

    void Update()
    {

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
            // creates the new body and sets it up
            GameObject nextObj = Instantiate(obj);
            next = nextObj.GetComponent<BodyController>();
            next.Setup(snake, this);
            next.gameObject.AddComponent<BowMan>();
            next.gameObject.GetComponent<BowMan>().Setup();
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
            quantity = Math.Abs(quantity) - defence;

            if (quantity < 0)
            {
                return true;
            }

            health -= quantity;
            
            // lost health trigger

            if (health <= 0)
            {
                // death trigger

                health = 0;
                DestroySelf();

                return false;
            }
        }

        return true;
    }

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

        // destroys this body
        Destroy(gameObject);
    }

    // moves the body
    internal void Move(float totalMass=0, List<Rigidbody2D> objects = null, List<Vector2> positions = null, float prevRadius = 0f)
    {
        // updates the total mass seen so far
        totalMass += selfRigid.mass;

        // gets the radius of the current circle
        float radius = transform.localScale.x / 2;

        // if it is the head, create the lists to pass on to the next instance, and move the head based on the angle facing and speed
        if (IsHead())
        {
            objects = new List<Rigidbody2D>() { selfRigid };

            positions = new List<Vector2>() { snake.velocityVector * Time.deltaTime / selfRigid.mass + selfRigid.position };
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

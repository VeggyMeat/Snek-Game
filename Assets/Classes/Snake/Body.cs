using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Diagnostics;

public class SnakeBody
{
    internal SnakeBody? next;
    internal SnakeBody? prev;
    internal HeadController snake;
    internal bool isInstantiated;
    internal GameObject physicalBody;
    internal int health;
    internal int defence;
    internal int maxHealth;
    readonly double lerpSpeed;

    public SnakeBody(SnakeBody? prev, HeadController snake, double lerpSpeed)
    {
        this.prev = prev;
        this.snake = snake;
        this.lerpSpeed = lerpSpeed;
        isInstantiated = false;
    }

    internal void InstantiateSelf(GameObject body)
    {
        // incomplete

        physicalBody = body;

        physicalBody.transform.position = new Vector3(0, 0, 0);

        physicalBody.GetComponent<BodyController>().selfBody = this;

        isInstantiated = true;
    }

    internal void InstantiateBody(GameObject body)
    {
        if (isInstantiated)
        {
            if (next is null)
            {
                throw new AlreadyInstantiatedException(Position());
            }

            next.InstantiateBody(body);
        }
        else
        {
            InstantiateSelf(body);
        }
    }

    internal bool IsHead()
    {
        return prev is null;
    }

    internal int Place()
    {
        if (prev is null)
        {
            return 1;
        }

        return prev.Place();
    }
    
    internal void AddBody(double lerpSpeed)
    {
        if (next is null)
        {
            next = new SnakeBody(this, snake, lerpSpeed);
        }
        else
        {
            next.AddBody(lerpSpeed);
        }
    }

    internal int Position()
    {
        if (prev is null)
        {
            return 0;
        }

        return prev.Position() + 1;
    }

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
        health += quantity;

        if (quantity > 0)
        {
            // increase health trigger

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else if (quantity < 0)
        {
            // lost health trigger

            if (health < 0)
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
        // todo: needs to delete the physical object

        if (next is not null)
        {
            next.prev = prev;
        }
        if (prev is not null)
        {
            prev.next = next;
        }
    }

    internal void Move()
    {
        if (IsHead())
        {
            physicalBody.transform.position += snake.velocityVector * Time.deltaTime;
        }
        else
        {
            float targetDistance = 1.0f;

            Vector3 diff = prev.physicalBody.transform.position - physicalBody.transform.position;

            float distance = diff.magnitude;

            float error = targetDistance - distance;

            Vector3 diffNormalized = diff.normalized;

            physicalBody.transform.position -= diffNormalized * error;
        }
    }
}

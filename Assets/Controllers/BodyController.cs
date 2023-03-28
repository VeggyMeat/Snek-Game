using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform selfTransform;

    internal BodyController? next;
    internal BodyController? prev;
    internal HeadController snake;
    internal int health;
    internal int defence;
    internal int maxHealth;

    void Start()
    {
        selfTransform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        Move();
    }

    void FixedUpdate()
    {

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

    internal void AddBody(GameObject obj, HeadController snake)
    {
        if (next is null)
        {
            GameObject nextObj = Instantiate(obj);

            next = nextObj.GetComponent<BodyController>();

            next.prev = this;

            next.snake = snake;
        }
        else
        {
            next.AddBody(obj, snake);
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
            selfTransform.position += snake.velocityVector * Time.deltaTime;
        }
        else
        {
            float targetDistance = 1.0f;

            Vector3 diff = prev.selfTransform.position - selfTransform.position;

            float distance = diff.magnitude;

            float error = targetDistance - distance;

            Vector3 diffNormalized = diff.normalized;

            selfTransform.position -= diffNormalized * error;
        }
    }
}

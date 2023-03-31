using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    internal Rigidbody2D selfRigid;

    internal BodyController? next;
    internal BodyController? prev;
    internal HeadController snake;
    internal int health;
    internal int defence;
    internal int maxHealth;

    internal Vector2 lastMoved;
    internal Vector2 lastPosition;

    // kinda like start, but called on creation, rather than before first update
    public void Setup(HeadController snake, BodyController? prev)
    {
        this.snake = snake;

        selfRigid = gameObject.GetComponent<Rigidbody2D>();

        selfRigid.position = new Vector2(0, 0);

        snake.totalMass += selfRigid.mass;

        this.prev = prev;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        lastMoved = (selfRigid.position - lastPosition) / Time.deltaTime;
        lastPosition = selfRigid.position;
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

            next.Setup(snake, this);
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

        Destroy(gameObject);
    }

    internal void Move(float totalMass=0, List<Rigidbody2D> objects = null, List<Vector2> positions = null)
    {
        totalMass += selfRigid.mass;

        if (IsHead())
        {
            objects = new List<Rigidbody2D>() { selfRigid };

            positions = new List<Vector2>() { snake.velocityVector * Time.deltaTime / selfRigid.mass + selfRigid.position };
        }
        else
        {
            float targetDistance = 1.0f;

            Vector2 diff = prev.selfRigid.position - selfRigid.position;

            float distance = diff.magnitude;

            float error = targetDistance - distance;

            Vector2 diffNormalized = diff.normalized;

            float weight = selfRigid.mass / totalMass;

            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] += diffNormalized * error * weight;
            }

            positions.Add(selfRigid.position - (1 - weight) * error * diffNormalized);

            objects.Add(selfRigid);
        }

        if (next is not null)
        {
            next.Move(totalMass, objects, positions);
        }
        else
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].MovePosition(positions[i]);
            }
        }
    }
}

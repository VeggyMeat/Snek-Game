using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public double turningRate = 2;
    public double velocity = 3;
    public double baseLerpSpeed = 5;

    public GameObject circle;

    internal BodyController? head;

    internal double angle;
    internal Vector2 velocityVector;

    private bool pressed = false;

    internal float totalMass = 0;

    void Start()
    {
        velocityVector = new Vector2(0f, 0f);

        for (int i = 0; i < 1; i++)
        {
            AddBody(circle);
            head.gameObject.AddComponent<BowMan>();
            head.gameObject.GetComponent<BowMan>().Setup();
        }
    }

    private void Update()
    {
        // temp way to add bodies
        if (Input.GetKey(KeyCode.Space))
        {
            if (!pressed)
            {
                pressed = true;
                AddBody(circle);
            }
        }
        else
        {
            pressed = false;
        }
    }

    private void FixedUpdate()
    {
        // incomplete

        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        if (rightPress && leftPress)
        {

        }
        else if (rightPress)
        {
            angle += turningRate * Time.deltaTime;

            velocityVector = new Vector2((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)));
        }

        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;


            velocityVector = new Vector2((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)));
        }

        head.Move();

        transform.position = HeadPos();
    }

    public Vector2 HeadPos()
    {
        if (head is null)
        {
            throw new Exception();
        }

        return head.transform.position;
    }

    public void AddBody(GameObject obj)
    {
        if (head is null)
        {
            GameObject body = Instantiate(obj);

            head = body.GetComponent<BodyController>();

            head.Setup(this, null);
        }
        else
        {
            head.AddBody(obj, this);
        }
    }

    public int Length()
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

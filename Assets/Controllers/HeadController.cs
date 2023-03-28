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
    private bool pressed = false;

    public GameObject circle;

    private BodyController? head;
    internal double angle;

    internal Vector3 velocityVector;

    void Start()
    {
        velocityVector = new Vector3((float)(velocity * Math.Sin(turningRate)), (float)(velocity * Math.Cos(turningRate)), 0);

        for (int i = 0; i < 1; i++)
        {
            AddBody(circle);
        }
    }

    private void Update()
    {
        transform.position = HeadPos();
    }

    private void FixedUpdate()
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

        // incomplete

        bool rightPress = Input.GetKey(KeyCode.RightArrow);
        bool leftPress = Input.GetKey(KeyCode.LeftArrow);

        if (rightPress && leftPress)
        {

        }
        else if (rightPress)
        {
            angle += turningRate * Time.deltaTime;

            velocityVector = new Vector3((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)), 0);
        }

        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;


            velocityVector = new Vector3((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)), 0);
        }
    }

    public Vector3 HeadPos()
    {
        if (head is null)
        {
            throw new Exception();
        }

        return head.selfTransform.position;
    }

    public void AddBody(GameObject obj)
    {
        if (head is null)
        {
            GameObject body = Instantiate(obj);

            head = body.GetComponent<BodyController>();

            head.prev = null;

            head.snake = this;
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

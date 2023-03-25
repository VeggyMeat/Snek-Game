using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Snake
{
    private SnakeBody? head;
    internal double angle;

    double turningRate;
    double velocity;

    internal Vector3 velocityVector;

    public Snake() : this(15, 10)
    {
        
    }

    public Snake(double turningRate, double velocity)
    {
        this.turningRate = turningRate;
        this.velocity = velocity;

        velocityVector = new Vector3((float)(velocity * Math.Sin(turningRate)), (float)(velocity * Math.Cos(turningRate)), 0);
    }

    public Vector3 HeadPos()
    {
        if (head is null)
        {
            throw new Exception();
        }

        return head.physicalBody.transform.position;
    }

    public void AddBody(double lerpSpeed)
    {
        if (head is null)
        {
            head = new SnakeBody(null, this, lerpSpeed);
        }
        else
        {
            head.AddBody(lerpSpeed);
        }
    }

    public void InstantiateBody(GameObject body)
    {
        // incomplete will have paramaters of data passed through

        if (head is null)
        {
            throw new AlreadyInstantiatedException(-1);
        }

        head.InstantiateBody(body);
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

    public bool FixedUpdate()
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

            velocityVector = new Vector3((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)), 0);
        }

        else if (leftPress)
        {
            angle -= turningRate * Time.deltaTime;


            velocityVector = new Vector3((float)(velocity * Math.Sin(angle)), (float)(velocity * Math.Cos(angle)), 0);
        }

        return head is not null;
    }

    public void Update()
    {
        if (head is not null)
        {
            head.Update();
        }
    }
}

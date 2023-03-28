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

    private SnakeBody? head;
    internal double angle;

    internal Vector3 velocityVector;

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

    void Start()
    {
        velocityVector = new Vector3((float)(velocity * Math.Sin(turningRate)), (float)(velocity * Math.Cos(turningRate)), 0);

        for (int i = 0; i < 1; i++)
        {
            AddBody(baseLerpSpeed);
            InstantiateBody(Instantiate(circle));
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
                AddBody(baseLerpSpeed);
                InstantiateBody(Instantiate(circle));
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
}

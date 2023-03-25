using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    Snake snake;
    public double turningRate;
    public double velocity;
    public double baseLerpSpeed;
    public bool pressed;

    public GameObject circle;
    public GameObject square;

    // Start is called before the first frame update
    void Start()
    {
        snake = new Snake(turningRate, velocity);
        for (int i = 0; i < 1; i++)
        {
            snake.AddBody(baseLerpSpeed);
            snake.InstantiateBody(Instantiate(circle));
        }
    }

    private void Update()
    {
        snake.Update();
        transform.position = snake.HeadPos();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {
            if (!pressed)
            {
                pressed = true;
                snake.AddBody(baseLerpSpeed);
                snake.InstantiateBody(Instantiate(circle));
            }
        }
        else
        {
            pressed = false;
        }
        snake.FixedUpdate();
    }
}

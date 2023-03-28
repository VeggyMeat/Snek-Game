using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public Transform selfTransform;
    public SnakeBody selfBody;

    void Update()
    {
        selfBody.Move();
    }

    void FixedUpdate()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlreadyInstantiatedException : Exception
{
    public int snakePosition;

    public AlreadyInstantiatedException(int snakePosition) : base()
    {
        this.snakePosition = snakePosition;
    }
}

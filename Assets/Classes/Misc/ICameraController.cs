using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraController
{
    public void SetGameSetup(IGameSetup gameSetup);
    public Transform Transform { get; }

}

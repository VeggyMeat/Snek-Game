using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReorganiser
{
    public void SetGameSetup(IGameSetup gameSetup);
    public bool Active { get; set; }
}

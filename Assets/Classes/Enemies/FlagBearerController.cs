using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBearerController : EnemyController
{
    public int speedBuff;
    public GameObject flag;
    public float flagTimeDelay;
    public int flagRange;
    public int flagLifeSpan;

    internal override void Setup()
    {
        base.Setup();
    }

    internal override void Die()
    {
        // creates the new flag
        GameObject summonedFlag = Instantiate(flag, transform.position, Quaternion.identity);
        summonedFlag.GetComponent<FlagController>().Setup(flagTimeDelay, flagRange, speedBuff, flagLifeSpan);

        // calls the normal death parameters
        base.Die();
    }
}

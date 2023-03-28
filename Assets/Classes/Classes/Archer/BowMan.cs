using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowMan : Archer
{
    // Update is called once per frame
    void Update()
    {

    }

    public override void LaunchProjectile()
    {
        Random.RandomRange(0, Mathf.PI);
    }
}

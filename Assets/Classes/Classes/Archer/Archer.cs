using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public GameObject projectile;
    public float timeDelay;
    public 

    void Start()
    {
        InvokeRepeating("LaunchProjectile", timeDelay, timeDelay);
    }

    void Update()
    {
        
    }

    public virtual void LaunchProjectile()
    {

    }
}

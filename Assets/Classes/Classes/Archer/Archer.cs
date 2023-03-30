using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    internal float timeDelay;
    internal float velocity;
    internal GameObject projectile;
    internal float lifeSpan;

    void Start()
    {
        InvokeRepeating("LaunchProjectile", timeDelay, timeDelay);
    }

    void Update()
    {
        
    }

    public virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }
}

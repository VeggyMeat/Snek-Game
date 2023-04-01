using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Archer : MonoBehaviour
{
    internal float timeDelay;
    internal float velocity;
    internal GameObject projectile;
    internal float lifeSpan;
    internal int projectileDamage;
    internal int enemiesKilled;
    internal int xp;

    void Start()
    {
        // sets up starting variables
        enemiesKilled = 0;
        xp = 0;

        // runs the LaunchProjectile function every timeDelay seconds
        InvokeRepeating("LaunchProjectile", timeDelay, timeDelay);
    }

    void Update()
    {
        
    }

    // creates a base case incase not implemented
    public virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }

    // called when an enemy is killed by a created projectile
    public virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;
        xp += enemy.GetComponent<EnemyControllerBasic>().XPDrop;
    }
}

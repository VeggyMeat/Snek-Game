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
    internal List<int> levelUps;
    internal int level = 1;
    internal BodyController body;

    internal virtual void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;
        xp = 0;
        body = GetComponent<BodyController>();

        // starts firing the projectiles
        StartRepeatingProjectile();
    }

    void Update()
    {
        
    }

    internal void ResetRepeatingProjectile()
    {
        // stops the current repeat
        CancelInvoke();

        // starts a new one
        StartRepeatingProjectile();
    }

    // runs the LaunchProjectile function every timeDelay seconds
    internal void StartRepeatingProjectile()
    {
        // InvokeRepeating(nameof(LaunchProjectile), timeDelay, timeDelay);
    }

    // creates a base case incase not implemented
    internal virtual void LaunchProjectile()
    {
        throw new System.NotImplementedException();
    }

    // called when an enemy is killed by a created projectile
    internal virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;
        xp += enemy.GetComponent<EnemyControllerBasic>().XPDrop;

        if (levelUps.Count > 0 )
        {
            if (xp >= levelUps[0])
            {
                levelUps.RemoveAt(0);
                LevelUp();
            }
        }
    }

    internal virtual void LevelUp()
    {
        level++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class : BodyController
{
    internal int enemiesKilled;
    internal int level = 1;

    // Called when the class is created
    internal override void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;

        // calls the setup for the body
        base.Setup();
    }

    // called when an enemy is killed by the class object
    internal virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        snake.IncreaseXP(enemy.GetComponent<EnemyControllerBasic>().XPDrop);
    }

    // called when an enemy dies, and this object is in the trigger list in TriggerController
    internal virtual void EnemyKilledTrigger(GameObject enemy)
    {
        
    }

    // called when a player body dies, and this object is in the trigger list in TriggerController
    internal virtual void BodyDiedTrigger(GameObject body)
    {
        
    }

    // called when the class levels up, overrided by above classes
    internal virtual void LevelUp()
    {

    }
}

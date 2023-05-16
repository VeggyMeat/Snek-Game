using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class : MonoBehaviour
{
    internal int enemiesKilled;
    internal int level = 1;
    internal BodyController body;

    // Start is called before the first frame update
    internal virtual void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;
        body = GetComponent<BodyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // called when an enemy is killed by the class object
    internal virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        body.snake.IncreaseXP(enemy.GetComponent<EnemyControllerBasic>().XPDrop);
    }

    // called when an enemy dies, and this object is in the trigger list in TriggerController
    internal virtual void EnemyKilledTrigger(GameObject enemy)
    {
        
    }

    // called when a player body dies, and this object is in the trigger list in TriggerController
    internal virtual void BodyDiedTrigger(GameObject body)
    {
        
    }
}

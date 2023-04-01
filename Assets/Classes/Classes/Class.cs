using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class : MonoBehaviour
{
    internal int enemiesKilled;
    internal int xp;
    internal List<int> levelUps;
    internal int level = 1;
    internal BodyController body;

    // Start is called before the first frame update
    internal virtual void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;
        xp = 0;
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
        xp += enemy.GetComponent<EnemyControllerBasic>().XPDrop;

        if (levelUps.Count > 0)
        {
            if (xp >= levelUps[0])
            {
                levelUps.RemoveAt(0);
                LevelUp();
            }
        }
    }

    // called when level has reached xp threshold
    internal virtual void LevelUp()
    {
        level++;
    }
}

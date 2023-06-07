using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        snake.IncreaseXP(enemy.GetComponent<EnemyController>().XPDrop);
    }

    // called when the class levels up, overrided by above classes
    internal virtual void LevelUp()
    {

    }

    internal void JsonSetup(string json)
    {
        // loads in all the variables from the json
        StreamReader reader = new StreamReader(json);
        string text = reader.ReadToEnd();
        reader.Close();

        JsonUtility.FromJsonOverwrite(text, this);
    }
}

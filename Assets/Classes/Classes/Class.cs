using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Class: MonoBehaviour
{
    internal int enemiesKilled;
    internal int level = 1;
    internal BodyController body;
    internal string jsonPath;

    // Called by ClassSetup, overwritten by all inheriting classes
    internal virtual void Setup()
    {
        // sets up starting variables
        enemiesKilled = 0;
    }

    // Called when the class is created
    internal virtual void ClassSetup()
    {
        JsonSetup(jsonPath);

        body.JsonSetup(jsonPath);
    }

    // called when an enemy is killed by the class object
    internal virtual void EnemyKilled(GameObject enemy)
    {
        // increases the enemy killed count, and the xp count
        enemiesKilled++;

        body.snake.IncreaseXP(enemy.GetComponent<EnemyController>().XPDrop);
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

    internal virtual void Revived()
    {

    }

    internal virtual void OnDeath()
    {

    }

    internal virtual void OnHealthBuffUpdate(float amount, bool multiplicative)
    {

    }

    internal virtual void OnSpeedBuffUpdate(float amount, bool multiplicative)
    {

    }

    internal virtual void OnAttackSpeedBuffUpdate(float amount, bool multiplicative)
    {

    }
}

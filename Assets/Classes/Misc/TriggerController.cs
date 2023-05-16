using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    // triggers that are needed:
    // - on the death of an enemy, gives location and copy of enemy
    // - on the death of a body, gives location and copy of player

    private List<Class> enemyDeathTriggered;
    private List<Class> bodyDeathTriggered;
    
    void Start()
    {
        enemyDeathTriggered = new List<Class>();
        bodyDeathTriggered = new List<Class>();
    }

    void Update()
    {
        
    }

    internal void addEnemyDeathTrigger(Class body)
    {
        enemyDeathTriggered.Add(body);
    }

    internal void addBodyDeathTrigger(Class body)
    {
        bodyDeathTriggered.Add(body);
    }

    internal void removeEnemyDeathTrigger(Class body)
    {
        enemyDeathTriggered.Remove(body);
    }

    internal void removeBodyDeathTrigger(Class body)
    {
        bodyDeathTriggered.Remove(body);
    }

    // possibly quite slow for thousands of enemies
    internal void enemyDied(GameObject enemy)
    {
        foreach (Class body in enemyDeathTriggered)
        {
            body.EnemyKilledTrigger(enemy);
        }
    }

    internal void bodyDied(GameObject body)
    {
        foreach (Class player in bodyDeathTriggered)
        {
            player.BodyDiedTrigger(body);
        }
    }
}

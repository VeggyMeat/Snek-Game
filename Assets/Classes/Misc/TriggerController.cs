using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriggerController
{
    // triggers that are needed:
    // - on the death of an enemy, gives location and copy of enemy
    // - on the death of a body, gives location and copy of player

    private static List<Class> enemyDeathTriggered = new List<Class>();
    private static List<Class> bodyDeathTriggered = new List<Class>();
    internal static void addEnemyDeathTrigger(Class body)
    {
        enemyDeathTriggered.Add(body);
    }

    internal static void addBodyDeathTrigger(Class body)
    {
        bodyDeathTriggered.Add(body);
    }

    internal static void removeEnemyDeathTrigger(Class body)
    {
        enemyDeathTriggered.Remove(body);
    }

    internal static void removeBodyDeathTrigger(Class body)
    {
        bodyDeathTriggered.Remove(body);
    }

    // possibly quite slow for thousands of enemies
    internal static void enemyDied(GameObject enemy)
    {
        foreach (Class body in enemyDeathTriggered)
        {
            body.EnemyKilledTrigger(enemy);
        }
    }

    internal static void bodyDied(GameObject body)
    {
        foreach (Class player in bodyDeathTriggered)
        {
            player.BodyDiedTrigger(body);
        }
    }
}

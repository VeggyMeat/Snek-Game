using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySummonerController : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;

    public int spawnClumps;
    public float spawnDelay;
    public float firstDelay;

    public float insideHeight;
    public float insideWidth;
    public float outsideHeight;
    public float outsideWidth;

    public GameObject triggerController;

    internal Transform cameraTransform;
    internal List<GameObject> enemies;
    internal int enemiesDead = 0;

    // Start is called before the first frame update
    void Start()
    {
        // gets the camera object
        cameraTransform = GameObject.Find("Main Camera").transform;

        // triggers the SpawnEnemies function every spawnDelay seconds
        InvokeRepeating(nameof(SpawnEnemies), firstDelay, spawnDelay);

        // creates a list to store the enemies summoned
        enemies = new List<GameObject>();
    }

    void Update()
    {
        
    }
    
    // spawns a number of enemies in random locations
    void SpawnEnemies()
    {
        for (int i = 0; i < spawnClumps; i++)
        {
            // random 1 or -1
            int xSign1 = Random.Range(0, 2) * 2 - 1;
            int xSign2 = Random.Range(0, 2) * 2 - 1;

            // creates a random position within the ranges defined
            Vector3 spawnPosition;

            if (Random.Range(0, 2) == 0)
            {
                spawnPosition = new Vector3(xSign1 * Random.Range(insideWidth, outsideWidth) + cameraTransform.position.x, xSign2 * Random.Range(0, outsideHeight) + cameraTransform.position.y, 0);
            }
            else
            {
                spawnPosition = new Vector3(xSign1 * Random.Range(0, outsideWidth) + cameraTransform.position.x, xSign2 * Random.Range(insideHeight, outsideHeight) + cameraTransform.position.y, 0);
            }

            // spawns an enemy at that random position
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPosition, Quaternion.identity);

            // sets the enemy's summoner to this
            EnemyController newEnemy = enemy.GetComponent<EnemyController>();
            newEnemy.summoner = this;
            newEnemy.Setup();

            // adds the enemy to the list of enemies to keep track
            enemies.Add(enemy);
        }
    }
}

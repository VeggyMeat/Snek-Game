using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

public class EnemySummonerController : MonoBehaviour
{
    /// <summary>
    /// All the enemy games objects to be spawned
    /// </summary>
    [SerializeField] private List<GameObject> unsortedEnemyPrefabs;

    private Dictionary<string, List<GameObject>> enemyPrefabs;

    /// <summary>
    /// The delay in seconds between the rounds of enemies spawning
    /// </summary>
    [SerializeField] private float spawnDelay;

    /// <summary>
    /// The delay in seconds before the first round of enemies spawn
    /// </summary>
    [SerializeField] private float firstDelay;

    /// <summary>
    /// The radius of the circle around the player that enemies spawn on
    /// </summary>
    [SerializeField] private float radius;

    internal string jsonPath = "Assets/Resources/Jsons/Enemies/EnemyWaves.json";

    private Transform cameraTransform;

    /// <summary>
    /// The number of enemies that have died
    /// </summary>
    internal int enemiesDead = 0;

    private List<Dictionary<string, int>> enemyData = new List<Dictionary<string, int>>();

    /// <summary>
    /// The current round of the game
    /// </summary>
    private int round = 0;

    /// <summary>
    /// TEMPORARY makes the game loop and adds a multiplier to the number of enemies spawned
    /// </summary>
    private int extra = 1;

    private int enemiesSpawned = 0;

    void Start()
    {
        // gets enemyData from the json file
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        enemyData = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(text);

        // gets the camera object
        cameraTransform = GameObject.Find("Main Camera").transform;

        // triggers the SpawnEnemies function every spawnDelay seconds
        InvokeRepeating(nameof(RoundSpawn), firstDelay, spawnDelay);

        // sorts the enemy prefabs into a dictionary
        enemyPrefabs = new Dictionary<string, List<GameObject>>()
        {
            {"Small", new List<GameObject>()},
            {"Medium", new List<GameObject>()},
            {"Large", new List<GameObject>()},
            {"Special", new List<GameObject>()}
        };

        // goes through each prefab
        foreach (GameObject enemyPrefab in unsortedEnemyPrefabs)
        
        {
            // gets the enemy type for that prefab
            string type = enemyPrefab.GetComponent<EnemyController>().EnemyType;

            // adds that prefab into the right dictionary
            enemyPrefabs[type].Add(enemyPrefab);
        }
    }

    private void RoundSpawn()
    {
        // if the round is greater than the number of rounds, add one to the multiplier and reset the round
        if (round >= enemyData.Count)
        {
            extra++;
            round %= enemyData.Count;
        }

        // grabs the data for the next round
        Dictionary<string, int> roundData = enemyData[round];

        // for each type of enemy
        foreach(KeyValuePair<string, int> pair in roundData)
        {
            // spawn a random enemy of that size, that many times
            SpawnEnemies(enemyPrefabs[pair.Key][Random.Range(0, enemyPrefabs[pair.Key].Count)], pair.Value * extra);
        }

        // increases the round for next time
        round++;
    }
    
    // spawns a number of enemies in random locations
    private void SpawnEnemies(GameObject enemyPrefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            // gets a random angle
            float angle = Random.Range(0, Mathf.PI * 2);

            // gets coordinates of that angle on the circle around
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 2);

            // spawns an enemy at that random position
            GameObject enemy = Instantiate(enemyPrefab, pos + cameraTransform.position + new Vector3 (0, 0, 6), Quaternion.identity);

            // sets up the enemy
            EnemyController newEnemy = enemy.GetComponent<EnemyController>();
            newEnemy.Setup(this, enemiesSpawned);

            // increases the number of enmies spawned
            enemiesSpawned++;
        }
    }

    /// <summary>
    /// Called by enemies when the enemy despawns
    /// </summary>
    /// <param name="enemy">The enemy that despawned</param>
    internal void EnemyDespawned(EnemyController enemy)
    {
        // spawns an enemy to replace it
        SpawnEnemies(enemyPrefabs[enemy.EnemyType][Random.Range(0, enemyPrefabs[enemy.EnemyType].Count)], 1);
    }
}

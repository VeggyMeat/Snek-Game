using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;

public class EnemySummonerController : MonoBehaviour
{
    public List<GameObject> unsortedEnemyPrefabs;

    private Dictionary<string, List<GameObject>> enemyPrefabs;

    public int spawnClumps;
    public float spawnDelay;
    public float firstDelay;

    public float radius;

    internal string jsonPath = "Assets/Resources/Jsons/Enemies/EnemyWaves.json";

    private Transform cameraTransform;
    internal int enemiesDead = 0;

    private List<Dictionary<string, int>> enemyData = new List<Dictionary<string, int>>();

    private int round = 0;
    private int extra = 1;

    // Start is called before the first frame update
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

        foreach (GameObject enemyPrefab in unsortedEnemyPrefabs)
        {
            string type = enemyPrefab.GetComponent<EnemyController>().enemyType;

            enemyPrefabs[type].Add(enemyPrefab);
        }
    }

    private void RoundSpawn()
    {
        if (round >= enemyData.Count)
        {
            extra++;
            round %= enemyData.Count;
        }

        Dictionary<string, int> roundData = enemyData[round];

        // for each type of enemy
        foreach(KeyValuePair<string, int> pair in roundData)
        {
            // spawn a random enemy of that size, that many times
            SpawnEnemies(enemyPrefabs[pair.Key][Random.Range(0, enemyPrefabs[pair.Key].Count)], pair.Value * extra);
        }
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
            GameObject enemy = Instantiate(enemyPrefab, pos + cameraTransform.position, Quaternion.identity);

            // sets the enemy's summoner to this
            EnemyController newEnemy = enemy.GetComponent<EnemyController>();
            newEnemy.summoner = this;
            newEnemy.Setup();
        }
    }

    // called when an enemy despawns
    internal void EnemyDespawned(EnemyController enemy)
    {
        // spawns an enemy to replace it
        SpawnEnemies(enemyPrefabs[enemy.enemyType][Random.Range(0, enemyPrefabs[enemy.enemyType].Count)], 1);
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

// COMPLETE

/// <summary>
/// The enemy summoner controller, placed on a void game object
/// Spawns enemies in waves
/// </summary>
public class EnemySummonerController : MonoBehaviour, IEnemySummonerController
{
    /// <summary>
    /// All the enemy games objects to be spawned
    /// </summary>
    [SerializeField] private List<GameObject> unsortedEnemyPrefabs;

    /// <summary>
    /// The dictionary of enemy prefabs, sorted by type
    /// </summary>
    private Dictionary<string, List<GameObject>> enemyPrefabs;

    /// <summary>
    /// The delay in seconds between the rounds of enemies spawning
    /// </summary>
    private const float spawnDelay = 15f;

    /// <summary>
    /// The delay in seconds before the first round of enemies spawn
    /// </summary>
    private const float firstDelay = 5f;

    /// <summary>
    /// The radius of the circle around the player that enemies spawn on
    /// </summary>
    private const float radius = 20f;

    /// <summary>
    /// The path to the json file with the enemy waves
    /// </summary>
    private string jsonPath = "Assets/Resources/Jsons/Enemies/EnemyWaves.json";

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the gameSetup of the snake
    /// </summary>
    /// <param name="gameSetup"></param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// The number of enemies that have died
    /// </summary>
    private int enemiesDead = 0;

    /// <summary>
    /// The number of enemies that have died
    /// </summary>
    public int EnemiesDead => enemiesDead;

    /// <summary>
    /// Called when an enemy dies, to increase the number of enemies dead
    /// </summary>
    public void EnemyDied()
    {
        enemiesDead++;
    }

    /// <summary>
    /// The data for the enemies to spawn
    /// </summary>
    private List<Dictionary<string, int>> enemyData = new List<Dictionary<string, int>>();

    /// <summary>
    /// The current round of the game
    /// </summary>
    private int round = 0;

    /// <summary>
    /// The extra multiplier for the number of enemies spawned
    /// </summary>
    private int extra = 1;

    /// <summary>
    /// The total number of enemies spawned
    /// </summary>
    private int enemiesSpawned = 0;

    /// <summary>
    /// The health multiplier for the enemies
    /// </summary>
    private int HealthMultiplier
    {
        get
        {
            return (int)Mathf.Pow(2f, extra - 1);
        }
    }

    /// <summary>
    /// The damage multiplier for the enemies
    /// </summary>
    private int DamageMultiplier
    {
        get
        {
            return (int)Mathf.Pow(2f, extra - 1);
        }
    }

    // Called by unity before the first frame
    private void Start()
    {
        // gets enemyData from the json file
        StreamReader reader = new StreamReader(jsonPath);
        string text = reader.ReadToEnd();
        reader.Close();

        enemyData = JsonConvert.DeserializeObject<List<Dictionary<string, int>>>(text);

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

    /// <summary>
    /// Spawns the next round of enemies
    /// </summary>
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
    
    /// <summary>
    /// Spawns a number of enemies in random locations
    /// </summary>
    /// <param name="enemyPrefab">The enemy prefab to spawn</param>
    /// <param name="number">The number of enemies to spawn</param>
    private void SpawnEnemies(GameObject enemyPrefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            // gets a random angle
            float angle = Random.Range(0, Mathf.PI * 2);

            // gets coordinates of that angle on the circle around
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 2);

            // spawns an enemy at that random position
            GameObject enemy = Instantiate(enemyPrefab, pos + gameSetup.CameraController.Transform.position + new Vector3 (0, 0, 6), Quaternion.identity);

            // sets up the enemy
            EnemyController newEnemy = enemy.GetComponent<EnemyController>();
            newEnemy.Setup(this);

            // adds the damage and health buffs to the enemy
            newEnemy.damageBuff.AddBuff(DamageMultiplier, true, null);
            newEnemy.healthBuff.AddBuff(HealthMultiplier, true, null);

            // increases the number of enmies spawned
            enemiesSpawned++;
        }
    }

    /// <summary>
    /// Called by enemies when the enemy despawns
    /// </summary>
    /// <param name="enemyController">The enemy that despawned</param>
    public void EnemyDespawned(EnemyController enemyController)
    {
        // spawns an enemy to replace it
        SpawnEnemies(enemyPrefabs[enemyController.EnemyType][Random.Range(0, enemyPrefabs[enemyController.EnemyType].Count)], 1);
    }
}

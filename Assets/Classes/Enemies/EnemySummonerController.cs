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

    internal Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;
        InvokeRepeating("SpawnEnemies", firstDelay, spawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < spawnClumps; i++)
        {
            // random 1 or -1
            int xSign1 = Random.Range(0, 2) * 2 - 1;
            int xSign2 = Random.Range(0, 2) * 2 - 1;

            Vector3 spawnPosition = new Vector3(xSign1 * Random.Range(insideWidth, outsideWidth) + cameraTransform.position.x, xSign2 * Random.Range(insideHeight, outsideHeight) + cameraTransform.position.y, 0);

            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnPosition, Quaternion.identity);
        }
    }
}

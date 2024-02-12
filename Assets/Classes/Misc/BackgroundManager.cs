using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject backgroundPrefab;
    [SerializeField] private HeadController headController;

    private Transform follow;

    public Dictionary<(int, int), GameObject> sprites = new Dictionary<(int, int), GameObject>();

    [SerializeField] private int tilesAroundX = 7;
    [SerializeField] private int tilesAroundY = 5;

    [SerializeField] private float tileSize = 1;

    private void Awake()
    {
        follow = headController.transform;
    }

    private List<(int, int)> GetTuples()
    {
        List<(int, int)> tuples = new List<(int, int)>();

        int xSpot = Mathf.RoundToInt(follow.position.x / tileSize);
        int ySpot = Mathf.RoundToInt(follow.position.y / tileSize);

        for (int x = xSpot - tilesAroundX; x < xSpot + tilesAroundX + 1; x++)
        {
            for (int y = ySpot - tilesAroundY; y < ySpot + tilesAroundY + 1; y++)
            {
                tuples.Add((x, y));
            }
        }

        return tuples;
    }

    private void FixedUpdate()
    {
        List<(int, int)> spots = GetTuples();
        List<(int, int)> toRemove = new List<(int, int)>();

        // get rid of all the tiles that are not in the spots
        foreach (KeyValuePair<(int, int), GameObject> sprite in sprites)
        {
            if (!spots.Contains(sprite.Key))
            {
                Destroy(sprite.Value);
                toRemove.Add(sprite.Key);
            }
        }

        foreach ((int, int) remove in toRemove)
        {
            sprites.Remove(remove);
        }

        // add all the tiles that are not in the spots
        foreach ((int, int) spot in spots)
        {
            if (!sprites.ContainsKey(spot))
            {
                GameObject newSprite = Instantiate(backgroundPrefab, new Vector3(spot.Item1 * tileSize, spot.Item2 * tileSize, 100), Quaternion.identity);
                newSprite.transform.parent = transform;
                sprites.Add(spot, newSprite);
            }
        }
    }
}

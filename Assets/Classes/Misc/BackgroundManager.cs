using System.Collections.Generic;
using UnityEngine;

// COMPLETE

/// <summary>
/// The background manager, responsible for managing the background tiles.
/// </summary>
public class BackgroundManager : MonoBehaviour, IBackgroundManager
{
    /// <summary>
    /// The prefab for a tile of the background
    /// </summary>
    [SerializeField] private GameObject backgroundPrefab;

    /// <summary>
    /// The transform to follow
    /// </summary>
    private Transform follow;

    /// <summary>
    /// The game setup
    /// </summary>
    private IGameSetup gameSetup;

    /// <summary>
    /// Sets the game setup
    /// </summary>
    /// <param name="gameSetup">The game setup</param>
    public void SetGameSetup(IGameSetup gameSetup)
    {
        this.gameSetup = gameSetup;
    }

    /// <summary>
    /// The dictionary of sprites, and their respective 'tile' positions
    /// </summary>
    private readonly Dictionary<(int, int), GameObject> sprites = new Dictionary<(int, int), GameObject>();

    /// <summary>
    /// How many tiles around the player to show in each direction on the y axis
    /// </summary>
    [SerializeField] private int tilesAroundX = 7;

    /// <summary>
    /// How many tiles around the player to show in each direction on the x axis
    /// </summary>
    [SerializeField] private int tilesAroundY = 5;

    /// <summary>
    /// The size of each tile (in unity units)
    /// </summary>
    [SerializeField] private float tileSize = 1;

    // Called as soon as the game starts
    private void Awake()
    {
        follow = gameSetup.HeadController.Transform;
    }

    /// <summary>
    /// Gets the tuples of the tiles around the player that should be shown
    /// </summary>
    /// <returns></returns>
    private List<(int, int)> GetTuples()
    {
        List<(int, int)> tuples = new List<(int, int)>();

        // get the spot the player is in (rounded to the nearest tile)
        int xSpot = Mathf.RoundToInt(follow.position.x / tileSize);
        int ySpot = Mathf.RoundToInt(follow.position.y / tileSize);

        // get all the tiles around the player
        for (int x = xSpot - tilesAroundX; x < xSpot + tilesAroundX + 1; x++)
        {
            for (int y = ySpot - tilesAroundY; y < ySpot + tilesAroundY + 1; y++)
            {
                tuples.Add((x, y));
            }
        }

        return tuples;
    }

    // Called every frame before physics
    private void FixedUpdate()
    {
        // get the spots
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

        // remove the tiles from the dictionary
        foreach ((int, int) remove in toRemove)
        {
            sprites.Remove(remove);
        }

        // add all the tiles that are not in the spots
        foreach ((int, int) spot in spots)
        {
            if (!sprites.ContainsKey(spot))
            {
                // create a new sprite
                GameObject newSprite = Instantiate(backgroundPrefab, new Vector3(spot.Item1 * tileSize, spot.Item2 * tileSize, 100), Quaternion.identity);
                
                // set the parent
                newSprite.transform.parent = transform;
                
                // add the sprite to the dictionary
                sprites.Add(spot, newSprite);
            }
        }
    }
}

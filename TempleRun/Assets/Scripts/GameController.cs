using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the main gameplay
/// </summary>
public class GameController : MonoBehaviour
{
    [Tooltip("A reference to the tile we want to spawn")]
    public Transform tile;

    [Tooltip("Where the first tile should be placed")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tile should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    /// <summary>
    /// Where the next tile should be spawned
    /// </summary>
    private Vector3 nextTileLocation;

    /// <summary>
    /// How should the next tile be rotated
    /// </summary>
    private Quaternion nextTileRotation;

    // Start is called before the first frame update
    void Start()
    {
        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for(int i = 0; i < initSpawnNum; i++)
        {
            SpawnNextTile();
        }
    }

    /// <summary>
    /// Will spawn a tile at a certain location and que the next position
    /// </summary>
    public void SpawnNextTile()
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
        var nextTile = newTile.Find("Next Spawn Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;
    }
}

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

    [Tooltip("A reference to the obstacle we want to spawn")]
    public Transform obstacle;

    [Tooltip("Where the first tile should be placed")]
    public Vector3 startPoint = new Vector3(0, 0, -5);

    [Tooltip("How many tile should we create in advance")]
    [Range(1, 15)]
    public int initSpawnNum = 10;

    [Tooltip("How many tiles we want to spawn initially with no obstacles")]
    public int initNoObstacles = 4;

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
        if (!GameObject.FindObjectOfType<UnityAdController>())
        {
            var adController = new GameObject("Unity Ad Manager");
            adController.AddComponent<UnityAdController>();
        }

        nextTileLocation = startPoint;
        nextTileRotation = Quaternion.identity;

        for(int i = 0; i < initSpawnNum; i++)
        {
            SpawnNextTile(i >= initNoObstacles);
        }
    }

    /// <summary>
    /// Will spawn a tile at a certain location and que the next position
    /// </summary>
    /// <param name="spawnObsctacles">If we should spawn an obstacle</param>
    public void SpawnNextTile(bool spawnObstacles = true)
    {
        var newTile = Instantiate(tile, nextTileLocation, nextTileRotation);
        var nextTile = newTile.Find("Next Spawn Point");
        nextTileLocation = nextTile.position;
        nextTileRotation = nextTile.rotation;
        if (spawnObstacles)
        {
            SpawnObstacle(newTile);
        }
    }

    private void SpawnObstacle(Transform newTile)
    {
        var obstacleSpawnPoints = new List<GameObject>();
        foreach(Transform child in newTile)
        {
            if (child.CompareTag("ObstacleSpawn"))
            {
                obstacleSpawnPoints.Add(child.gameObject);
            }
        }

        if(obstacleSpawnPoints.Count > 0)
        {
            var spawnPoint = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Count)];
            var spawnPosition = spawnPoint.transform.position;

            var newObstacle = Instantiate(obstacle, spawnPosition, Quaternion.identity);
            newObstacle.SetParent(spawnPoint.transform);
        }
    }
}

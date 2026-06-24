using System;
using UnityEngine;

public class BrickLevelSpawner : MonoBehaviour
{
    // Define your grid layout
    private int rows = 4;
    private int columns = 10;
    private Vector2 spacing = new Vector2(-1f, -0.8f);
    private Vector3 spawnOrigin = new Vector3(4.5f, 0.5f, -4.666f);
    public GameObject brickPrefab; // Assign the Brick prefab in the Inspector
    public LevelManager levelManager; // Assign the LevelManager in the Inspector
    public AudioManager audioManager; // Assign the AudioManager in the Inspector

    public void Start()
    {
        SpawnLevel();
    }

    public void SpawnLevel()
    {
        int currentBrickCount = 0;
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                // Calculate position based on grid index
                Vector3 spawnPos = new Vector3(
                    spawnOrigin.x + (x * spacing.x),
                    spawnOrigin.y,
                    spawnOrigin.z - (z * spacing.y)
                );

                GameObject brickObj = GameManager.Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                Brick brick = brickObj.GetComponent<Brick>();
                brick.Initialize(levelManager, audioManager);
                currentBrickCount++;
            }
        }
        levelManager.SetTotalBricks(currentBrickCount);
    }

}
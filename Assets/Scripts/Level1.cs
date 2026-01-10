using System;
using UnityEngine;

public class Level1
{
    private int currentBrickCount = 0;
    // Define your grid layout
    private int rows = 4;
    private int columns = 10;
    private Vector2 spacing = new Vector2(-1f, -0.8f);
    private Vector3 spawnOrigin = new Vector3(4.5f, 0.5f, -4.666f);

    public void SpawnNewLevel(GameObject brickPrefab)
    {
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

                GameManager.Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                currentBrickCount++;
            }
        }
    }

    public bool BrickDestroyed()
    {
        currentBrickCount--;
        return currentBrickCount <= 0;
    }
}
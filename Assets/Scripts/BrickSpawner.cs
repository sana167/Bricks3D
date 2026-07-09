using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnDelay = 4f;
    [SerializeField] private float maxSpawnDelay = 8f;

    private BrickLevelSpawner levelSpawner;
    private int column;

    public void Initialize(BrickLevelSpawner spawner, int columnIndex)
    {
        levelSpawner = spawner;
        column = columnIndex;
        Invoke(nameof(TrySpawnBrick), Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    private void TrySpawnBrick()
    {
        if (levelSpawner != null)
        {
            levelSpawner.SpawnRedBrickInColumn(column, transform.position);
        }

        Invoke(nameof(TrySpawnBrick), Random.Range(minSpawnDelay, maxSpawnDelay));
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
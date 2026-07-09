using TMPro;
using UnityEngine;

public class BrickLevelSpawner : MonoBehaviour
{
    // Define your grid layout
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 10;
    [SerializeField] private Vector2 spacing = new(-1f, -0.8f);
    [SerializeField] private Vector3 spawnOrigin = new(4.5f, 0.5f, -4.666f);
    private GameObject currentLevelBrick;
    private Transform brickParent;
    [SerializeField] private GameObject normalBrickPrefab; // Assign the Normal Brick prefab in the Inspector
    [SerializeField] private GameObject toughBrickPrefab; // Assign the Tough Brick prefab in the Inspector
    [SerializeField] private LevelManager levelManager; // Assign the LevelManager in the Inspector
    [SerializeField] private AudioManager audioManager; // Assign the AudioManager in the Inspector
    [SerializeField] private SunController sunController; // Assign the SunController in the Inspector
    [SerializeField] private TextMeshProUGUI levelText; // Assign the TextMeshProUGUI in the Inspector
    private int currentLevel = 1;
    [SerializeField] private GameObject spawnerBrickPrefab;
    [SerializeField] private int level3Rows = 4;

    private Brick[,] brickGrid;

    public void Start()
    {
        SpawnLevel(MainMenu.GetSelectedLevel());
    }

    public void SpawnLevel(int level)
    {
        currentLevel = level;

        if (level == 1)
        {
            currentLevelBrick = normalBrickPrefab;
            SpawnNormalLevel();
        }
        else if (level == 2)
        {
            currentLevelBrick = toughBrickPrefab;
            SpawnNormalLevel();
        }
        else if (level == 3)
        {
            SpawnSpawnerLevel();
        }
        else
        {
            Debug.LogWarning($"Unknown level {level}. Loading normal bricks.");
            currentLevelBrick = normalBrickPrefab;
            SpawnNormalLevel();
        }
    }

    private void SpawnNormalLevel()
    {
        if (brickParent != null)
        {
            Destroy(brickParent.gameObject);
        }

        GameObject parent = new("Bricks");
        brickParent = parent.transform;

        if (sunController != null)
        {
            sunController.RandomizeSun();
        }
        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }
        int currentBrickCount = 0;
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                // Calculate position based on grid index
                Vector3 spawnPos = new(
                    spawnOrigin.x + (x * spacing.x),
                    spawnOrigin.y,
                    spawnOrigin.z - (z * spacing.y)
                );

                GameObject brickObj = Instantiate(currentLevelBrick, spawnPos, Quaternion.identity, brickParent);
                if (brickObj.TryGetComponent<Brick>(out var brick))
                {
                    brick.Initialize(levelManager, audioManager);
                }
                currentBrickCount++;
            }
        }
        levelManager.SetTotalBricks(currentBrickCount);
    }

    private void SpawnSpawnerLevel()
    {
        if (brickParent != null)
        {
            Destroy(brickParent.gameObject);
        }

        GameObject parent = new("Bricks");
        brickParent = parent.transform;

        brickGrid = new Brick[columns, level3Rows];

        if (sunController != null)
        {
            sunController.RandomizeSun();
        }

        if (levelText != null)
        {
            levelText.text = $"Level {currentLevel}";
        }

        int currentBrickCount = 0;

        for (int x = 0; x < columns; x++)
        {
            Brick spawnerBrick = CreateBrick(spawnerBrickPrefab, x, 0);
            brickGrid[x, 0] = spawnerBrick;
            currentBrickCount++;

            BrickSpawner spawner = spawnerBrick.gameObject.AddComponent<BrickSpawner>();
            spawner.Initialize(this, x);
        }

        for (int x = 0; x < columns; x++)
        {
            for (int z = 1; z < level3Rows; z++)
            {
                Brick redBrick = CreateBrick(normalBrickPrefab, x, z);
                brickGrid[x, z] = redBrick;
                currentBrickCount++;
            }
        }

        levelManager.SetTotalBricks(currentBrickCount);
    }

    private Brick CreateBrick(GameObject prefab, int x, int z)
    {
        Vector3 spawnPos = GetGridPosition(x, z);

        GameObject brickObj = Instantiate(prefab, spawnPos, Quaternion.identity, brickParent);

        if (brickObj.TryGetComponent<Brick>(out var brick))
        {
            brick.Initialize(levelManager, audioManager);
            brick.SetGridInfo(this, x, z);
        }

        return brick;
    }

    private Vector3 GetGridPosition(int x, int z)
    {
        return new Vector3(
            spawnOrigin.x + (x * spacing.x),
            spawnOrigin.y,
            spawnOrigin.z - (z * spacing.y)
        );
    }

    public void SpawnRedBrickInColumn(int column, Vector3 throwStartPosition)
    {
        if (brickGrid == null)
            return;

        for (int z = level3Rows - 1; z >= 1; z--)
        {
            if (brickGrid[column, z] == null)
            {
                Vector3 targetPosition = GetGridPosition(column, z);

                GameObject brickObj = Instantiate(
                    normalBrickPrefab,
                    throwStartPosition,
                    Quaternion.identity,
                    brickParent
                );

                Brick redBrick = brickObj.GetComponent<Brick>();
                redBrick.Initialize(levelManager, audioManager);
                redBrick.SetGridInfo(this, column, z);

                brickGrid[column, z] = redBrick;
                levelManager.AddBrick();

                ThrownBrick thrownBrick = brickObj.AddComponent<ThrownBrick>();
                thrownBrick.ThrowTo(targetPosition);

                return;
            }
        }
    }

    public void ClearGridSlot(int x, int z)
    {
        if (brickGrid == null)
            return;

        if (x < 0 || x >= columns || z < 0 || z >= level3Rows)
            return;

        brickGrid[x, z] = null;
    }
}
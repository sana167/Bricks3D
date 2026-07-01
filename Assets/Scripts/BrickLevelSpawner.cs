using TMPro;
using UnityEngine;

public class BrickLevelSpawner : MonoBehaviour
{
    // Define your grid layout
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 10;
    [SerializeField] private Vector2 spacing = new Vector2(-1f, -0.8f);
    [SerializeField] private Vector3 spawnOrigin = new Vector3(4.5f, 0.5f, -4.666f);
    private GameObject currentLevelBrick;
    private Transform brickParent;
    [SerializeField] private GameObject normalBrickPrefab; // Assign the Normal Brick prefab in the Inspector
    [SerializeField] private GameObject toughBrickPrefab; // Assign the Tough Brick prefab in the Inspector
    [SerializeField] private LevelManager levelManager; // Assign the LevelManager in the Inspector
    [SerializeField] private AudioManager audioManager; // Assign the AudioManager in the Inspector
    [SerializeField] private SunController sunController; // Assign the SunController in the Inspector
    [SerializeField] private TextMeshProUGUI levelText; // Assign the TextMeshProUGUI in the Inspector
    private int currentLevel = 1;

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
        }
        else if (level == 2)
        {
            currentLevelBrick = toughBrickPrefab;
        }
        else
        {
            Debug.LogWarning($"Unknown level {level}. Loading normal bricks.");
            currentLevelBrick = normalBrickPrefab;
        }
        SpawnLevel();
    }

    private void SpawnLevel()
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
                Vector3 spawnPos = new Vector3(
                    spawnOrigin.x + (x * spacing.x),
                    spawnOrigin.y,
                    spawnOrigin.z - (z * spacing.y)
                );

                GameObject brickObj = Instantiate(currentLevelBrick, spawnPos, Quaternion.identity, brickParent);
                Brick brick = brickObj.GetComponent<Brick>();
                if (brick != null)
                {
                    brick.Initialize(levelManager, audioManager);
                }
                currentBrickCount++;
            }
        }
        levelManager.SetTotalBricks(currentBrickCount);
    }

}
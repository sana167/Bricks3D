using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Brick : MonoBehaviour
{
    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private int hitPoints = 1;
    [SerializeField] private Material damagedMaterial;
    [SerializeField] private GameObject aimAssistPowerUpPrefab;
    [SerializeField] private GameObject electricBallPowerUpPrefab;
    [SerializeField] private GameObject bombPowerUpPrefab;
    [SerializeField] private float powerUpDropChance = 0.2f;

    private Renderer brickRenderer;
    private bool isDestroyed;
    private BrickLevelSpawner brickLevelSpawner;
    private int gridX = -1;
    private int gridZ = -1;

    public void SetGridInfo(BrickLevelSpawner spawner, int x, int z)
    {
        brickLevelSpawner = spawner;
        gridX = x;
        gridZ = z;
    }

    private void Awake()
    {
        brickRenderer = GetComponent<Renderer>();
    }

    public void Initialize(LevelManager gm, AudioManager am)
    {
        levelManager = gm;
        audioManager = am;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball")) return;

        BallMovement ball = collision.gameObject.GetComponent<BallMovement>();
        bool electricHit = ball != null && ball.IsElectric;

        TakeHit(electricHit);
    }

    public void DestroyFromPowerUp()
    {
        hitPoints = 0;
        BreakBrick(false);
    }

    private void TakeHit(bool instantDestroy)
    {
        if (audioManager != null)
        {
            audioManager.PlayShatter();
        }

        if (instantDestroy)
        {
            hitPoints = 0;
        }
        else
        {
            hitPoints--;
        }

        if (hitPoints > 0)
        {
            ShowDamage();
            return;
        }

        BreakBrick(true);
    }

    private void ShowDamage()
    {
        if (damagedMaterial != null && brickRenderer != null)
        {
            brickRenderer.material = damagedMaterial;
        }

        if (damagePrefab != null)
        {
            Instantiate(damagePrefab, transform.position, Quaternion.identity);
        }
    }

    private void BreakBrick(bool canDropPowerUp)
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (canDropPowerUp)
        {
            TryDropPowerUp();
        }

        if (brickLevelSpawner != null)
        {
            brickLevelSpawner.ClearGridSlot(gridX, gridZ);
        }

        Destroy(gameObject);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (levelManager != null)
            levelManager.BrickDestroyed();
    }

    private void TryDropPowerUp()
    {
        if (Random.value > powerUpDropChance)
            return;

        GameObject powerUpPrefab = PickPowerUpPrefab();
        if (powerUpPrefab == null)
            return;

        Instantiate(powerUpPrefab,
                transform.position + Vector3.up * 0.1f,
                powerUpPrefab.transform.rotation);
    }

    private GameObject PickPowerUpPrefab()
    {
        GameObject[] availablePowerUps =
        {
            aimAssistPowerUpPrefab,
            electricBallPowerUpPrefab,
            bombPowerUpPrefab
        };

        int availableCount = 0;
        foreach (GameObject powerUpPrefab in availablePowerUps)
        {
            if (powerUpPrefab != null)
            {
                availableCount++;
            }
        }

        if (availableCount == 0)
            return null;

        int selectedIndex = Random.Range(0, availableCount);
        foreach (GameObject powerUpPrefab in availablePowerUps)
        {
            if (powerUpPrefab == null)
                continue;

            if (selectedIndex == 0)
                return powerUpPrefab;

            selectedIndex--;
        }

        return null;
    }
}
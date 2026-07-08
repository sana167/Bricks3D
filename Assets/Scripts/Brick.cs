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
    [SerializeField] private float powerUpDropChance = 0.2f;

    private Renderer brickRenderer;

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
        if (audioManager != null)
        {
            audioManager.PlayShatter();
        }

        if (electricHit)
        {
            hitPoints = 0;
        }
        else
        {
            hitPoints--;
        }

        if (hitPoints > 0)
        {
            if (damagedMaterial != null && brickRenderer != null)
            {
                brickRenderer.material = damagedMaterial;
            }

            if (damagePrefab != null)
            {
                Instantiate(damagePrefab, transform.position, Quaternion.identity);
            }

            return;
        }

        if (Random.value <= powerUpDropChance)
        {
            GameObject powerUpPrefab = null;
            if (aimAssistPowerUpPrefab != null && Random.value <= 0.5f)
            {
                powerUpPrefab = aimAssistPowerUpPrefab;
            } else if (electricBallPowerUpPrefab != null)
            {
                powerUpPrefab = electricBallPowerUpPrefab;
            }
            if (powerUpPrefab != null)
            {
                Instantiate(powerUpPrefab,
                        transform.position + Vector3.up * 0.1f,
                        powerUpPrefab.transform.rotation);
            }
        }

        Destroy(gameObject);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (levelManager != null)
            levelManager.BrickDestroyed();
    }
}
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
    [SerializeField] private float aimAssistDropChance = 0.2f;

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

        if (audioManager != null)
        {
            audioManager.PlayShatter();
        }

        hitPoints--;

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
        
        if (aimAssistPowerUpPrefab != null && Random.value <= aimAssistDropChance)
        {
            Instantiate(aimAssistPowerUpPrefab,
                transform.position + Vector3.up * 0.1f,
                aimAssistPowerUpPrefab.transform.rotation);
        }

        Destroy(gameObject);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (levelManager != null)
            levelManager.BrickDestroyed();
    }
}
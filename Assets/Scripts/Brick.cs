using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject explosionPrefab;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private int hitPoints = 1;
    [SerializeField] private Material damagedMaterial;

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

        hitPoints--;

        if (hitPoints > 0)
        {
            if (damagedMaterial != null && brickRenderer != null)
            {
                brickRenderer.material = damagedMaterial;
            }

            return;
        }

        DestroyBrick();
    }

    private void DestroyBrick()
    {
        Destroy(gameObject);

        if (audioManager != null)
            audioManager.PlayShatter();

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (levelManager != null)
            levelManager.BrickDestroyed();
    }
}
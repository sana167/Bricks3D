using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab to instantiate on brick destruction
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioManager audioManager;

    public void Initialize(LevelManager gm, AudioManager am)
    {
        levelManager = gm;
        audioManager = am;
    }

    // Called when another collider enters this brick's collider (trigger is off)
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the Ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Destroy the brick
            Destroy(gameObject);
            if (audioManager != null)
            {
                audioManager.PlayShatter();
            }
            if (explosionPrefab != null)
            {
                // 2. Instantiate the prefab at the brick's current position and rotation
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            if (levelManager != null)
            {
                levelManager.BrickDestroyed();
            }
        }
    }
}
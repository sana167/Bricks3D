using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab to instantiate on brick destruction
    private GameManager gameManager; // Reference to the GameManager for playing sound
    
    void Start()
    {
        // Find the GameManager object when the scene starts
        GameObject managerObject = GameObject.FindWithTag("GameManager"); 
        if (managerObject != null)
        {
            gameManager = managerObject.GetComponent<GameManager>();
        }
    }

    // Called when another collider enters this brick's collider (trigger is off)
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the Ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (gameManager != null)
            {
                gameManager.PlaySFX(gameManager.shatterSFX);
            }
            if (explosionPrefab != null)
            {
                // 2. Instantiate the prefab at the brick's current position and rotation
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            if (gameManager != null)
            {
                gameManager.BrickDestroyed();
            }
            // Destroy the brick
            Destroy(gameObject);
        }
    }
}
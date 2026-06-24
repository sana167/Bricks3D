using UnityEngine;
using TMPro;

public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 10f; // Speed of the ball
    public float minSpeedX = 2f; // Minimum horizontal speed to prevent stalling
    public TextMeshProUGUI startInstructionText; // Reference to the instruction text UI
    public float deathZPosition; // Z position at which the ball is considered "missed"
    public Vector3 initialPosition; // Starting position of the ball
    public GameObject explosionPrefab; // Prefab to instantiate on ball destruction
    public AudioManager audioManager;
    private Rigidbody rb; // Rigidbody component of the ball
    public bool IsLaunched { get; private set; } = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = initialPosition;
    }

    public void TryLaunch()
    {
        if (!IsLaunched)
        {
            Launch();
            IsLaunched = true;
            
            if (startInstructionText != null)
            {
                startInstructionText.gameObject.SetActive(false);
            }
        }
    }

    void Launch()
    {
        // Apply an initial force in a general forward and upward direction
        Vector3 direction = new Vector3(Random.Range(-0.5f, 0.5f), 0, 1f).normalized;
        rb.linearVelocity = direction * initialSpeed;
    }

    void Update()
    {
        // Check if the ball has fallen below the death Z position
        if (transform.position.z > deathZPosition)
        {
            if (audioManager != null)
            {
                audioManager.PlayExplode();
            }
            if (explosionPrefab != null)
            {
                // Instantiate the explosion prefab at the ball's current position and rotation
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            ResetBall();
        }
    }

    public void ResetBall()
    {
        // 1. Stop all movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 2. Teleport the ball back to the starting position
        transform.position = initialPosition;
        
        // 3. Update the launch state
        IsLaunched = false;

        // 4. Show the instruction text again, ready for the next launch
        if (startInstructionText != null)
        {
            startInstructionText.gameObject.SetActive(true);
        }

        Debug.Log("Ball missed! Resetting position.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioManager != null && !collision.gameObject.CompareTag("Brick"))
        {
            audioManager.PlayBounce();
        }
    }
}
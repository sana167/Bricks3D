using UnityEngine;
using TMPro;

public class BallMovement : MonoBehaviour
{
    public float initialSpeed = 10f; // Speed of the ball
    public float minSpeedX = 2f; // Minimum horizontal speed to prevent stalling
    public TextMeshProUGUI startInstructionText; // Reference to the instruction text UI
    public float deathZPosition = 10f; // Z position at which the ball is considered "missed"
    public Vector3 initialPosition = new Vector3(0f, 0.5f, -1f); // Starting position of the ball
    public GameManager gameManager; // Reference to the GameManager for playing sound
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

    void Update()
    {
        // Check if the ball has fallen below the death Z position
        if (transform.position.z > deathZPosition)
        {
            ResetBall();
        }
    }

    void Launch()
    {
        // Apply an initial force in a general forward and upward direction
        Vector3 direction = new Vector3(Random.Range(-0.5f, 0.5f), 0, 1f).normalized;
        rb.linearVelocity = direction * initialSpeed;
    }

    private void ResetBall()
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
        if (gameManager != null && !collision.gameObject.CompareTag("Brick"))
        {
            gameManager.PlaySFX(gameManager.bounceSFX);
        }
        // --- FIX FOR HORIZONTAL STALLING ---

        // Get the current velocity of the ball
        Vector3 currentVelocity = rb.linearVelocity;

        // Check if the absolute X component is below the defined minimum
        if (Mathf.Abs(currentVelocity.x) < minSpeedX)
        {
            // 1. Determine the direction the ball was moving before correction
            float signX = Mathf.Sign(currentVelocity.x);
            
            // If the speed is near zero, use the last direction or default to positive if near perfect 0
            if (signX == 0) 
            {
                signX = 1; 
            }

            // 2. Calculate the new, corrected X velocity
            float correctedX = signX * minSpeedX;

            // 3. Set the new velocity while keeping the existing Y and Z components
            rb.linearVelocity = new Vector3(correctedX, currentVelocity.y, currentVelocity.z);

            // 4. Re-normalize the total velocity to prevent speed changes (optional, but good practice)
            rb.linearVelocity = rb.linearVelocity.normalized * initialSpeed; 
            
            Debug.Log("Corrected X velocity from " + currentVelocity.x + " to " + rb.linearVelocity.x);
        }
    }
}
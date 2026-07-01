using UnityEngine;
using TMPro;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f; // Speed of the ball
    [SerializeField] private TextMeshProUGUI startInstructionText; // Reference to the instruction text UI
    [SerializeField] private float deathZPosition; // Z position at which the ball is considered "missed"
    [SerializeField] private Vector3 initialPosition; // Starting position of the ball
    [SerializeField] private GameObject explosionPrefab; // Prefab to instantiate on ball destruction
    [SerializeField] private AudioManager audioManager;
    private Rigidbody rb; // Rigidbody component of the ball
    public bool IsLaunched { get; private set; } = false;
    [SerializeField] private float minBallSpeed = 8f;
    [SerializeField] private float maxBallSpeed = 12f;
    [SerializeField] private float minZSpeed = 2f;

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

        if (collision.gameObject.CompareTag("Paddle"))
        {
            AdjustBounceFromPaddle(collision);
        } else
        {
            rb.linearVelocity = rb.linearVelocity.normalized * initialSpeed;
        }
    }

    private void AdjustBounceFromPaddle(Collision collision)
    {
        float paddleX = collision.transform.position.x;
        float ballX = transform.position.x;
        float paddleWidth = collision.collider.bounds.size.x;

        float hitFactor = (ballX - paddleX) / (paddleWidth / 2f);
        hitFactor = Mathf.Clamp(hitFactor, -1f, 1f);

        float maxX = 1f;

        Vector3 newDirection = new Vector3(
            hitFactor * maxX,
            0f,
            -1f
        ).normalized;

        rb.linearVelocity = newDirection * initialSpeed;
    }

    private void FixedUpdate()
    {
        if (!IsLaunched) return;

        KeepBallMoving();
    }

    private void KeepBallMoving()
    {
        Vector3 velocity = rb.linearVelocity;

        if (velocity.magnitude < minBallSpeed)
        {
            velocity = velocity.normalized * minBallSpeed;
        }

        if (Mathf.Abs(velocity.z) < minZSpeed)
        {
            velocity.z = Mathf.Sign(velocity.z == 0 ? -1 : velocity.z) * minZSpeed;
        }

        rb.linearVelocity = Vector3.ClampMagnitude(velocity, maxBallSpeed);
    }
}
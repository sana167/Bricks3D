using UnityEngine;

public class AimAssistPowerUp : MonoBehaviour
{
    [SerializeField] private float lifetime = 12f;
    [SerializeField] private float bobHeight = 0.2f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float rotationSpeed = 90f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        AimAssistPreview preview = FindAnyObjectByType<AimAssistPreview>();

        if (preview != null)
        {
            preview.Activate();
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        // Rotate around the Y axis
        transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);

        // Bob up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}